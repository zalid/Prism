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

using System.Windows;
using Prism.Commands;
using Prism.Interfaces;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Article;

namespace StockTraderRI.Modules.News.Controllers
{
    public class NewsController : INewsController
    {
        private const string ViewNameKey = "News.{0}";

        private IRegionManager regionManager;
        //private IUnityContainer container;
        private IArticlePresenter articlePresenter;
        private INewsReaderPresenter readerPresenter;

        public NewsController(IRegionManager regionManagerService, IArticlePresenter articlePresenter, StockTraderRICommandProxy commands)
        {
            this.regionManager = regionManagerService;
            //this.container = container;
            this.articlePresenter = articlePresenter;
            this.articlePresenter.Controller = this;
            
            this.regionManager.GetRegion("NewsRegion").Add((UIElement)articlePresenter.View);
            commands.ShowNewsCommand.RegisterCommand(new DelegateCommand<string>(ShowNews));
        }

        public NewsController(IRegionManager regionManagerService, 
                                IArticlePresenter articlePresenter, 
                                StockTraderRICommandProxy commands, 
                                INewsReaderPresenter readerPresenter) :
            this(regionManagerService, articlePresenter, commands)
        {
            this.readerPresenter = readerPresenter;
        }

        public void ShowNews(string companySymbol)
        {
            articlePresenter.SetTickerSymbol(companySymbol);
        }

        public void CurrentNewsArticleChanged(NewsArticle article)
        {
            //throw new NotImplementedException();
            this.readerPresenter.SetNewsArticle(article);
        }

        public void ShowNewsReader()
        {
            readerPresenter.Show();
        }
    }
}
