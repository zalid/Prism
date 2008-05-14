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
using System.ComponentModel;
using System.Windows;
using Prism.Interfaces;

namespace StockTraderRI.Modules.WatchList.Tests
{
    public class MockRegionManager : IRegionManager
    {
        public Dictionary<string, IRegion> Regions = new Dictionary<string, IRegion>();

        public void Register(string regionName, IRegion region)
        {
            Regions.Add(regionName, region);
        }

        public IRegion GetRegion(string regionName)
        {
            return Regions[regionName];
        }

        #region Not implemented

        public bool HasRegion(string regionName)
        {
            throw new NotImplementedException();
        }

        public void CreateRegion(DependencyObject containerElement, string regionName)
        {
            throw new NotImplementedException();
        }

        public IRegionManager CreateRegionManager()
        {
            throw new NotImplementedException();
        }

        public void Unregister(string regionName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class MockRegion : IRegion
    {
        public List<object> AddedViews = new List<object>();

        #region IRegion Members

        public IRegionManager Add(object view)
        {
            AddedViews.Add(view);
            return null;
        }

        public void Remove(object view)
        {
            throw new NotImplementedException();
        }

        public ICollectionView Views
        {
            get { throw new NotImplementedException(); }
        }

        public void Show(object view)
        {
            throw new NotImplementedException();
        }

        public IRegionManager Add(object view, string name)
        {
            throw new NotImplementedException();
        }

        public object GetView(string name)
        {
            throw new NotImplementedException();
        }

        public IRegionManager Add(object view, string name, bool createRegionManagerScope)
        {
            throw new NotImplementedException();
        }

        public IRegionManager RegionManager
        {
            set { throw new NotImplementedException(); }
        }
        #endregion
    }
}
