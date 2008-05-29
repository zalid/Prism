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
using Prism.Interfaces;
using Prism.Utility;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.PresentationModels;

namespace StockTraderRI.Modules.Position.PositionSummary
{
    public class PositionSummaryPresenter : IPositionSummaryPresenter
    {
        public event EventHandler<DataEventArgs<string>> TickerSymbolSelected = delegate { };

        public PositionSummaryPresenter(IPositionSummaryView view, IAccountPositionService accountPositionService
                                        , IMarketFeedService marketFeedSvc
                                        , IMarketHistoryService marketHistorySvc
                                        , ITrendLinePresenter trendLinePresenter
                                        , IOrdersController ordersController
                                        , IEventAggregator eventAggregator)
        {
            View = view;
            AccountPositionSvc = accountPositionService;
            MarketHistorySvc = marketHistorySvc;
            EventAggregator = eventAggregator;

            PresentationModel = new PositionSummaryPresentationModel();

            PopulatePresentationModel(AccountPositionSvc, marketFeedSvc, PresentationModel, MarketHistorySvc);
            PresentationModel.BuyCommand = ordersController.BuyCommand;
            PresentationModel.SellCommand = ordersController.SellCommand;

            View.Model = PresentationModel;
            _trendLinePresenter = trendLinePresenter;
            View.ShowTrendLine(trendLinePresenter.View);

            //Initially show the FAKEINDEX
            trendLinePresenter.OnTickerSymbolSelected("FAKEINDEX");

            eventAggregator.Get<MarketPricesUpdatedEvent>().Subscribe(MarketPricesUpdated, ThreadOption.UIThread);

            InitializeEvents();

        }

        private void InitializeEvents()
        {
            AccountPositionSvc.Updated += model_Updated;
            View.TickerSymbolSelected += View_TickerSymbolSelected;
        }

        void View_TickerSymbolSelected(object sender, DataEventArgs<string> e)
        {
            _trendLinePresenter.OnTickerSymbolSelected(e.Value);

            EventAggregator.Get<TickerSymbolSelectedEvent>().Fire(e.Value);
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

        private void MarketPricesUpdated(IDictionary<string, decimal> priceList)
        {
            foreach (PositionSummaryItem position in PresentationModel.Data)
            {
                if (priceList.ContainsKey(position.TickerSymbol))
                {
                    position.CurrentPrice = priceList[position.TickerSymbol];
                }
            }
        }

        private static void PopulatePresentationModel(IAccountPositionService accountPositionService, IMarketFeedService marketFeed, PositionSummaryPresentationModel presentationModel, IMarketHistoryService marketHistoryService)
        {
            foreach (AccountPosition pos in accountPositionService.GetAccountPositions())
            {
                presentationModel.AddPosition(pos.TickerSymbol, pos.CostBasis, pos.Shares, marketFeed.GetPrice(pos.TickerSymbol), marketHistoryService.GetPriceHistory(pos.TickerSymbol));
            }
        }

        private IAccountPositionService AccountPositionSvc { get; set; }
        public PositionSummaryPresentationModel PresentationModel { get; private set; }
        private IMarketHistoryService MarketHistorySvc { get; set; }
        private IEventAggregator EventAggregator { get; set; }
        public IPositionSummaryView View { get; set; }
        private readonly ITrendLinePresenter _trendLinePresenter;
    }
}
