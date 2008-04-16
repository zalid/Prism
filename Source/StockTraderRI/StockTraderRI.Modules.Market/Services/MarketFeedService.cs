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
using System.Threading;
using System.Xml;
using System.Globalization;
using System.Xml.Linq;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Market.Properties;

namespace StockTraderRI.Modules.Market.Services
{
    public class MarketFeedService : IMarketFeedService, IDisposable
    {
        private Dictionary<string, decimal> _priceList = new Dictionary<string, decimal>();
        private Dictionary<string, long> _volumeList = new Dictionary<string, long>();
        static Random randomGenerator = new Random(unchecked((int)DateTime.Now.Ticks));
        private Timer _timer;
        private int _refreshInterval = 10000;

        public MarketFeedService() : this (XDocument.Load("Data/Market.xml"))
        {
        }

        protected MarketFeedService(XDocument document)
        {
            _timer = new Timer(TimerTick);

            var marketItems = document.Element("MarketItems");
            var items = marketItems.Elements("MarketItem");
            foreach (var item in items)
            {
                string tickerSymbol = item.Attribute("TickerSymbol").Value;
                decimal lastPrice = decimal.Parse(item.Attribute("LastPrice").Value, NumberStyles.Float, CultureInfo.InvariantCulture);
                long volume = Convert.ToInt64(item.Attribute("Volume").Value, CultureInfo.InvariantCulture);
                _priceList.Add(tickerSymbol, lastPrice);
                _volumeList.Add(tickerSymbol, volume);
            }

            var refreshRateAttribute = marketItems.Attribute("RefreshRate");
            if (refreshRateAttribute != null)
            {
                RefreshInterval = CalculateRefreshIntervalMillisecondsFromSeconds(int.Parse(refreshRateAttribute.Value));
            }
        }

        private static int CalculateRefreshIntervalMillisecondsFromSeconds(int seconds)
        {
            return seconds * 1000;
        }

        public int RefreshInterval
        {
            get { return _refreshInterval; }
            set
            {
                _refreshInterval = value;
                _timer.Change(_refreshInterval, _refreshInterval);
            }
        }

        /// <summary>
        /// Callback for Timer
        /// </summary>
        /// <param name="state"></param>
        private void TimerTick(object state)
        {
            UpdatePrices();
        }

        public decimal GetPrice(string tickerSymbol)
        {
            if (!SymbolExists(tickerSymbol))
                throw new ArgumentException(Resources.MarketFeedTickerSymbolNotFoundException, "tickerSymbol");

            return _priceList[tickerSymbol];
        }

        public long GetVolume(string tickerSymbol)
        {
            return _volumeList[tickerSymbol];
        }

        public bool SymbolExists(string tickerSymbol)
        {
            return _priceList.ContainsKey(tickerSymbol);
        }

        public event EventHandler Updated = delegate { };


        protected void UpdatePrice(string tickerSymbol, decimal newPrice, long newVolume)
        {
            _priceList[tickerSymbol] = newPrice;
            _volumeList[tickerSymbol] = newVolume;
            OnUpdated(this, EventArgs.Empty);
        }

        protected virtual void OnUpdated(object sender, EventArgs e)
        {
            Updated(sender, e);
        }

        protected void UpdatePrices()
        {
            foreach (string symbol in _priceList.Keys.ToArray())
            {
                decimal newValue = _priceList[symbol];
                newValue += Convert.ToDecimal(randomGenerator.NextDouble() * 10f) - 5m;
                _priceList[symbol] = newValue > 0 ? newValue : 0.1m;
            }
            OnUpdated(this, EventArgs.Empty);
        }


        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_timer != null)
                _timer.Dispose();
            _timer = null;
        }

        // Use C# destructor syntax for finalization code.
        ~MarketFeedService()
        {
            Dispose(false);
        } 
    }
}
