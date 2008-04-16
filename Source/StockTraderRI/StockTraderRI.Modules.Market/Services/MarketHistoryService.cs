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
using System.Xml.Linq;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.PresentationModels;
using System.Xml.Serialization;
using System.Xml;
using System.Globalization;

namespace StockTraderRI.Modules.Market.Services
{
    public class MarketHistoryService : IMarketHistoryService
    {
        readonly Dictionary<string, MarketHistoryCollection> _marketHistory = new Dictionary<string,MarketHistoryCollection>();

        #region IMarketHistoryService Members

        public MarketHistoryService()
        {
            InitializeMarketHistory();
        }

        private void InitializeMarketHistory()
        {
            var document = XDocument.Load("Data/MarketHistory.xml");
            foreach (var marketHistoryItemElement in document.Descendants("MarketHistoryItem"))
            {
                var item = new MarketHistoryItem
                               {
                                   DateTimeMarker =
                                       DateTime.Parse(marketHistoryItemElement.Attribute("Date").Value,
                                                      CultureInfo.InvariantCulture),
                                   Value =
                                       Decimal.Parse(marketHistoryItemElement.Value, NumberStyles.Float,
                                                     CultureInfo.InvariantCulture)
                               };
                string tickerSymbol = marketHistoryItemElement.Attribute("TickerSymbol").Value;

                if (!_marketHistory.ContainsKey(tickerSymbol))
                {
                    _marketHistory.Add(tickerSymbol, new MarketHistoryCollection());
                    
                }

                var items = _marketHistory[tickerSymbol];

                items.Add(item);
            }
        }

        public MarketHistoryCollection GetPriceHistory(string tickerSymbol)
        {
            MarketHistoryCollection items = _marketHistory[tickerSymbol];
            return items;
            
        }

        #endregion
    }
}
