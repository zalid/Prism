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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.PresentationModels;
using StockTraderRI.Modules.News.Tests.Mocks;

namespace StockTraderRI.Modules.News.Tests
{
    [TestClass]
    public class ArticlePresenterFixture
    {
        [TestMethod]
        public void CanInitPresenter()
        {
            MockArticleView view = new MockArticleView();
            MockRegionManager regionManager = new MockRegionManager();
            MockNewsFeedService newsFeedService = new MockNewsFeedService();

            ArticlePresenter presenter = new ArticlePresenter(view, newsFeedService);
            Assert.AreEqual<IArticleView>(view, presenter.View);
        }


        [TestMethod]
        public void ShowNewsDoesNothingIfNewsFeedHasNoNews()
        {
            MockArticleView view = new MockArticleView();
            MockRegionManager regionManager = new MockRegionManager();
            MockNewsFeedService newsFeedService = new MockNewsFeedService();
            newsFeedService.NewsArticles = null;
            ArticlePresenter presenter = new ArticlePresenter(view, newsFeedService);
            presenter.Controller = new MockNewsController();

            presenter.SetTickerSymbol("InexistentNews");

            Assert.AreEqual(0, regionManager.MockNewsRegion.AddedViews.Count);
        }

        [TestMethod]
        public void ShowNewsPassesNewsContentToView()
        {
            MockArticleView view = new MockArticleView();
            MockRegionManager regionManager = new MockRegionManager();
            MockNewsFeedService newsFeedService = new MockNewsFeedService();
            newsFeedService.NewsArticles = new List<NewsArticle>() { new NewsArticle() { Title = "FUND0", Body = "My custom body text" } };
            ArticlePresenter presenter = new ArticlePresenter(view, newsFeedService);
            presenter.Controller = new MockNewsController();

            presenter.SetTickerSymbol("FUND0");

            Assert.AreEqual("My custom body text", ((NewsArticle)view.Model.Articles.CurrentItem).Body);
        }

        [TestMethod]
        public void ViewContainsCorrectModelHeaderInfoAfterSetTickerSymbol()
        {
            var view = new MockArticleView();
            var regionManager = new MockRegionManager();
            var newsFeedService = new MockNewsFeedService();
            newsFeedService.NewsArticles = new List<NewsArticle>() { new NewsArticle() { Title = "MySymbol", IconUri = "MyPath" } };
            var presenter = new ArticlePresenter(view, newsFeedService);
            presenter.Controller = new MockNewsController();

            presenter.SetTickerSymbol("MyTitle");

            Assert.IsNotNull(view.Model);
            Assert.IsNotNull(view.Model.Articles);
            Assert.AreEqual("MyPath", ((NewsArticle)view.Model.Articles.CurrentItem).IconUri);
            Assert.AreEqual("MySymbol", ((NewsArticle)view.Model.Articles.CurrentItem).Title);
        }

        [TestMethod]
        public void ArticlePresenterNotifiesControllerOnItemChange()
        {
            var view = new MockArticleView();
            var regionManager = new MockRegionManager();
            var newsFeedService = new MockNewsFeedService();
            var mockController = new MockNewsController();
            newsFeedService.NewsArticles = new List<NewsArticle>() { new NewsArticle() { Title = "MySymbol", IconUri = "MyPath" },
                                                                     new NewsArticle() { Title = "OtherSymbol", IconUri = "OtherPath" }};
            var presenter = new TestableArticlePresenter(view, newsFeedService);
            presenter.Controller = mockController;
            presenter.SetTickerSymbol("DoesNotMatter");

            presenter.GetModel().Articles.MoveCurrentToNext();

            Assert.IsTrue(mockController.CurrentItemWasCalled);
        }

        [TestMethod]
        public void ArticlePresenterCallControllerToShowNewsReader()
        {
            var view = new MockArticleView();
            var regionManager = new MockRegionManager();
            var newsFeedService = new MockNewsFeedService();
            var mockController = new MockNewsController();

            var presenter = new ArticlePresenter(view, newsFeedService);
            presenter.Controller = mockController;

            view.RaiseShowNewsReaderEvent();

            Assert.IsTrue(mockController.ShowNewsReaderCalled);

        }



        private class MockNewsFeedService : INewsFeedService
        {
            public IList<NewsArticle> NewsArticles = new List<NewsArticle>()
                                                         {
                                                             new NewsArticle()
                                                                 {Title = "Title", IconUri = "IconUri", Body = "Body", PublishedDate = DateTime.Now}
                                                         };


            #region INewsFeedService Members

            public IList<NewsArticle> GetNews(string tickerSymbol)
            {
                return NewsArticles;
            }

            public bool HasNews(string tickerSymbol)
            {
                throw new NotImplementedException();
            }

            public event EventHandler<NewsFeedEventArgs> Updated = delegate { };

            #endregion
        }
    }

    internal class MockNewsController : INewsController
    {
        public bool CurrentItemWasCalled = false;

        public bool ShowNewsReaderCalled { get; private set; }

        public void CurrentNewsArticleChanged(NewsArticle article)
        {
            CurrentItemWasCalled = true;
        }

        public void ShowNewsReader()
        {
            ShowNewsReaderCalled = true;
        }
    }

    internal class TestableArticlePresenter : ArticlePresenter
    {
        public TestableArticlePresenter(IArticleView view, INewsFeedService service)
            : base(view, service)
        {
        }

        public ArticlePresentationModel GetModel()
        {
            return Model;
        }
    }
}
