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
using Microsoft.Practices.Composite.Events;
using StockTraderRI.Modules.Watch.WatchList;

namespace StockTraderRI.Modules.WatchList.Tests.Mocks
{
    class MockWatchListView : IWatchListView
    {
        public event EventHandler<DataEventArgs<string>> OnRemoveMenuItemClicked;

        public void ClickRemoveMenuItem(string symbol)
        {
            OnRemoveMenuItemClicked(this, new DataEventArgs<string>(symbol));
        }

        public WatchListPresentationModel Model { get; set; }

    }
}