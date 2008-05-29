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
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using StockTraderRI.Infrastructure.PresentationModels;

namespace StockTraderRI.Modules.Position.PresentationModels
{
    public class PositionSummaryPresentationModel
    {
        public PositionSummaryPresentationModel()
        {
            Data = new ObservableCollection<PositionSummaryItem>();
        }

        public decimal GetPrice(string ticker)
        {
            return FindTickerItem(ticker).CurrentPrice;
        }

        public long GetQuantity(string ticker)
        {
            return FindTickerItem(ticker).Shares;
        }

        public void AddPosition(string ticker, decimal costBasis, long quantity, decimal currentPrice, MarketHistoryCollection priceHistoryCollection)
        {
            PositionSummaryItem position = new PositionSummaryItem(ticker, costBasis, quantity, currentPrice);
            position.PriceMarketHistory = priceHistoryCollection;
            this.Data.Add(position);
        }

        private PositionSummaryItem FindTickerItemSafe(string ticker)
        {
            return this.Data.FirstOrDefault<PositionSummaryItem>(n => n.TickerSymbol == ticker ? true : false);
        }

        private PositionSummaryItem FindTickerItem(string ticker)
        {
            PositionSummaryItem item = FindTickerItemSafe(ticker);
            if (item == null)
            {
                throw new ArgumentOutOfRangeException("ticker");
            }

            return item;
        }

        public ObservableCollection<PositionSummaryItem> Data { get; private set; }
        public DelegateCommand<string> BuyCommand { get; set; }
        public DelegateCommand<string> SellCommand { get; set; }
    }


}
