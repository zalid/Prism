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
using System.Windows.Data;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.PresentationModels;

namespace StockTraderRI.Modules.News.Article
{

    public class ArticlePresenter : IArticlePresenter
    {

        public ArticlePresenter(IArticleView view, INewsFeedService newsFeedService)
        {
            Model = new ArticlePresentationModel();
            View = view;
            View.Model = Model;
            NewsFeedService = newsFeedService;
            View.ShowNewsReader += View_ShowNewsReader;
        }

        void View_ShowNewsReader(object sender, EventArgs e)
        {
            this.Controller.ShowNewsReader();
        }

        void Articles_CurrentChanged(object sender, EventArgs e)
        {

            if (Model.Articles == null)
            {
                Controller.CurrentNewsArticleChanged(null);
            }
            else
            {
                Controller.CurrentNewsArticleChanged((NewsArticle)Model.Articles.CurrentItem);
            }


        }

        public IArticleView View { get; set; }

        INewsFeedService NewsFeedService { get; set; }

        public INewsController Controller { get; set; }

        public void SetTickerSymbol(string companySymbol)
        {
            if (Model.Articles != null)
            {
                Model.Articles.CurrentChanged -= Articles_CurrentChanged;
            }

            IList<NewsArticle> newsArticles = NewsFeedService.GetNews(companySymbol);

            if (newsArticles == null)
            {
                Model.Articles = null;
                Articles_CurrentChanged(null, null);
            }
            else
            {
                Model.Articles = CollectionViewSource.GetDefaultView(newsArticles);
                Model.Articles.CurrentChanged += Articles_CurrentChanged;
                Articles_CurrentChanged(null, null);
            }
        }

        protected ArticlePresentationModel Model { get; set; }
    }
}
