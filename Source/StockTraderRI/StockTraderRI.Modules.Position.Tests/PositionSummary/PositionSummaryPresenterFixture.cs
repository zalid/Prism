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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Modules.Position.Tests.Mocks;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.PresentationModels;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.Interfaces;


namespace StockTraderRI.Modules.Position.Tests.PositionSummary
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PositionSummaryPresenterFixture
    {
        MockPositionSummaryView view;
        MockAccountPositionService accountPositionService;
        MockNewsFeedService newsFeedService;
        MockMarketFeedService marketFeedService;
        MockMarketHistoryService marketHistoryService;
        MockTrendLinePresenter trendLinePresenter;
        MockOrdersController ordersController;


        [TestInitialize]
        public void SetUp()
        {
            view = new MockPositionSummaryView();
            accountPositionService = new MockAccountPositionService();
            newsFeedService = new MockNewsFeedService();
            marketFeedService = new MockMarketFeedService();
            marketHistoryService = new MockMarketHistoryService();
            trendLinePresenter = new MockTrendLinePresenter();
            ordersController = new MockOrdersController();
        }

        [TestMethod]
        public void CanInitPresenter()
        {
            PositionSummaryPresenter presenter = CreatePresenter();

            Assert.AreEqual<IPositionSummaryView>(view, presenter.View);
           
        }

        [TestMethod]
        public void PresenterGeneratesPresentationModelFromPositionModelMarketAndNewsFeeds()
        {
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
            PositionSummaryPresenter presenter = CreatePresenter();

            Assert.AreSame(presenter.PresentationModel, view.Model);
        }


        [TestMethod]
        public void PresenterUpdatesPresentationModelwithMarketUpdates()
        {
            marketFeedService.SetPrice("FUND0", 30.00m);
            accountPositionService.AddPosition("FUND0", 25.00m, 1000);
            marketFeedService.SetPrice("FUND1", 20.00m);
            accountPositionService.AddPosition("FUND1", 15.00m, 100);
            PositionSummaryPresenter presenter = CreatePresenter();
           
            //Update price in mock. Updates to real market feed may come in different form
            marketFeedService.UpdatePrice("FUND0", 50.00m, 12345678L);

            Assert.AreEqual<decimal>(50.00m, presenter.PresentationModel.GetPrice("FUND0"));
        }

        [TestMethod]
        public void PresenterUpdatesPresentationModelWithNewsUpdate()
        {
            marketFeedService.SetPrice("FUND0", 30.00m);
            accountPositionService.AddPosition("FUND0", 25.00m, 1000);
            marketFeedService.SetPrice("FUND2", 30.00m);
            accountPositionService.AddPosition("FUND2", 25.00m, 1000);

            PositionSummaryPresenter presenter = CreatePresenter();

            newsFeedService.UpdateNews("FUND0", "Widget 2008 Ships");
            newsFeedService.UpdateNews("FUND2", "Company announces 4th quarter profits");

            Assert.IsTrue(presenter.PresentationModel.HasNews("FUND0"));
            Assert.IsFalse(presenter.PresentationModel.HasNews("FUND1"));
        }

        [TestMethod]
        public void MarketUpdatesPresenterPositionUpdatesButCollectionDoesNot()
        {
            marketFeedService.SetPrice("FUND1", 20.00m);
            accountPositionService.AddPosition("FUND1", 15.00m, 100);

            PositionSummaryPresenter presenter = CreatePresenter();

            bool presentationModelCollectionUpdated = false;
            presenter.PresentationModel.Data.CollectionChanged += delegate
              {
                  presentationModelCollectionUpdated = true;
              };

            bool presentationModelItemUpdated = false;
            presenter.PresentationModel.Data.First<PositionSummaryItem>(p => p.TickerSymbol == "FUND1").PropertyChanged += delegate
               {
                   presentationModelItemUpdated = true;
               };

            //Update price in mock. Updates to real market feed may come in different form
            marketFeedService.UpdatePrice("FUND1", 50.00m, 12345678L);

            Assert.IsFalse(presentationModelCollectionUpdated);
            Assert.IsTrue(presentationModelItemUpdated);
        }

        [TestMethod]
        public void PresenterPopulatesSummaryCollectionWithMarketHistory()
        {
            marketFeedService.SetPrice("FUND1", 20.00m);
            accountPositionService.AddPosition("FUND1", 15.00m, 100);
            PositionSummaryPresenter presenter = CreatePresenter();

            Assert.AreEqual(2, presenter.PresentationModel.Data.First(p => p.TickerSymbol == "FUND1").PriceMarketHistory.Count);
        }

        [TestMethod]
        public void NewsUpdatesPresenterPositionSummaryItemButCollectionDoesNot()
        {
            marketFeedService.SetPrice("FUND0", 20.00m);
            accountPositionService.AddPosition("FUND0", 15.00m, 100);

            PositionSummaryPresenter presenter = CreatePresenter();

            bool presentationModelCollectionUpdated = false;
            presenter.PresentationModel.Data.CollectionChanged += delegate
              {
                  presentationModelCollectionUpdated = true;
              };

            bool presentationModelItemUpdated = false;
            presenter.PresentationModel.Data.First<PositionSummaryItem>(p => p.TickerSymbol == "FUND0").PropertyChanged += delegate
               {
                   presentationModelItemUpdated = true;
               };

            //Update price in mock. Updates to real market feed may come in different form
            newsFeedService.UpdateNews("FUND0", "Prism Ships");

            Assert.IsFalse(presentationModelCollectionUpdated);
            Assert.IsTrue(presentationModelItemUpdated);
        }

        [TestMethod]
        public void AccountPositionModificationUpdatesPM()
        {
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
            PositionSummaryPresenter presenter = CreatePresenter();
            
            view.SelectFUND0Row();

            Assert.IsTrue(trendLinePresenter.TickerSymbolSelected);
            Assert.AreEqual("FUND0", trendLinePresenter.SelectedTickerSymbol);
        }

        [TestMethod]
        public void ControllerCommandsSetIntoPresentationModel()
        {
            PositionSummaryPresenter presenter = CreatePresenter();

            Assert.AreSame(presenter.PresentationModel.BuyCommand, ordersController.BuyCommand);
            Assert.AreSame(presenter.PresentationModel.SellCommand, ordersController.SellCommand);


        }
	

        

        private PositionSummaryPresenter CreatePresenter()
        {
            return new PositionSummaryPresenter(view, accountPositionService
                                                , marketFeedService, newsFeedService
                                                , marketHistoryService, trendLinePresenter, ordersController);
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
