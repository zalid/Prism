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
using System.Windows;
using Prism.Regions;
using System.Windows.Controls;
using System.Globalization;
using System.ComponentModel;

namespace Prism.Services
{
    public class RegionManagerService : IRegionManagerService, IDisposable
    {
        private Dictionary<string, IRegion> _regions = new Dictionary<string, IRegion>();

        public RegionManagerService()
        {
            RegionManager.RegionPropertyChanged += RegionManager_RegionPropertyChanged;
        }

        void RegionManager_RegionPropertyChanged(object sender, RegionPropertyChangedEventArgs e)
        {
            SetRegion(e.ContainerElement, e.RegionName);
        }

        public void Register(string regionName, IRegion region)
        {
            _regions.Add(regionName, region);
        }

        public IRegion GetRegion(string regionName)
        {
            if (_regions.ContainsKey(regionName))
                return _regions[regionName];

            throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "{0} not found", regionName));
        }

        public bool HasRegion(string regionName)
        {
            return _regions.ContainsKey(regionName);
        }

        public void SetRegion(DependencyObject containerElement, string regionName)
        {
            IPrismContainer container = PrismContainerProvider.Provider;

            Type currentType = containerElement.GetType();

            while (currentType != typeof(DependencyObject))
            {
                IRegionAdapter region = (IRegionAdapter)container.TryResolve(typeof(IRegion<>).MakeGenericType(currentType));

                if (region != null)
                {
                    region.Initialize(containerElement);
                    _regions.Add(regionName, (IRegion)region);
                    return;
                }

                currentType = currentType.BaseType;
            }
        }

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RegionManager.RegionPropertyChanged -= RegionManager_RegionPropertyChanged;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
