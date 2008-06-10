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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Prism.Interfaces;
using Prism.Properties;
using System.Windows.Data;

namespace Prism.Regions
{
    public class ItemsControlRegionAdapter : IRegionAdapter
    {
        public IRegion Initialize(DependencyObject controlToWrap)
        {
            ItemsControl control = controlToWrap as ItemsControl;
            IRegion region = null;

            if (control != null)
            {
                if (control.ItemsSource != null || (BindingOperations.GetBinding(control, ItemsControl.ItemsSourceProperty) != null))
                    throw new InvalidOperationException(Resources.ItemsControlHasItemsSourceException);

                Selector selector = control as Selector;
                if (selector != null)
                    selector.IsSynchronizedWithCurrentItem = true;

                region = this.CreateRegion();
                //Control must be empty before setting ItemsSource
                foreach (object childItem in control.Items)
                {
                    region.Add(childItem);
                }
                control.Items.Clear();
                control.ItemsSource = region.Views;
            }
            return region;
        }

        public virtual IRegion CreateRegion()
        {
            return new SimpleRegion();
        }
    }
}