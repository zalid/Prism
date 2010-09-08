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
    /// Implementation of <see cref="INavigationTargetHandler"/> that relies on a <see cref="IServiceLocator"/>
    /// to create new views when necessary.
    /// </summary>
    public class LocatorNavigationTargetHandler : INavigationTargetHandler
    {
        private readonly IServiceLocator serviceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocatorNavigationTargetHandler"/> class with a service locator.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public LocatorNavigationTargetHandler(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        /// <summary>
        /// Gets the view to which the navigation request represented by <paramref name="navigationContext"/> applies.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="navigationContext">The context representing the navigation request.</param>
        /// <returns>
        /// The view to be the target of the navigation request.
        /// </returns>
        /// <remarks>
        /// If none of the views in the region can be the target of the navigation request, a new view
        /// is created and added to the region.
        /// </remarks>
        /// <exception cref="ArgumentException">when a new view cannot be created for the navigation request.</exception>
        public object GetTargetView(IRegion region, NavigationContext navigationContext)
        {
            var typeName = UriParsingHelper.GetAbsolutePath(navigationContext.Uri);
            typeName = typeName.TrimStart('/');

            var candidates =
                region.Views.Where(v => string.Compare(v.GetType().Name, typeName, StringComparison.Ordinal) == 0);

            var acceptingCandidates =
                candidates.Where(
                    v =>
                    {
                        var navigationAware = v as INavigationAware;
                        if (navigationAware != null && !navigationAware.CanNavigateTo(navigationContext))
                        {
                            return false;
                        }

                        var frameworkElement = v as FrameworkElement;
                        if (frameworkElement == null)
                        {
                            return true;
                        }

                        navigationAware = frameworkElement.DataContext as INavigationAware;
                        return navigationAware == null || navigationAware.CanNavigateTo(navigationContext);
                    });


            var view = acceptingCandidates.FirstOrDefault();

            if (view != null)
            {
                return view;
            }

            try
            {
                view = this.serviceLocator.GetInstance<object>(typeName);
            }
            catch (ActivationException e)
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Resources.CannotCreateNavigationTarget, typeName),
                    e);
            }

            region.Add(view);

            return view;
        }
    }
}
