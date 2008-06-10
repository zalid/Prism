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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Prism.Interfaces;

namespace Prism.Regions
{
    public class ContentControlRegionAdapter : IRegionAdapter
    {
        public IRegion Initialize(DependencyObject obj)
        {
            ContentControl control = obj as ContentControl;
            IRegion region = null;

            if (control != null)
            {
                region = CreateRegion();
                Binding binding = new Binding("CurrentItem");
                binding.Source = region.Views;
                BindingOperations.SetBinding(control, ContentControl.ContentProperty, binding);
            }
            return region;
        }

        public virtual IRegion CreateRegion()
        {
            return new SimpleRegion();
        }
    }
}
