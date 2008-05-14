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
using System.Globalization;
using System.Xml.Linq;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;

namespace StockTraderRI.Modules.News.Services
{
    public class NewsFeedService : INewsFeedService
    {
        readonly Dictionary<string, List<NewsArticle>> newsData = new Dictionary<string, List<NewsArticle>>();

        public NewsFeedService()
        {
            var document = XDocument.Load("Data/News.xml");
            foreach (var newsItem in document.Descendants("NewsItem"))
            {
                var tickerSymbol = newsItem.Attribute("TickerSymbol").Value;
                if (newsData.ContainsKey(tickerSymbol) == false)
                {
                    newsData.Add(tickerSymbol, new List<NewsArticle>());
                }
                newsData[tickerSymbol].Add(new NewsArticle()
                {
                    PublishedDate = DateTime.Parse(newsItem.Attribute("PublishedDate").Value, CultureInfo.CurrentCulture),
                    Title = newsItem.Element("Title").Value,
                    Body = newsItem.Element("Body").Value,
                    IconUri = newsItem.Attribute("IconUri") != null ? newsItem.Attribute("IconUri").Value : null
                });

            }
        }

        #region INewsFeed Members

        public IList<NewsArticle> GetNews(string tickerSymbol)
        {
            List<NewsArticle> articles = new List<NewsArticle>();
            newsData.TryGetValue(tickerSymbol, out articles);
            return articles;
        }

        public event EventHandler<NewsFeedEventArgs> Updated = delegate { };

        public bool HasNews(string tickerSymbol)
        {
            return newsData.ContainsKey(tickerSymbol);
        }

        #endregion
    }
}
