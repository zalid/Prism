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
using System.Windows.Threading;
using StockTraderRI.Infrastructure.Interfaces;
using Microsoft.Practices.Unity;
using Prism;
using System.Windows;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Infrastructure.PresentationModels;
using System.Collections.ObjectModel;
using Prism.Interfaces;
using StockTraderRI.Modules.WatchList.Services;
using Prism.Utility;
using StockTraderRI.Modules.Watch.PresentationModels;
using StockTraderRI.Modules.Watch.Properties;

namespace StockTraderRI.Modules.Watch.WatchList
{
    public class WatchListPresenter : IWatchListPresenter
    {
        WatchListPresentationModel _model = new WatchListPresentationModel();

        public WatchListPresenter(IWatchListView view, IWatchListService watchListService, IMarketFeedService marketFeedService)
        {
            View = view;
            _model.HeaderInfo = Resources.WatchListTitle;
            View.Model = _model;

            this.marketFeedService = marketFeedService;
            this.marketFeedService.Updated += marketFeedService_Updated;
            this.watchList = watchListService.RetrieveWatchList();
            watchList.CollectionChanged += delegate { PopulateWatchItemsList(watchList); };
            PopulateWatchItemsList(watchList);
            
            InitializeEvents();
        }

        void marketFeedService_Updated(object sender, EventArgs e)
        {
            if (watchList.Count > 0)
            {
                PopulateWatchItemsList(watchList);
            }
        }


        private void InitializeEvents()
        {
            View.OnRemoveMenuItemClicked += View_OnRemoveMenuItemClicked;
        }

        void View_OnRemoveMenuItemClicked(object sender, DataEventArgs<string> e)
        {
            watchList.Remove(e.Value);
        }

        private void PopulateWatchItemsList(ObservableCollection<string> watchItemsList)
        {
            Dispatcher dispatcher = ((UIElement) View).Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                       new Action<ObservableCollection<string>>(PopulateWatchItemsList),
                                       watchList);
                return;
            }

            ObservableCollection<WatchItem> watchItems = new ObservableCollection<WatchItem>();
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
                watchItems.Add(new WatchItem(tickerSymbol, currentPrice));
            }
            _model.WatchListItems = watchItems;
        }

        public IWatchListView View { get; set; }
        private IMarketFeedService marketFeedService;
        private ObservableCollection<string> watchList;
    }
}
