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
using Microsoft.Practices.Unity;
using StockTraderRI.Modules.News.Tests.Mocks;
using Prism.Interfaces;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Article;

namespace StockTraderRI.Modules.News.Tests
{
    [TestClass]
    public class ArticlePresenterFixture
    {
        [TestMethod]
        public void CanInitPresenter()
        {
            MockArticleView view = new MockArticleView();
            MockRegionManagerService regionManager = new MockRegionManagerService();
            MockNewsFeedService newsFeedService = new MockNewsFeedService();

            ArticlePresenter presenter = new ArticlePresenter(view, newsFeedService);
            Assert.AreEqual<IArticleView>(view, presenter.View);
        }


        [TestMethod]
        public void ShowNewsDoesNothingIfNewsFeedHasNoNews()
        {
            MockArticleView view = new MockArticleView();
            MockRegionManagerService regionManager = new MockRegionManagerService();
            MockNewsFeedService newsFeedService = new MockNewsFeedService();
            newsFeedService.NewsArticle = null;
            ArticlePresenter presenter = new ArticlePresenter(view, newsFeedService);

            presenter.SetTickerSymbol("InexistentNews");

            Assert.AreEqual(0, regionManager.MockNewsRegion.Views.Count);
        }

        [TestMethod]
        public void ShowNewsPassesNewsContentToView()
        {
            MockArticleView view = new MockArticleView();
            MockRegionManagerService regionManager = new MockRegionManagerService();
            MockNewsFeedService newsFeedService = new MockNewsFeedService();
            newsFeedService.NewsArticle = new NewsArticle() { Title = "FUND0", Body = "My custom body text" };
            ArticlePresenter presenter = new ArticlePresenter(view, newsFeedService);

            presenter.SetTickerSymbol("FUND0");

            Assert.AreEqual("My custom body text", view.Model.ArticleBody);
        }

        [TestMethod]
        public void ViewContainsCorrectModelHeaderInfoAfterSetTickerSymbol()
        {
            var view = new MockArticleView();
            var regionManager = new MockRegionManagerService();
            var newsFeedService = new MockNewsFeedService();
            newsFeedService.NewsArticle = new NewsArticle() { Title = "MySymbol", IconUri = "MyPath" };
            var presenter = new ArticlePresenter(view, newsFeedService);

            presenter.SetTickerSymbol("MyTitle");

            Assert.IsNotNull(view.Model);
            Assert.IsNotNull(view.Model.HeaderInfo);
            Assert.AreEqual("MyPath", view.Model.HeaderInfo.IconUri);
            Assert.AreEqual("MySymbol", view.Model.HeaderInfo.Title);
        }


        class MockNewsFeedService : INewsFeedService
        {
            public NewsArticle NewsArticle = new NewsArticle() { Title = "Title", IconUri = "IconUri", Body = "Body" };

            #region INewsFeedService Members

            public NewsArticle GetNews(string tickerSymbol)
            {
                return NewsArticle;
            }

            public bool HasNews(string tickerSymbol)
            {
                throw new NotImplementedException();
            }

            public event EventHandler<NewsFeedEventArgs> Updated;

            #endregion
        }
    }
}
