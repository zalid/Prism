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
using Prism.Utility;

namespace StockTraderRI.Modules.Watch.WatchList
{
    /// <summary>
    /// Interaction logic for WatchListView.xaml
    /// </summary>
    public partial class WatchListView : UserControl, IWatchListView
    {
        public event EventHandler<DataEventArgs<string>> OnRemoveMenuItemClicked = delegate { };

        public WatchListView()
        {
            InitializeComponent();
        }

        private void RemoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WatchItem selectedItem = watchListListView.SelectedValue as WatchItem;
            if (selectedItem != null)
                OnRemoveMenuItemClicked(this, new DataEventArgs<string>(selectedItem.TickerSymbol));
        }

        #region IWatchListView Members


        public WatchListPresentationModel Model
        {
            set { DataContext = value; }
        }

        #endregion
    }
}
