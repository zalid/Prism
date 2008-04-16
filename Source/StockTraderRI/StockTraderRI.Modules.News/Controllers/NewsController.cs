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
using Prism.Commands;
using StockTraderRI.Infrastructure;
using Prism.Interfaces;
using Microsoft.Practices.Unity;
using System.Windows;
using StockTraderRI.Modules.News.Article;
using System.Globalization;

namespace StockTraderRI.Modules.News.Controllers
{
    public class NewsController : INewsController
    {
        private const string ViewNameKey = "News.{0}";

        private IRegionManagerService regionManagerService;
        private IUnityContainer container;

        public NewsController(IRegionManagerService regionManagerService, IUnityContainer container, StockTraderRICommandProxy commands)
        {
            this.regionManagerService = regionManagerService;
            this.container = container;
            commands.ShowNewsCommand.RegisterCommand(new DelegateCommand<string>(ShowNews));
        }

        public void ShowNews(string companySymbol)
        {
            IRegion newsRegion = regionManagerService.GetRegion("ResearchArticlesRegion");
            string viewName = String.Format(CultureInfo.InvariantCulture, ViewNameKey, companySymbol);

            UIElement view = newsRegion.GetView(viewName);
            if (view != null)
            {
                newsRegion.Show(view);
                return;
            }

            IArticlePresenter presenter = container.Resolve<IArticlePresenter>();
            presenter.SetTickerSymbol(companySymbol);
            newsRegion.Add((UIElement)presenter.View, viewName);
        }
    }
}