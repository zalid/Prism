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

using Prism.Interfaces;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Article;

namespace StockTraderRI.Modules.News.Controllers
{
    public class NewsController : INewsController
    {
        private readonly IRegionManager regionManager;
        private readonly IArticlePresenter articlePresenter;
        private readonly IEventAggregator eventAggregator;
        private readonly INewsReaderPresenter readerPresenter;

        public NewsController(IRegionManager regionManagerService, IArticlePresenter articlePresenter, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManagerService;
            this.articlePresenter = articlePresenter;
            this.eventAggregator = eventAggregator;
            this.articlePresenter.Controller = this;
        }

        public void Run()
        {
            this.regionManager.GetRegion("NewsRegion").Add(articlePresenter.View);
            eventAggregator.Get<TickerSymbolSelectedEvent>().Subscribe(ShowNews, ThreadOption.UIThread);
        }

        public NewsController(IRegionManager regionManagerService,
                                IArticlePresenter articlePresenter,
                                IEventAggregator eventAggregator,
                                INewsReaderPresenter readerPresenter) :
            this(regionManagerService, articlePresenter, eventAggregator)
        {
            this.readerPresenter = readerPresenter;
        }

        public void ShowNews(string companySymbol)
        {
            articlePresenter.SetTickerSymbol(companySymbol);
        }

        public void CurrentNewsArticleChanged(NewsArticle article)
        {
            this.readerPresenter.SetNewsArticle(article);
        }

        public void ShowNewsReader()
        {
            readerPresenter.Show();
        }
    }
}
