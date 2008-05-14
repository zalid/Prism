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

using System.Collections.Generic;
using System.Globalization;
using Prism.Interfaces;

namespace Prism.Services
{
    public class RegionManagerService : IRegionManagerService
    {
        private readonly Dictionary<string, IRegion> _regions = new Dictionary<string, IRegion>();

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
    }
}
