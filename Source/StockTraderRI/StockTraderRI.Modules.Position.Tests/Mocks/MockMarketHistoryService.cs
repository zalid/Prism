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

namespace StockTraderRI.Modules.Position.Tests.Mocks
{
    public class MockMarketHistoryService : IMarketHistoryService
    {
        #region IMarketHistoryService Members

        public MarketHistoryCollection GetPriceHistory(string tickerSymbol)
        {
            return new MarketHistoryCollection { new MarketHistoryItem(new DateTime(1), 1.00m), new MarketHistoryItem(new DateTime(2), 2.00m) };
        }

        #endregion
    }
}
