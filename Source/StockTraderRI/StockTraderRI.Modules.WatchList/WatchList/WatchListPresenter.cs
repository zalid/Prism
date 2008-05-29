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
using System.Collections.ObjectModel;
using Prism.Interfaces;
using Prism.Utility;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Watch.PresentationModels;
using StockTraderRI.Modules.Watch.Properties;
using StockTraderRI.Modules.Watch.Services;

namespace StockTraderRI.Modules.Watch.WatchList
{
    public class WatchListPresenter : IWatchListPresenter
    {
        readonly WatchListPresentationModel _model = new WatchListPresentationModel();

        public WatchListPresenter(IWatchListView view, IWatchListService watchListService, IMarketFeedService marketFeedService, IEventAggregator eventAggregator)
        {
            View = view;
            _model.HeaderInfo = Resources.WatchListTitle;
            _model.WatchListItems = new ObservableCollection<WatchItem>();
            View.Model = _model;

            this.marketFeedService = marketFeedService;

            this.watchList = watchListService.RetrieveWatchList();
            watchList.CollectionChanged += delegate { PopulateWatchItemsList(watchList); };
            PopulateWatchItemsList(watchList);

            eventAggregator.Get<MarketPricesUpdatedEvent>().Subscribe(MarketPricesUpdated, ThreadOption.UIThread);
            View.OnRemoveMenuItemClicked += View_OnRemoveMenuItemClicked;
        }

        private void MarketPricesUpdated(IDictionary<string, decimal> updatedPriceList)
        {
            foreach (WatchItem watchItem in _model.WatchListItems)
            {
                if (updatedPriceList.ContainsKey(watchItem.TickerSymbol))
                    watchItem.CurrentPrice = updatedPriceList[watchItem.TickerSymbol];
            }
        }

        private void View_OnRemoveMenuItemClicked(object sender, DataEventArgs<string> e)
        {
            watchList.Remove(e.Value);
        }

        private void PopulateWatchItemsList(ObservableCollection<string> watchItemsList)
        {
            _model.WatchListItems.Clear();
            foreach (string tickerSymbol in watchItemsList)
            {
                decimal? currentPrice;
                try
                {
                    currentPrice = marketFeedService.GetPrice(tickerSymbol);
                }
                catch (ArgumentException)
                {
                    currentPrice = null;
                }
                _model.WatchListItems.Add(new WatchItem(tickerSymbol, currentPrice));
            }
        }

        public IWatchListView View { get; set; }
        private readonly IMarketFeedService marketFeedService;
        private readonly ObservableCollection<string> watchList;
    }
}
