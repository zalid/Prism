//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Properties;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// Provides navigation for regions.
    /// </summary>
    public class RegionNavigationService : IRegionNavigationService
    {
        private readonly IServiceLocator serviceLocator;
        private readonly INavigationTargetHandler navigationTargetHandler;
        private IRegionNavigationJournal journal;
        private bool isNavigating;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNavigationService"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="navigationTargetHandler">The navigation target handler.</param>
        /// <param name="journal">The journal.</param>
        public RegionNavigationService(IServiceLocator serviceLocator, INavigationTargetHandler navigationTargetHandler, IRegionNavigationJournal journal)
        {
            if (serviceLocator == null)
            {
                throw new ArgumentNullException("serviceLocator");
            }

            if (navigationTargetHandler == null)
            {
                throw new ArgumentNullException("navigationTargetHandler");
            }

            if (journal == null)
            {
                throw new ArgumentNullException("journal");
            }

            this.serviceLocator = serviceLocator;
            this.navigationTargetHandler = navigationTargetHandler;
            this.journal = journal;
            this.journal.NavigationTarget = this;
        }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>The region.</value>
        public IRegion Region { get; set; }

        /// <summary>
        /// Gets the journal.
        /// </summary>
        /// <value>The journal.</value>
        public IRegionNavigationJournal Journal
        {
            get
            {
                return this.journal;
            }
        }

        /// <summary>
        /// Initiates navigation to the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="navigationCallback">A callback to execute when the navigation request is completed.</param>
        public void RequestNavigate(Uri source, Action<NavigationResult> navigationCallback)
        {
            try
            {
                DoNavigate(source, navigationCallback);
            }
            catch (Exception e)
            {
                navigationCallback(new NavigationResult(new NavigationContext(this, source), e));
            }
        }

        private void DoNavigate(Uri source, Action<NavigationResult> navigationCallback)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (this.Region == null)
            {
                throw new ArgumentNullException("region");
            }

            if (this.isNavigating)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.NavigationInProgress, this.Region.Name));
            }

            this.isNavigating = true;

            NavigationContext navigationContext = new NavigationContext(this, source);

            // starts querying the active views
            RequestCanNavigateFromOnCurrentlyActiveView(
                navigationContext,
                navigationCallback,
                this.Region.ActiveViews.ToArray(),
                0);
        }

        private void RequestCanNavigateFromOnCurrentlyActiveView(
            NavigationContext navigationContext,
            Action<NavigationResult> navigationCallback,
            object[] activeViews,
            int currentViewIndex)
        {
            if (currentViewIndex < activeViews.Length)
            {
                var vetoingView = activeViews[currentViewIndex] as INavigationAwareWithVeto;
                if (vetoingView != null)
                {
                    // the current active view implements INavigationAwareWithVeto, request confirmation
                    // providing a callback to resume the navigation request
                    vetoingView.RequestCanNavigateFrom(
                        navigationContext,
                        canNavigate =>
                        {
                            if (canNavigate)
                            {
                                RequestCanNavigateFromOnCurrentlyActiveViewModel(
                                    navigationContext,
                                    navigationCallback,
                                    this.Region.ActiveViews.ToArray(),
                                    currentViewIndex);
                            }
                            else
                            {
                                this.isNavigating = false;
                                navigationCallback(new NavigationResult(navigationContext, false));
                            }
                        });
                }
                else
                {
                    RequestCanNavigateFromOnCurrentlyActiveViewModel(
                        navigationContext,
                        navigationCallback,
                        this.Region.ActiveViews.ToArray(),
                        currentViewIndex);
                }
            }
            else
            {
                ExecuteNavigation(navigationContext, navigationCallback);
            }
        }

        private void RequestCanNavigateFromOnCurrentlyActiveViewModel(
            NavigationContext navigationContext,
            Action<NavigationResult> navigationCallback,
            object[] activeViews,
            int currentViewIndex)
        {
            var frameworkElement = activeViews[currentViewIndex] as FrameworkElement;

            if (frameworkElement != null)
            {
                var vetoingViewModel = frameworkElement.DataContext as INavigationAwareWithVeto;

                if (vetoingViewModel != null)
                {
                    // the data model for the current active view implements INavigationAwareWithVeto, request confirmation
                    // providing a callback to resume the navigation request
                    vetoingViewModel.RequestCanNavigateFrom(
                        navigationContext,
                        canNavigate =>
                        {
                            if (canNavigate)
                            {
                                RequestCanNavigateFromOnCurrentlyActiveView(
                                    navigationContext,
                                    navigationCallback,
                                    this.Region.ActiveViews.ToArray(),
                                    currentViewIndex + 1);
                            }
                            else
                            {
                                this.isNavigating = false;
                                navigationCallback(new NavigationResult(navigationContext, false));
                            }
                        });

                    return;
                }
            }

            RequestCanNavigateFromOnCurrentlyActiveView(
                navigationContext,
                navigationCallback,
                this.Region.ActiveViews.ToArray(),
                currentViewIndex + 1);
        }

        private void ExecuteNavigation(NavigationContext navigationContext, Action<NavigationResult> navigationCallback)
        {
            object view = this.navigationTargetHandler.GetTargetView(this.Region, navigationContext);

            this.Region.Activate(view);

            // Update the navigation journal before notifying others of navigaton
            IRegionNavigationJournalEntry journalEntry = this.serviceLocator.GetInstance<IRegionNavigationJournalEntry>();
            journalEntry.Uri = navigationContext.Uri;
            this.journal.RecordNavigation(journalEntry);

            // The view can be informed of navigation
            INavigationAware navigationAwareView = view as INavigationAware;
            if (navigationAwareView != null)
            {
                navigationAwareView.OnNavigatedTo(navigationContext);
            }

            // When using patterns like MVVM, the DataContext can also be informed of navigation.                        
            FrameworkElement viewFrameworkElement = view as FrameworkElement;
            if (viewFrameworkElement != null)
            {
                INavigationAware navigationAwareDataContext = viewFrameworkElement.DataContext as INavigationAware;
                if (navigationAwareDataContext != null)
                {
                    navigationAwareDataContext.OnNavigatedTo(navigationContext);
                }
            }

            this.isNavigating = false;
            navigationCallback(new NavigationResult(navigationContext, true));
        }
    }
}
