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
using StockTraderRI.Infrastructure.PresentationModels;
using System.Windows.Controls;
using StockTraderRI.Modules.Position.PresentationModels;
using Prism.Utility;
using StockTraderRI.Modules.Position.Interfaces;

namespace StockTraderRI.Modules.Position.Tests.Mocks
{
    class MockPositionSummaryView : Control, IPositionSummaryView
    {
        public PositionSummaryPresentationModel Model { get; set; }

        public event EventHandler<DataEventArgs<string>> TickerSymbolSelected = delegate { };


        internal void SelectFUND0Row()
        {
            TickerSymbolSelected(this, new DataEventArgs<string>("FUND0"));
        }

        #region IPositionSummaryView Members


        public void ShowTrendLine(ITrendLineView view)
        {
            
        }

        #endregion

    }
}