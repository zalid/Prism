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
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Prism.Interfaces;
using System.Windows;
using Prism.Utility;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.PresentationModels;

namespace StockTraderRI.Modules.News.Article
{

    public class ArticlePresenter : IArticlePresenter
    {
        public ArticlePresenter(IArticleView view, INewsFeedService newsFeedService)
        {
            View = view;
            NewsFeedService = newsFeedService;
        }

        public IArticleView View { get; set; }
        INewsFeedService NewsFeedService { get; set; }

        public void SetTickerSymbol(string companySymbol)
        {
            NewsArticle newsArticle = NewsFeedService.GetNews(companySymbol);
            if (newsArticle != null)
            {
                ExtendedHeader extendedHeader = new ExtendedHeader();
                extendedHeader.Title = newsArticle.Title;
                extendedHeader.IconUri = newsArticle.IconUri;
                extendedHeader.ToolTip = string.Format(CultureInfo.InvariantCulture, Properties.Resources.LastestStockNewsTooltip, newsArticle.Title);
                ArticlePresentationModel articlePresentationModel = new ArticlePresentationModel();
                articlePresentationModel.ArticleBody = newsArticle.Body;
                articlePresentationModel.HeaderInfo = extendedHeader;

                View.Model = articlePresentationModel;
                
            }
        }
    }
}
