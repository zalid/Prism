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
using System.Xml.Serialization;
using System.Xml;
using StockTraderRI.Infrastructure.Models;
using System.Xml.Linq;

namespace StockTraderRI.Modules.News.Services
{
    public class NewsFeedService : INewsFeedService
    {
        readonly Dictionary<string, NewsArticle> newsData = new Dictionary<string, NewsArticle>();

        public NewsFeedService()
        {
            var document = XDocument.Load("Data/News.xml");
            foreach (var newsItem in document.Descendants("NewsItem"))
            {
                var newsArticle = new NewsArticle
                                      {
                                          Title = newsItem.Attribute("TickerSymbol").Value,
                                          IconUri = newsItem.Attribute("IconUri") != null ? newsItem.Attribute("IconUri").Value : null,
                                          Body = newsItem.Value,
                                      };
                newsData.Add(newsArticle.Title, newsArticle);
            }
        }

        #region INewsFeed Members

        public NewsArticle GetNews(string tickerSymbol)
        {
            return newsData[tickerSymbol];
        }

        public event EventHandler<NewsFeedEventArgs> Updated = delegate { };

        public bool HasNews(string tickerSymbol)
        {
            return newsData.ContainsKey(tickerSymbol);
        }

        #endregion
    }
}
