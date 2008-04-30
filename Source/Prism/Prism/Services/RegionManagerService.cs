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
using Prism.Interfaces;
using System.Windows;
using System.Globalization;
using Prism.Properties;

namespace Prism.Services
{
    public class RegionManagerService : IRegionManagerService
    {
        private readonly Dictionary<string, IRegion> _regions = new Dictionary<string, IRegion>();

        public void Register(string regionName, IRegion region)
        {
            //Check to ensure a region with the same name is not already registered
            if (HasRegion(regionName))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.RegionNameExistsException, regionName));
            }

            _regions.Add(regionName, region);
        }

        public IRegion GetRegion(string regionName)
        {
            if (HasRegion(regionName))
                return _regions[regionName];

            throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Resources.ValueNotFound, regionName));
        }

        public bool HasRegion(string regionName)
        {
            return _regions.ContainsKey(regionName);
        }

        public void SetRegion(DependencyObject containerElement, string regionName)
        {
            IPrismContainer container = PrismContainerProvider.Provider;
            if (container == null)
                throw new InvalidOperationException(Resources.PrismContainerProviderNotInitialized);

            Type currentType = containerElement.GetType();

            while (typeof(DependencyObject).IsAssignableFrom(currentType))
            {
                IRegionAdapter region = (IRegionAdapter)container.TryResolve(typeof(IRegion<>).MakeGenericType(currentType));

                if (region != null)
                {
                    region.Initialize(containerElement);
                    Register(regionName, (IRegion) region);
                    return;
                }

                currentType = currentType.BaseType;
            }
        }
    }
}
