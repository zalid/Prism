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
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Market.Services;
using StockTraderRI.Modules.Market.Tests.Mocks;
using StockTraderRI.Modules.Market.Tests.Properties;

namespace StockTraderRI.Modules.Market.Tests.Services
{
    [TestClass]
    public class MarketFeedServiceFixture
    {
        [TestMethod]
        public void CanGetPriceAndVolumeFromMarketFeed()
        {
            using (var marketFeed = new TestableMarketFeedService(new MockPriceUpdatedEventAggregator()))
            {
                marketFeed.TestUpdatePrice("STOCK0", 40.00m, 1234);

                Assert.AreEqual<decimal>(40.00m, marketFeed.GetPrice("STOCK0"));
                Assert.AreEqual<long>(1234, marketFeed.GetVolume("STOCK0"));
            }
        }

        [TestMethod]
        public void ShouldFireUpdatedOnSinglePriceChange()
        {
            var eventAggregator = new MockPriceUpdatedEventAggregator();

            using (TestableMarketFeedService marketFeed = new TestableMarketFeedService(eventAggregator))
            {
                marketFeed.TestUpdatePrice("STOCK0", 30.00m, 1000);
            }

            Assert.IsTrue(eventAggregator.MockMarketPriceUpdatedEvent.FireCalled);
        }

        [TestMethod]
        public void GetPriceOfNonExistingSymbolThrows()
        {
            using (var marketFeed = new MarketFeedService(new MockPriceUpdatedEventAggregator()))
            {
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
        }

        [TestMethod]
        public void SymbolExistsWorksAsExpected()
        {
            using (var marketFeed = new MarketFeedService(new MockPriceUpdatedEventAggregator()))
            {
                Assert.IsTrue(marketFeed.SymbolExists("STOCK0"));
                Assert.IsFalse(marketFeed.SymbolExists("NONEXISTANT"));
            }
        }

        [TestMethod]
        public void ShouldUpdatePricesWithin5Points()
        {
            using (var marketFeed = new TestableMarketFeedService(new MockPriceUpdatedEventAggregator()))
            {
                decimal originalPrice = marketFeed.GetPrice("STOCK0");
                marketFeed.InvokeUpdatePrices();
                Assert.IsTrue(Math.Abs(marketFeed.GetPrice("STOCK0") - originalPrice) <= 5);
            }
        }

        [TestMethod]
        public void ShouldFireUpdatedAfterUpdatingPrices()
        {
            var eventAggregator = new MockPriceUpdatedEventAggregator();

            using (var marketFeed = new TestableMarketFeedService(eventAggregator))
            {
                marketFeed.InvokeUpdatePrices();
            }
            Assert.IsTrue(eventAggregator.MockMarketPriceUpdatedEvent.FireCalled);
        }


        [TestMethod]
        public void MarketServiceReadsIntervalFromXml()
        {
            var xmlMarketData = XDocument.Parse(Resources.TestXmlMarketData);
            using (var marketFeed = new TestableMarketFeedService(xmlMarketData, new MockPriceUpdatedEventAggregator()))
            {
                Assert.AreEqual<int>(5000, marketFeed.RefreshInterval);
            }
        }

        [TestMethod]
        public void UpdateShouldFireWithinRefreshInterval()
        {
            var eventAggregator = new MockPriceUpdatedEventAggregator();

            using (var marketFeed = new TestableMarketFeedService(eventAggregator))
            {
                marketFeed.RefreshInterval = 500; // ms

                var callCompletedEvent = new System.Threading.ManualResetEvent(false);

                eventAggregator.MockMarketPriceUpdatedEvent.FireCalledEvent +=
                        delegate { callCompletedEvent.Set(); };

                callCompletedEvent.WaitOne(5000, true); // Wait up to 5 seconds
            }
            Assert.IsTrue(eventAggregator.MockMarketPriceUpdatedEvent.FireCalled);
        }

        [TestMethod]
        public void RefreshIntervalDefaultsTo10SecondsWhenNotSpecified()
        {
            var xmlMarketData = XDocument.Parse(Resources.TestXmlMarketData);
            xmlMarketData.Element("MarketItems").Attribute("RefreshRate").Remove();

            using (var marketFeed = new TestableMarketFeedService(xmlMarketData, new MockPriceUpdatedEventAggregator()))
            {
                Assert.AreEqual<int>(10000, marketFeed.RefreshInterval);
            }
        }

        [TestMethod]
        public void FiredEventContainsTheUpdatedPriceList()
        {
            var eventAgregator = new MockPriceUpdatedEventAggregator();
            var marketFeed = new TestableMarketFeedService(eventAgregator);
            Assert.IsTrue(marketFeed.SymbolExists("STOCK0"));

            marketFeed.InvokeUpdatePrices();

            Assert.IsTrue(eventAgregator.MockMarketPriceUpdatedEvent.FireCalled);
            var payload = eventAgregator.MockMarketPriceUpdatedEvent.FireArgumentPayload;
            Assert.IsNotNull(payload);
            Assert.IsTrue(payload.ContainsKey("STOCK0"));
            Assert.AreEqual(marketFeed.GetPrice("STOCK0"), payload["STOCK0"]);
        }

    }

    class TestableMarketFeedService : MarketFeedService
    {
        public TestableMarketFeedService(MockPriceUpdatedEventAggregator eventAggregator)
            : base(eventAggregator)
        {

        }

        public TestableMarketFeedService(XDocument xmlDocument, MockPriceUpdatedEventAggregator eventAggregator)
            : base(xmlDocument, eventAggregator)
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



    class MockPriceUpdatedEventAggregator : MockEventAggregator
    {
        public MockMarketPricesUpdatedEvent MockMarketPriceUpdatedEvent = new MockMarketPricesUpdatedEvent();
        public MockPriceUpdatedEventAggregator()
        {
            AddMapping<MarketPricesUpdatedEvent>(MockMarketPriceUpdatedEvent);
        }

        public class MockMarketPricesUpdatedEvent : MarketPricesUpdatedEvent
        {
            public bool FireCalled;
            public IDictionary<string, decimal> FireArgumentPayload;
            public EventHandler FireCalledEvent;

            private void OnFireCalledEvent(object sender, EventArgs args)
            {
                if (FireCalledEvent != null)
                    FireCalledEvent(sender, args);
            }

            public override void Fire(IDictionary<string, decimal> payload)
            {
                FireCalled = true;
                FireArgumentPayload = payload;
                OnFireCalledEvent(this, EventArgs.Empty);
            }
        }
    }
}
