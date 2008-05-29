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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Prism.Interfaces;
using Prism.Properties;

namespace Prism.Regions
{
    public class SimpleRegion : IRegion
    {
        private readonly ObservableCollection<object> _innerCollection = new ObservableCollection<object>();
        private Dictionary<string, object> _namedViews = new Dictionary<string, object>();

        public SimpleRegion()
        {
            Views.CurrentChanged += Views_CurrentChanged;
        }

        void Views_CurrentChanged(object sender, EventArgs e)
        {
            foreach (object item in Views)
            {
                IActiveAware view = item as IActiveAware;
                if (view != null)
                    view.IsActive = false;
            }

            IActiveAware currentView = Views.CurrentItem as IActiveAware;
            if (currentView != null)
                currentView.IsActive = true;
        }

        private void InnerAdd(object view, string name, IRegionManager regionManager)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (_namedViews.ContainsKey(name))
                    throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, Resources.RegionViewNameExistsException, name));
                _namedViews.Add(name, view);
            }

            DependencyObject dependencyObject = view as DependencyObject;

            if (dependencyObject != null)
            {
                Regions.RegionManager.SetRegionManager(dependencyObject, regionManager);
            }

            if (_innerCollection.Contains(view))
                throw new InvalidOperationException(Resources.RegionViewExistsException);

            _innerCollection.Add(view);
        }


        public ICollectionView Views
        {
            get { return CollectionViewSource.GetDefaultView(_innerCollection); }
        }

        public IRegionManager Add(object view)
        {
            InnerAdd(view, null, this.RegionManager);
            return this.RegionManager;
        }

        public IRegionManager Add(object view, string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeNullOrEmpty, "viewName"));

            InnerAdd(view, viewName, this.RegionManager);
            return this.RegionManager;

        }

        public IRegionManager Add(object view, string viewName, bool createRegionManagerScope)
        {
            IRegionManager regionManager = createRegionManagerScope ? this.RegionManager.CreateRegionManager() : this.RegionManager;
            InnerAdd(view, viewName, regionManager);
            return regionManager;
        }

        public void Remove(object view)
        {
            if (!_innerCollection.Contains(view))
                throw new ArgumentException(Resources.ViewToRemoveNotInRegion, "view");

            _innerCollection.Remove(view);

            string viewName = GetNameFromView(view);
            if (viewName != null)
            {
                _namedViews.Remove(viewName);
            }
        }

        private string GetNameFromView(object view)
        {
            if (!_namedViews.ContainsValue(view))
                return null;

            foreach (var key in _namedViews.Keys)
            {
                if (_namedViews[key] == view)
                {
                    return key;
                }
            }
            return null;
        }

        public void Activate(object view)
        {
            if (!Views.Contains(view))
            {
                Add(view);
            }
            Views.MoveCurrentTo(view);
        }

        public object GetView(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeNullOrEmpty, "viewName"));

            if (_namedViews.ContainsKey(viewName))
            {
                return _namedViews[viewName];
            }
            return null;
        }

        public IRegionManager RegionManager { get; set; }
    }
}
