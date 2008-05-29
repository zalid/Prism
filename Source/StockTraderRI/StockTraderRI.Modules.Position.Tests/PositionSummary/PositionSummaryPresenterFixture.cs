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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Prism.Interfaces;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.PresentationModels;
using StockTraderRI.Modules.Position.Tests.Mocks;


namespace StockTraderRI.Modules.Position.Tests.PositionSummary
{
    [TestClass]
    public class PositionSummaryPresenterFixture
    {
        MockPositionSummaryView view;
        MockAccountPositionService accountPositionService;
        MockMarketFeedService marketFeedService;
        MockMarketHistoryService marketHistoryService;
        MockTrendLinePresenter trendLinePresenter;
        MockOrdersController ordersController;
        MockEventAggregator eventAggregator;

        [TestInitialize]
        public void SetUp()
        {
            view = new MockPositionSummaryView();
            accountPositionService = new MockAccountPositionService();
            marketFeedService = new MockMarketFeedService();
            marketHistoryService = new MockMarketHistoryService();
            trendLinePresenter = new MockTrendLinePresenter();
            ordersController = new MockOrdersController();
            this.eventAggregator = new MockEventAggregator();
        }

        [TestMethod]
        public void CanInitPresenter()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());

            PositionSummaryPresenter presenter = CreatePresenter();

            Assert.AreEqual<IPositionSummaryView>(view, presenter.View);

        }

        [TestMethod]
        public void PresenterGeneratesPresentationModelFromPositionModelMarketAndNewsFeeds()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());

            accountPositionService.AddPosition(new AccountPosition("FUND0", 300m, 1000));
            accountPositionService.AddPosition(new AccountPosition("FUND1", 200m, 100));
            marketFeedService.SetPrice("FUND0", 30.00m);
            marketFeedService.SetPrice("FUND1", 20.00m);

            PositionSummaryPresenter presenter = CreatePresenter();

            Assert.AreEqual<decimal>(30.00m, presenter.PresentationModel.GetPrice("FUND0"));
            Assert.AreEqual<long>(1000, presenter.PresentationModel.GetQuantity("FUND0"));
            Assert.AreEqual<decimal>(20.00m, presenter.PresentationModel.GetPrice("FUND1"));
            Assert.AreEqual<long>(100, presenter.PresentationModel.GetQuantity("FUND1"));

        }

        [TestMethod]
        public void CanSetPresentationModelIntoView()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());

            PositionSummaryPresenter presenter = CreatePresenter();

            Assert.AreSame(presenter.PresentationModel, view.Model);
        }


        [TestMethod]
        public void PresenterUpdatesPresentationModelwithMarketUpdates()
        {
            var marketPricesUpdatedEvent = new MockMarketPricesUpdatedEvent();
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(marketPricesUpdatedEvent);

            marketFeedService.SetPrice("FUND0", 30.00m);
            accountPositionService.AddPosition("FUND0", 25.00m, 1000);
            marketFeedService.SetPrice("FUND1", 20.00m);
            accountPositionService.AddPosition("FUND1", 15.00m, 100);
            PositionSummaryPresenter presenter = CreatePresenter();

            var updatedPriceList = new Dictionary<string, decimal> { { "FUND0", 50.00m } };

            Assert.IsNotNull(marketPricesUpdatedEvent.SubscribeArgumentAction);
            Assert.AreEqual(ThreadOption.UIThread, marketPricesUpdatedEvent.SubscribeArgumentThreadOption);


            marketPricesUpdatedEvent.SubscribeArgumentAction(updatedPriceList);

            Assert.AreEqual<decimal>(50.00m, presenter.PresentationModel.GetPrice("FUND0"));
        }

        [TestMethod]
        public void MarketUpdatesPresenterPositionUpdatesButCollectionDoesNot()
        {
            var marketPricesUpdatedEvent = new MockMarketPricesUpdatedEvent();
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(marketPricesUpdatedEvent);

            marketFeedService.SetPrice("FUND1", 20.00m);
            accountPositionService.AddPosition("FUND1", 15.00m, 100);

            PositionSummaryPresenter presenter = CreatePresenter();

            bool presentationModelCollectionUpdated = false;
            presenter.PresentationModel.Data.CollectionChanged += delegate
              {
                  presentationModelCollectionUpdated = true;
              };

            bool presentationModelItemUpdated = false;
            presenter.PresentationModel.Data.First(p => p.TickerSymbol == "FUND1").PropertyChanged += delegate
               {
                   presentationModelItemUpdated = true;
               };

            marketPricesUpdatedEvent.SubscribeArgumentAction(new Dictionary<string, decimal> { { "FUND1", 50m } });

            Assert.IsFalse(presentationModelCollectionUpdated);
            Assert.IsTrue(presentationModelItemUpdated);
        }

        [TestMethod]
        public void PresenterPopulatesSummaryCollectionWithMarketHistory()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            marketFeedService.SetPrice("FUND1", 20.00m);
            accountPositionService.AddPosition("FUND1", 15.00m, 100);
            PositionSummaryPresenter presenter = CreatePresenter();

            Assert.AreEqual(2, presenter.PresentationModel.Data.First(p => p.TickerSymbol == "FUND1").PriceMarketHistory.Count);
        }

        [TestMethod]
        public void AccountPositionModificationUpdatesPM()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            marketFeedService.SetPrice("FUND0", 20.00m);
            accountPositionService.AddPosition("FUND0", 150.00m, 100);
            PositionSummaryPresenter presenter = CreatePresenter();

            bool presentationModelItemUpdated = false;
            presenter.PresentationModel.Data.First<PositionSummaryItem>(p => p.TickerSymbol == "FUND0").PropertyChanged += delegate
               {
                   presentationModelItemUpdated = true;
               };

            AccountPosition accountPosition = accountPositionService.GetAccountPositions().First<AccountPosition>(p => p.TickerSymbol == "FUND0");
            accountPosition.Shares += 11;
            accountPosition.CostBasis = 25.00m;

            Assert.IsTrue(presentationModelItemUpdated);
            Assert.AreEqual(111, presenter.PresentationModel.Data.First<PositionSummaryItem>(p => p.TickerSymbol == "FUND0").Shares);
            Assert.AreEqual(25.00m, presenter.PresentationModel.Data.First<PositionSummaryItem>(p => p.TickerSymbol == "FUND0").CostBasis);
        }

        [TestMethod]
        public void WhenPositionRowSelectedSymbolsTrendDataShowsInLineChart()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            eventAggregator.AddMapping<TickerSymbolSelectedEvent>(new MockTickerSymbolSelectedEvent());
            PositionSummaryPresenter presenter = CreatePresenter();

            view.SelectFUND0Row();

            Assert.IsTrue(trendLinePresenter.TickerSymbolSelected);
            Assert.AreEqual("FUND0", trendLinePresenter.SelectedTickerSymbol);
        }

        [TestMethod]
        public void TickerSymbolSelectedFiresEvent()
        {
            var tickerSymbolSelectedEvent = new MockTickerSymbolSelectedEvent();
            eventAggregator.AddMapping<TickerSymbolSelectedEvent>(tickerSymbolSelectedEvent);
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            PositionSummaryPresenter presenter = CreatePresenter();

            view.SelectFUND0Row();

            Assert.IsTrue(tickerSymbolSelectedEvent.FireCalled);
            Assert.AreEqual("FUND0", tickerSymbolSelectedEvent.FireArgumentPayload);
        }

        [TestMethod]
        public void ControllerCommandsSetIntoPresentationModel()
        {
            eventAggregator.AddMapping<MarketPricesUpdatedEvent>(new MockMarketPricesUpdatedEvent());
            PositionSummaryPresenter presenter = CreatePresenter();

            Assert.AreSame(presenter.PresentationModel.BuyCommand, ordersController.BuyCommand);
            Assert.AreSame(presenter.PresentationModel.SellCommand, ordersController.SellCommand);
        }

        private PositionSummaryPresenter CreatePresenter()
        {
            return new PositionSummaryPresenter(view, accountPositionService
                                                , marketFeedService
                                                , marketHistoryService
                                                , trendLinePresenter
                                                , ordersController
                                                , eventAggregator);
        }


    }

    internal class MockTickerSymbolSelectedEvent : TickerSymbolSelectedEvent
    {
        public bool FireCalled;
        public string FireArgumentPayload;


        public override void Fire(string payload)
        {
            FireCalled = true;
            FireArgumentPayload = payload;
        }
    }

    internal class MockMarketPricesUpdatedEvent : MarketPricesUpdatedEvent
    {
        public Action<IDictionary<string, decimal>> SubscribeArgumentAction;
        public Predicate<IDictionary<string, decimal>> SubscribeArgumentFilter;
        public ThreadOption SubscribeArgumentThreadOption;

        public override SubscriptionToken Subscribe(Action<IDictionary<string, decimal>> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<IDictionary<string, decimal>> filter)
        {
            SubscribeArgumentAction = action;
            SubscribeArgumentFilter = filter;
            SubscribeArgumentThreadOption = threadOption;
            return null;
        }
    }
}

/*
 * updates view when position added/removed
 * 
 * presenter does NOT update view when market feed does not relate to positions (filtering)
 * 
 * market update with volume change should be reflected in presentation model
 */
