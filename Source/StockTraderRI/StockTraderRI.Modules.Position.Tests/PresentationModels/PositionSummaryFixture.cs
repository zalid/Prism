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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using StockTraderRI.Infrastructure.PresentationModels;
using StockTraderRI.Modules.Position.PresentationModels;

namespace StockTraderRI.Modules.Position.Tests.PresentationModels
{
    /// <summary>
    /// Summary description for PositionSummaryFixture
    /// </summary>
    [TestClass]
    public class PositionSummaryFixture
    {
        [TestMethod]
        public void ChangingHasNewsFiresPropertyChangeNotificationEvent()
        {
            PositionSummaryItem positionSummary = new PositionSummaryItem("FUND0", 49.99M, 50, 52.99M, false);

            bool propertyChanged = false;
            string lastPropertyChanged = string.Empty;
            
            positionSummary.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                                                   {
                                                       propertyChanged = true;
                                                       lastPropertyChanged = e.PropertyName;
                                                   };

            positionSummary.HasNews = true;

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual<string>("HasNews", lastPropertyChanged);
        }

        [TestMethod]
        public void ChangingCurrentPriceFiresPropertyChangeNotificationEvent()
        {
            PositionSummaryItem positionSummary = new PositionSummaryItem("FUND0", 49.99M, 50, 52.99M, false);
            
            bool currentPriceChanged = false;
            positionSummary.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                                                   {
                                                       if (e.PropertyName == "CurrentPrice")
                                                           currentPriceChanged = true;
                                                   };

            positionSummary.CurrentPrice -= 5;

            Assert.IsTrue(currentPriceChanged);
        }

        [TestMethod]
        public void ChangingCostBasisFiresPropertyChangeNotificationEvent()
        {
            PositionSummaryItem positionSummary = new PositionSummaryItem("FUND0", 49.99M, 50, 52.99M, false);

            bool costBasisPropertyChanged = false;
            positionSummary.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                                                   {
                                                       if (e.PropertyName == "CostBasis")
                                                           costBasisPropertyChanged = true;
                                                   };

            positionSummary.CostBasis -= 5;

            Assert.IsTrue(costBasisPropertyChanged);
        }

        [TestMethod]
        public void ChangingSharesFiresPropertyChangeNotificationEvent()
        {
            PositionSummaryItem positionSummary = new PositionSummaryItem("FUND0", 49.99M, 50, 52.99M, false);

            bool sharesPropertyChanged = false;
            positionSummary.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                                                   {
                                                       if (e.PropertyName == "Shares")
                                                           sharesPropertyChanged = true;
                                                   };

            positionSummary.Shares -= 5;

            Assert.IsTrue(sharesPropertyChanged);
        }

        [TestMethod]
        public void ChangingSymbolPropertyChangeNotificationEvent()
        {
            PositionSummaryItem positionSummary = new PositionSummaryItem("AAAA", 49.99M, 50, 52.99M, false);

            bool propertyChanged = false;
            string lastPropertyChanged = string.Empty;

            positionSummary.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
                                                   {
                                                       propertyChanged = true;
                                                       lastPropertyChanged = e.PropertyName;
                                                   };

            positionSummary.TickerSymbol = "XXXX";

            Assert.IsTrue(propertyChanged);
            Assert.AreEqual<string>("TickerSymbol", lastPropertyChanged);
        }

        [TestMethod]
        public void PositionSummaryCalculatesCurrentMarketValue()
        {
            decimal lastPrice = 52.99M;
            long numShares = 50;

            PositionSummaryItem positionSummary = new PositionSummaryItem("AAAA", 49.99M, numShares, lastPrice, false);

            Assert.AreEqual<decimal>(lastPrice * numShares, positionSummary.MarketValue);
        }

        [TestMethod]
        public void PositionSummaryStoresCollectionsOfMarketHistoryValues()
        {
            PositionSummaryItem positionSummary = new PositionSummaryItem("AAAA", 49.99M, 50, 52.99M, false);
            positionSummary.PriceMarketHistory.Add(new MarketHistoryItem(new DateTime(1), 1.00m));
            positionSummary.PriceMarketHistory.Add(new MarketHistoryItem(new DateTime(2), 2.00m));
            positionSummary.PriceMarketHistory.Add(new MarketHistoryItem(new DateTime(3), 30.00m));

            Assert.AreEqual<int>(3, positionSummary.PriceMarketHistory.Count);
        }

        [TestMethod]
        public void PositionSummaryCalculatesGainLossPercent()
        {
            decimal costBasis = 49.99M;
            decimal lastPrice = 52.99M;
            long numShares = 1000;

            PositionSummaryItem positionSummary = new PositionSummaryItem("AAAA", costBasis, numShares, lastPrice, false);

            Assert.AreEqual<decimal>(105901.2002M, Math.Round(positionSummary.GainLossPercent, 4));
        }
	
    }
}