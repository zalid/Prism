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

namespace Microsoft.Practices.Composite.Wpf.Regions
{
    public class ItemMetadata : DependencyObject
    {
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(ItemMetadata));


        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ItemMetadata), new PropertyMetadata(DependencyPropertyChanged));

        public ItemMetadata(object item)
        {
            //check for null
            this.Item = item;
        }

        public object Item { get; set; }

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public event EventHandler MetadataChanged;

        public void InvokeMetadataChanged()
        {
            EventHandler metadataChangedHandler = MetadataChanged;
            if (metadataChangedHandler != null) metadataChangedHandler(this, EventArgs.Empty);
        }

        private static void DependencyPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ItemMetadata itemMetadata = dependencyObject as ItemMetadata;
            if (itemMetadata != null)
            {
                itemMetadata.InvokeMetadataChanged();
            }
        }
    }
}