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
    public class MockNewsFeedService : INewsFeedService
    {
        Dictionary<string, string> newsData = new Dictionary<string, string>();

        internal void UpdateNews(string tickerSymbol, string headline)
        {
            newsData.Add(tickerSymbol, headline);
            Updated(this, new NewsFeedEventArgs(tickerSymbol,headline));
        }

        #region INewsFeedService Members

        public NewsArticle GetNews(string tickerSymbol)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<NewsFeedEventArgs> Updated = delegate { };

        public bool HasNews(string tickerSymbol)
        {
            return newsData.ContainsKey(tickerSymbol);
        }

        #endregion
    }
}
