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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Properties;

namespace Microsoft.Practices.Composite.Wpf.Regions
{
    public class ItemsControlRegionAdapter : RegionAdapterBase<ItemsControl>
    {
        protected override void Adapt(IRegion region, ItemsControl regionTarget)
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

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}