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
using Castle.Core;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.PresentationModels;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;

namespace StockTraderRI.Modules.Market.TrendLine
{
    public class TrendLinePresenter : ITrendLinePresenter
    {
        IMarketHistoryService _marketHistoryService;

        public TrendLinePresenter(ITrendLineView view, IMarketHistoryService marketHistoryService)
        {
            this.View = view;
            this._marketHistoryService = marketHistoryService;

        }

        #region ITrendLinePresenter Members

        [DoNotWire]
        public ITrendLineView View { get; set; }

        public void OnTickerSymbolSelected(string tickerSymbol)
        {
            MarketHistoryCollection historyCollection = _marketHistoryService.GetPriceHistory(tickerSymbol);

            View.UpdateLineChart(historyCollection);
            View.SetChartTitle(tickerSymbol);
        }

        #endregion
    }
}
