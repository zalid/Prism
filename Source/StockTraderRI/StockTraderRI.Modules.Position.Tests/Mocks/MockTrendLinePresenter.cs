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
using System.Windows.Controls;

namespace StockTraderRI.Modules.Position.Tests.Mocks
{
    public class MockTrendLinePresenter : ITrendLinePresenter
    {
        public MockTrendLinePresenter()
        {
            TickerSymbolSelected = false;
            SelectedTickerSymbol = string.Empty;
        }
        ITrendLineView view = new MockTrendLineView();
        #region ITrendLinePresenter Members

        public ITrendLineView View
        {
            get { return view; }
        }

        #endregion

        #region ITrendLinePresenter Members


        public void OnTickerSymbolSelected(string tickerSymbol)
        {
            TickerSymbolSelected = true;
            SelectedTickerSymbol = tickerSymbol;

        }

        #endregion

        public bool TickerSymbolSelected { get; set; }
        public string SelectedTickerSymbol { get; set; }
    }

    public class MockTrendLineView : ITrendLineView
    {
        #region ITrendLineView Members

        public void UpdateLineChart(StockTraderRI.Infrastructure.PresentationModels.MarketHistoryCollection historyCollection)
        {
            throw new NotImplementedException();
        }

        public void SetChartTitle(string title)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
