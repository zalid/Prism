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
using System.Collections;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Watch;
using Prism.Commands;

namespace StockTraderRI.Modules.WatchList.Services
{
    public class WatchListService : IWatchListService
    {
        private IMarketFeedService marketFeedService;

        private ObservableCollection<string> WatchItems { get; set; }

        public WatchListService(IMarketFeedService marketFeedService)
        {
            this.marketFeedService = marketFeedService;
            WatchItems = new ObservableCollection<string>();
            AddWatchCommand = new DelegateCommand<string>(OnFiredCallback);
        }

        public ObservableCollection<string> RetrieveWatchList()
        {
            return WatchItems;
        }

        private void OnFiredCallback(object paramater)
        {
            string tickerSymbol = paramater as string;
            if (!String.IsNullOrEmpty(tickerSymbol))
            {
                string upperCasedTrimmedSymbol = tickerSymbol.ToUpperInvariant().Trim();
                if (!WatchItems.Contains(upperCasedTrimmedSymbol))
                {
                    if (marketFeedService.SymbolExists(upperCasedTrimmedSymbol))
                    {
                        WatchItems.Add(upperCasedTrimmedSymbol);
                    }
                }
            }
        }

        public DelegateCommand<string> AddWatchCommand { get; set; }
    }
}
