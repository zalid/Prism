//===============================================================================
// Microsoft patterns & practices
// Composite WPF (PRISM)
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Properties;

namespace Microsoft.Practices.Composite.Wpf.Regions
{
    public class SelectorRegionAdapter : RegionAdapterBase<Selector>
    {
        protected override void Adapt(IRegion region, Selector regionTarget)
        {
            if (regionTarget.ItemsSource != null || (BindingOperations.GetBinding(regionTarget, ItemsControl.ItemsSourceProperty) != null))
                throw new InvalidOperationException(Resources.ItemsControlHasItemsSourceException);

            if (regionTarget.Items.Count > 0)
            {
                //Control must be empty before setting ItemsSource
                foreach (object childItem in regionTarget.Items)
                {
                    region.Add(childItem);
                }
                regionTarget.Items.Clear();
            }
            regionTarget.ItemsSource = region.Views;
        }

        protected override void AttachBehaviors(IRegion region, Selector regionTarget)
        {
            base.AttachBehaviors(region, regionTarget);

            //The coordinator uses weak references while listening to events to prevent memory leaks
            //when destroying the region but not the control or viceversa.
            SelectorRegionAdapterCoordinator coordinator = new SelectorRegionAdapterCoordinator(regionTarget, region);
            coordinator.Attach();
        }

        protected override IRegion CreateRegion()
        {
            return new Region();
        }

        private class SelectorRegionAdapterCoordinator
        {
            private readonly WeakReference _selectorWeakReference;
            private readonly WeakReference _regionWeakReference;

            public SelectorRegionAdapterCoordinator(Selector selector, IRegion region)
            {
                _selectorWeakReference = new WeakReference(selector);
                _regionWeakReference = new WeakReference(region);
            }

            public void Attach()
            {
                Selector selector = GetSelector();
                IRegion region = GetRegion();
                if (selector != null && region != null)
                {
                    selector.SelectionChanged += OnSelectionChanged;
                    region.ActiveViews.CollectionChanged += OnActiveViewsChanged;
                }
            }

            public void Detach()
            {
                Selector selector = GetSelector();
                if (selector != null)
                {
                    selector.SelectionChanged -= OnSelectionChanged;
                }
                IRegion region = GetRegion();
                if (region != null)
                {
                    region.ActiveViews.CollectionChanged -= OnActiveViewsChanged;
                }
            }

            private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                IRegion region = GetRegion();
                Selector selector = GetSelector();
                if (region == null || selector == null || selector.ItemsSource != region.Views)
                {
                    Detach();
                }
                else
                {
                    if (selector == e.OriginalSource)
                    {
                        foreach (object item in e.RemovedItems)
                        {
                            if (region.ActiveViews.Contains(item))
                            {
                                region.Deactivate(item);
                            }
                        }
                        foreach (object item in e.AddedItems)
                        {
                            region.Activate(item);
                        }
                    }
                }
            }

            private void OnActiveViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Selector selector = GetSelector();
                IRegion region = GetRegion();
                if (region == null || selector == null || selector.ItemsSource != region.Views)
                {
                    Detach();
                }
                else
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        selector.SelectedItem = e.NewItems[0];
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Remove &&
                             e.OldItems.Contains(selector.SelectedItem))
                    {
                        selector.SelectedItem = null;
                    }
                }
            }

            private Selector GetSelector()
            {
                return _selectorWeakReference.Target as Selector;
            }

            private IRegion GetRegion()
            {
                return _regionWeakReference.Target as IRegion;
            }
        }
    }
}