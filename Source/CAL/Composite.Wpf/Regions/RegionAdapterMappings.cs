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
using System.Globalization;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Properties;

namespace Microsoft.Practices.Composite.Wpf.Regions
{
    public class RegionAdapterMappings
    {
        Dictionary<Type, IRegionAdapter> mappings = new Dictionary<Type, IRegionAdapter>();

        public void RegisterMapping(Type controlType, IRegionAdapter adapter)
        {
            if (controlType == null)
            {
                throw new ArgumentNullException("controlType");
            }

            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            if (mappings.ContainsKey(controlType))
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                                                                  Resources.MappingExistsException, controlType.Name));
            }

            mappings.Add(controlType, adapter);
        }

        public IRegionAdapter GetMapping(Type controlType)
        {
            Type currentType = controlType;

            while (currentType != null)
            {
                if (mappings.ContainsKey(currentType))
                {
                    return mappings[currentType];
                }
                currentType = currentType.BaseType;
            }
            throw new KeyNotFoundException("controlType");
        }
    }
}