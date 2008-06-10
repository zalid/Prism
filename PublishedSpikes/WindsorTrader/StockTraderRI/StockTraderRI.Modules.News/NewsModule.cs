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

using Castle.Core;
using Castle.Windsor;
using Prism.Interfaces;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.Services;

namespace StockTraderRI.Modules.News
{
    public class NewsModule : IModule
    {
        private IWindsorContainer _container;

        public NewsModule(IWindsorContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
            INewsController controller = _container.Resolve<INewsController>();
            controller.Run();

        }
        #endregion

        protected void RegisterViewsAndServices()
        {
            _container.AddComponentWithLifestyle<INewsFeedService, NewsFeedService>(LifestyleType.Singleton);
            _container.AddComponentWithLifestyle<INewsController, NewsController>(LifestyleType.Singleton);
            _container.AddComponentWithLifestyle<IArticleView, ArticleView>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IArticlePresentationModel, ArticlePresentationModel>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<INewsReaderView, NewsReader>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<INewsReaderPresenter, NewsReaderPresenter>(LifestyleType.Transient);
        }


    }
}
