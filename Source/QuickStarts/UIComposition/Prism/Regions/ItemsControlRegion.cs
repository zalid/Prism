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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Interfaces;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Automation;
using Prism.Properties;
using System.Globalization;

namespace Prism.Regions
{
    public class ItemsControlRegion : IRegion<ItemsControl>
    {
        private Dictionary<string, UIElement> namedViews = new Dictionary<string, UIElement>();

        public ItemsControl WrappedControl { get; set; }

        #region IRegion Members

        public void Add(UIElement view)
        {
            WrappedControl.Items.Add(view);
            WrappedControl.InvalidateMeasure(); //Forces the wrapped control to resize
            if (WrappedControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                ApplyMetadata(view);

            Show(view);
        }

        public void Add(UIElement view, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(name);

            if (namedViews.ContainsKey(name))
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, Resources.RegionViewNameExistsException, name));

            Add(view);

            namedViews.Add(name, view);
        }

        public void Remove(UIElement view)
        {
            if (!WrappedControl.Items.Contains(view))
                throw new ArgumentException("The region does not contains the view you want to remove", "view");

            WrappedControl.Items.Remove(view);
            WrappedControl.InvalidateMeasure();

            string viewName = GetNameFromView(view);
            if (viewName != null)
            {
                namedViews.Remove(viewName);
            }
        }

        private string GetNameFromView(UIElement view)
        {
            if (!namedViews.ContainsValue(view))
                return null;

            foreach (var key in namedViews.Keys)
            {
                if (namedViews[key] == view)
                {
                    return key;
                }
            }
            return null;
        }

        public IList<UIElement> Views
        {
            get
            {
                List<UIElement> views = new List<UIElement>(WrappedControl.Items.Count);
                foreach (UIElement item in WrappedControl.Items)
                {
                    views.Add(item);
                }
                return views;
            }

        }

        public void Show(UIElement view)
        {
            if (!Views.Contains(view))
                throw new ArgumentException("The region does not contains the view you want to show", "view");

            WrappedControl.SetValue(Selector.SelectedItemProperty, view);
        }

        public UIElement GetView(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(name);

            if (namedViews.ContainsKey(name))
            {
                return namedViews[name];
            }
            return null;
        }

        #endregion

        public void Initialize(DependencyObject obj)
        {
            WrappedControl = (ItemsControl)obj;
            WrappedControl.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
            Selector selectorControl = obj as Selector;
            if (selectorControl != null)
            {
                selectorControl.SelectionChanged += new SelectionChangedEventHandler(selectorControl_SelectionChanged);
            }
        }

        //TODO: Metadata should not be enforced from the regions.
        //Remove this behavior from the framework
        private void ApplyMetadata(UIElement view)
        {
            IMetadataInfoProvider metadaInfoProviderView = view as IMetadataInfoProvider;
            if (metadaInfoProviderView != null)
            {
                DependencyObject itemContainer = WrappedControl.ItemContainerGenerator.ContainerFromItem(view);
                if (itemContainer == null)
                {
                    itemContainer = view;
                }
                IMetadataInfo metadataInfo = metadaInfoProviderView.GetMetadataInfo();
                itemContainer.SetValue(HeaderedContentControl.HeaderProperty, metadataInfo);
            }
        }

        void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (WrappedControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                WrappedControl.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
                foreach (var view in Views)
                {
                    ApplyMetadata(view);
                }
            }
        }

        void selectorControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source == WrappedControl))
                return;

            foreach (UIElement view in e.RemovedItems)
            {
                IActiveAware activeAwareView = view as IActiveAware;
                if (activeAwareView != null)
                {
                    activeAwareView.IsActive = false;
                }
            }
            foreach (UIElement view in e.AddedItems)
            {
                IActiveAware activeAwareView = view as IActiveAware;
                if (activeAwareView != null)
                {
                    activeAwareView.IsActive = true;
                }
            }
        }

    }


}
