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
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Modules.Market.Services;
using StockTraderRI.Modules.Market.Tests.Properties;

namespace StockTraderRI.Modules.Market.Tests.Services
{
    [TestClass]
    public class MarketFeedServiceFixture
    {
        [TestMethod]
        [DeploymentItem("Data/Market.xml", "Data")]
        public void CanGetPriceAndVolumeFromMarketFeed()
        {
            TestableMarketFeedService marketFeed = new TestableMarketFeedService();
            marketFeed.TestUpdatePrice("STOCK0", 40.00m, 1234);

            Assert.AreEqual<decimal>(40.00m, marketFeed.GetPrice("STOCK0"));
            Assert.AreEqual<long>(1234, marketFeed.GetVolume("STOCK0"));
        }


        [TestMethod]
        [DeploymentItem("Data/Market.xml", "Data")]
        public void ShouldFireUpdatedOnSinglePriceChange()
        {
            TestableMarketFeedService marketFeed = new TestableMarketFeedService();

            bool updateFired = false;
            marketFeed.Updated += delegate
            {
                updateFired = true;
            };

            marketFeed.TestUpdatePrice("STOCK0", 30.00m, 1000);

            Assert.IsTrue(updateFired);
        }

        [TestMethod]
        [DeploymentItem("Data/Market.xml", "Data")]
        public void GetPriceOfNonExistingSymbolThrows()
        {
            MarketFeedService marketFeed = new MarketFeedService();

            try
            {
                marketFeed.GetPrice("NONEXISTANT");
                Assert.Fail("No exception thrown");
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
                Assert.IsTrue(ex.Message.Contains("Symbol does not exist in market feed."));
            }
        }

        [TestMethod]
        [DeploymentItem("Data/Market.xml", "Data")]
        public void SymbolExistsWorksAsExpected()
        {
            MarketFeedService marketFeed = new MarketFeedService();

            Assert.IsTrue(marketFeed.SymbolExists("STOCK0"));
            Assert.IsFalse(marketFeed.SymbolExists("NONEXISTANT"));

        }

        [TestMethod]
        [DeploymentItem("Data/Market.xml", "Data")]
        public void ShouldUpdatePricesWithin5Points()
        {
            TestableMarketFeedService marketFeed = new TestableMarketFeedService();

            decimal originalPrice = marketFeed.GetPrice("STOCK0");
            marketFeed.InvokeUpdatePrices();
            Assert.IsTrue(Math.Abs(marketFeed.GetPrice("STOCK0") - originalPrice) <= 5);
        }

        [TestMethod]
        [DeploymentItem("Data/Market.xml", "Data")]
        public void ShouldFireUpdatedAfterUpdatingPrices()
        {
            var marketFeed = new TestableMarketFeedService();
            bool updateCalled = false;

            marketFeed.Updated += delegate { updateCalled = true; };

            marketFeed.InvokeUpdatePrices();
            Assert.IsTrue(updateCalled);
        }


        [TestMethod]
        public void MarketServiceReadsIntervalFromXml()
        {
            var xmlMarketData = System.Xml.Linq.XDocument.Parse(Resources.TestXmlMarketData);
            var marketFeed = new TestableMarketFeedService(xmlMarketData);

            Assert.AreEqual<int>(5000, marketFeed.RefreshInterval);
        }


        [TestMethod]
        [DeploymentItem("Data/Market.xml", "Data")]
        public void UpdateShouldFireWithinRefreshInterval()
        {
            var marketFeed = new TestableMarketFeedService();
            marketFeed.RefreshInterval = 500; // ms

            var delegateCallCompletedEvent = new System.Threading.ManualResetEvent(false);

            bool updateCalled = false;
            marketFeed.Updated += delegate
            {
                updateCalled = true;
                delegateCallCompletedEvent.Set();
            };

            delegateCallCompletedEvent.WaitOne(5000, true); // Wait up to 5 seconds
            Assert.IsTrue(updateCalled);
        }


        [TestMethod]
        public void RefreshIntervalDefaultsTo10SecondsWhenNotSpecified()
        {
            var xmlMarketData = System.Xml.Linq.XDocument.Parse(Resources.TestXmlMarketData);
            xmlMarketData.Element("MarketItems").Attribute("RefreshRate").Remove();
            var marketFeed = new TestableMarketFeedService(xmlMarketData);

            Assert.AreEqual<int>(10000, marketFeed.RefreshInterval);
        }
    }

    class TestableMarketFeedService : MarketFeedService
    {
        public TestableMarketFeedService()
            : base()
        {

        }

        public TestableMarketFeedService(XDocument xmlDocument)
            : base(xmlDocument)
        {

        }

        public void TestUpdatePrice(string tickerSymbol, decimal price, long volume)
        {
            this.UpdatePrice(tickerSymbol, price, volume);
        }

        public void InvokeUpdatePrices()
        {
            base.UpdatePrices();
        }
    }
}
