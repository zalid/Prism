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

namespace StockTraderRI.Modules.Position.Tests.Mocks
{
    public class MockMarketFeedService : IMarketFeedService
    {

        Dictionary<string, decimal> feedData = new Dictionary<string, decimal>();

        internal void SetPrice(string tickerSymbol, decimal price)
        {
            feedData.Add(tickerSymbol, price);
        }


        internal void UpdatePrice(string tickerSymbol, decimal newPrice, long volume)
        {
            feedData[tickerSymbol] = newPrice;
            Updated(this, EventArgs.Empty);
        }

        #region IMarketFeedService Members

        public decimal GetPrice(string tickerSymbol)
        {
            if (feedData.ContainsKey(tickerSymbol))
                return feedData[tickerSymbol];
            else
                return 0m;
        }

        public long GetVolume(string tickerSymbol)
        {
            throw new NotImplementedException();
        }

        public event EventHandler Updated = delegate { };

        #endregion

        public bool SymbolExists(string tickerSymbol)
        {
            throw new NotImplementedException();
        }
    }
}
