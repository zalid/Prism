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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.WatchList.Tests.Mocks;
using Microsoft.Practices.Unity;
using Prism.Regions;
using System.Windows.Controls;
using Prism.Interfaces;
using StockTraderRI.Modules.Watch.WatchList;
using StockTraderRI.Modules.Watch;
using System.Threading;

namespace StockTraderRI.Modules.WatchList.Tests.WatchList
{
    [TestClass]
    public class WatchListPresenterFixture
    {
        MockWatchListView view;
        MockWatchListService watchListService;
        MockMarketFeedService marketFeedService;

        [TestInitialize]
        public void SetUp()
        {
            view = new MockWatchListView();
            watchListService = new MockWatchListService();
            marketFeedService = new MockMarketFeedService();
        }

        [TestMethod]
        public void CanInitPresenter()
        {
            WatchListPresenter presenter = CreatePresenter();

            Assert.AreEqual<IWatchListView>(view, presenter.View);
        }

        [TestMethod]
        public void ClickingOnTheRemoveMenuItemCallsTheRemoveSelectedItemMethod()
        {
            watchListService.MockWatchList.Add("TEST");
            WatchListPresenter presenter = CreatePresenter();

            Assert.AreEqual(1, watchListService.MockWatchList.Count);

            view.ClickRemoveMenuItem("TEST");

            Assert.AreEqual(0, watchListService.MockWatchList.Count);
        }

        [TestMethod]
        public void CanGetItemsFromWatchListServiceAndPutInView()
        {
            watchListService.MockWatchList.Add("TESTFUND0");
            watchListService.MockWatchList.Add("TESTFUND1");

            WatchListPresenter presenter = CreatePresenter();

            Assert.IsNotNull(view.Model);
            Assert.IsNotNull(view.Model.WatchListItems);
            Assert.AreEqual(2, view.Model.WatchListItems.Count);
            Assert.AreEqual<string>("TESTFUND0", view.Model.WatchListItems[0].TickerSymbol);
            Assert.AreEqual<string>("TESTFUND1", view.Model.WatchListItems[1].TickerSymbol);
        }

        [TestMethod]
        public void PresenterObservesWatchListAndUpdatesView()
        {
            watchListService.MockWatchList.Add("TESTFUND0");

            WatchListPresenter presenter = CreatePresenter();

            Assert.AreEqual(1, view.Model.WatchListItems.Count());

            watchListService.MockWatchList.Add("TESTFUND1");

            Assert.AreEqual(2, view.Model.WatchListItems.Count());
            Assert.AreEqual<string>("TESTFUND1", view.Model.WatchListItems[1].TickerSymbol);
        }


        [TestMethod]
        public void ViewGetsCurrentPriceForSymbolRetrievedFromMarketFeedService()
        {
            watchListService.MockWatchList.Add("TESTFUND0");
            marketFeedService.SetPrice("TESTFUND0", 15.5m);

            WatchListPresenter presenter = CreatePresenter();

            WatchItem watchItem = view.Model.WatchListItems[0];
            Assert.AreEqual<string>("TESTFUND0", watchItem.TickerSymbol);
            Assert.IsNotNull(watchItem.CurrentPrice);
            Assert.AreEqual<decimal>(15.5m, watchItem.CurrentPrice.Value);
        }

        [TestMethod]
        public void IfPriceIsNotAvailableForSymbolSetsNullCurrentPriceInView()
        {
            watchListService.MockWatchList.Add("NONEXISTING");
            marketFeedService.MockSymbolExists = false;

            WatchListPresenter presenter = CreatePresenter();

            WatchItem watchItem = view.Model.WatchListItems[0];
            Assert.AreEqual<string>("NONEXISTING", watchItem.TickerSymbol);
            Assert.IsNull(watchItem.CurrentPrice);
        }

        [TestMethod]
        public void PresenterObservesMarketFeedAndUpdatesView()
        {
            marketFeedService.feedData.Add("TESTFUND0", 15.5m);
            watchListService.MockWatchList.Add("TESTFUND0");

            WatchListPresenter presenter = CreatePresenter();

            Assert.AreEqual<decimal>(15.5m, view.Model.WatchListItems[0].CurrentPrice.Value);

            marketFeedService.feedData["TESTFUND0"] = 25.3m;
            marketFeedService.RaiseUpdated();

            Assert.AreEqual<decimal>(25.3m, view.Model.WatchListItems[0].CurrentPrice.Value);
        }

        [TestMethod]
        public void MarketFeedUpdatedDoesNotUpdateViewIfWatchListCountIsZero()
        {
            WatchListPresenter presenter = CreatePresenter();
            view.Model.WatchListItems = null;

            marketFeedService.RaiseUpdated();

            Assert.IsNull(view.Model.WatchListItems);
        }

        [TestMethod]
        public void PresentationModelShouldHaveHeaderInfoSet()
        {
            WatchListPresenter presenter = CreatePresenter();
            Assert.AreEqual("Watch List",view.Model.HeaderInfo);            
        }


        private WatchListPresenter CreatePresenter()
        {
            return new WatchListPresenter(view, watchListService, marketFeedService);
        }


    }
}