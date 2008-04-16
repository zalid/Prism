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
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Infrastructure.PresentationModels;
using Prism.Services;
using StockTraderRI.Infrastructure;
using Prism.Interfaces;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using StockTraderRI.Modules.Position.PresentationModels;
using Prism.Utility;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Controllers;

namespace StockTraderRI.Modules.Position.PositionSummary
{
    public class PositionSummaryPresenter : IPositionSummaryPresenter
    {
        public event EventHandler<DataEventArgs<string>> TickerSymbolSelected = delegate { };

        public PositionSummaryPresenter(IPositionSummaryView view, IAccountPositionService accountPositionService
                                        , IMarketFeedService marketFeedSvc, INewsFeedService newsFeedSvc
                                        , IMarketHistoryService marketHistorySvc
                                        , ITrendLinePresenter trendLinePresenter
                                        , IOrdersController ordersController)
        {
            View = view;
            AccountPositionSvc = accountPositionService;
            NewsFeedSvc = newsFeedSvc;
            MarketFeedSvc = marketFeedSvc;
            MarketHistorySvc = marketHistorySvc;

            PresentationModel = new PositionSummaryPresentationModel();

            PopulatePresentationModel(AccountPositionSvc, MarketFeedSvc, newsFeedSvc, PresentationModel, MarketHistorySvc);
            PresentationModel.BuyCommand = ordersController.BuyCommand;
            PresentationModel.SellCommand = ordersController.SellCommand;

            View.Model = PresentationModel;
            _trendLinePresenter = trendLinePresenter;
            View.ShowTrendLine(trendLinePresenter.View);

            //Initially show the FAKEINDEX
            trendLinePresenter.OnTickerSymbolSelected("FAKEINDEX");
            
            InitializeEvents();

        }

        private void InitializeEvents()
        {
            MarketFeedSvc.Updated += new EventHandler(marketFeed_Updated);
            NewsFeedSvc.Updated += new EventHandler<NewsFeedEventArgs>(newsFeed_Updated);
            AccountPositionSvc.Updated += new EventHandler<AccountPositionModelEventArgs>(model_Updated);
            View.TickerSymbolSelected += new EventHandler<DataEventArgs<string>>(View_TickerSymbolSelected);
        }

        void View_TickerSymbolSelected(object sender, DataEventArgs<string> e)
        {
            _trendLinePresenter.OnTickerSymbolSelected(e.Value);
        }

        void model_Updated(object sender, AccountPositionModelEventArgs e)
        {
            if (e.AcctPosition != null)
            {
                PositionSummaryItem posit = PresentationModel.Data.First(p => p.TickerSymbol == e.AcctPosition.TickerSymbol);

                if (posit != null)
                {
                    posit.Shares = e.AcctPosition.Shares;
                    posit.CostBasis = e.AcctPosition.CostBasis;
                }
            }
        }

        void newsFeed_Updated(object sender, NewsFeedEventArgs e)
        {
            PositionSummaryItem posit = PresentationModel.Data.First(p => p.TickerSymbol == e.TickerSymbol);
            if (posit != null)
            {
                posit.HasNews = true;
            }
        }

        void marketFeed_Updated(object sender, EventArgs e)
        {
            foreach (var position in PresentationModel.Data)
            {
                position.CurrentPrice = MarketFeedSvc.GetPrice(position.TickerSymbol);
            }
        }

        private static void PopulatePresentationModel(IAccountPositionService accountPositionService, IMarketFeedService marketFeed, INewsFeedService newsFeed, PositionSummaryPresentationModel presentationModel, IMarketHistoryService marketHistoryService)
        {
            foreach (AccountPosition pos in accountPositionService.GetAccountPositions())
            {
                presentationModel.AddPosition(pos.TickerSymbol, pos.CostBasis, pos.Shares, marketFeed.GetPrice(pos.TickerSymbol), newsFeed.HasNews(pos.TickerSymbol), marketHistoryService.GetPriceHistory(pos.TickerSymbol));
            }
        }

        private IMarketFeedService MarketFeedSvc { get; set; }
        private INewsFeedService NewsFeedSvc { get; set; }
        private IAccountPositionService AccountPositionSvc { get; set; }
        public PositionSummaryPresentationModel PresentationModel { get; private set; }
        private IMarketHistoryService MarketHistorySvc { get; set; }
        public IPositionSummaryView View { get; set; }
        private ITrendLinePresenter _trendLinePresenter;
    }
}
