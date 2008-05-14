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
using System.Windows;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Commands;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.Tests.Mocks;

namespace StockTraderRI.Modules.News.Tests.Controllers
{
    [TestClass]
    public class NewsControllerFixture
    {
        [TestMethod]
        public void ShowNewsResolvesPresenterAndCallsSetTickerSymbolOnItAndAddsNamedViewToRegion()
        {
            var regionManagerService = new MockRegionManager();
            var presenter = new MockArticlePresenter();
            var controller = new NewsController(regionManagerService, presenter, new MockStockTraderRICommandProxy());

            controller.ShowNews("Test");

            Assert.IsNotNull(presenter.SetTickerSymbolArgumentCompanySymbol);
            Assert.AreEqual("Test", presenter.SetTickerSymbolArgumentCompanySymbol);
        }


        [TestMethod]
        [Ignore]
        public void WhenViewWithSameNameExistsInRegionDoesNotCreatePresenter()
        {
            var regionManager = new MockRegionManager();
            regionManager.MockNewsRegion.GetViewReturnValue = new MockArticleView();
            var presenter = new MockArticlePresenter();
            var controller = new NewsController(regionManager, presenter, new MockStockTraderRICommandProxy());

            controller.ShowNews("EXISTING");

            Assert.AreEqual("News.EXISTING", regionManager.MockNewsRegion.GetViewArgumentName);
            Assert.IsNull(presenter.SetTickerSymbolArgumentCompanySymbol);
        }

        [TestMethod]
        [Ignore]
        public void WhenViewWithSameTitleExistsInRegionShowTheOldOneAgain()
        {
            var regionManager = new MockRegionManager();
            UIElement view = new MockArticleView();
            regionManager.MockNewsRegion.GetViewReturnValue = view;

            var controller = new NewsController(regionManager, null, new MockStockTraderRICommandProxy());

            controller.ShowNews("EXISTING");

            Assert.AreSame(view, regionManager.MockNewsRegion.ShowArgumentView);
        }

        [TestMethod]
        public void ControllerShowNewsWhenExecutingGlobalCommandInstance()
        {
            var presenter = new MockArticlePresenter();
            var commands = new MockStockTraderRICommandProxy();
            var controller = new NewsController(new MockRegionManager(), presenter, commands);

            commands.ShowNewsCommand.Execute("TEST_SYMBOL");
            Assert.IsNotNull(presenter.SetTickerSymbolArgumentCompanySymbol);
            Assert.AreEqual("TEST_SYMBOL", presenter.SetTickerSymbolArgumentCompanySymbol);
        }

        [TestMethod]
        public void ShouldNotifyReaderWhenCurrentNewsArticleChanges()
        {
            var presenter = new MockArticlePresenter();
            var commands = new MockStockTraderRICommandProxy();
            var newsReaderPresenter = new MockNewsReaderPresenter();

            var controller = new NewsController(new MockRegionManager(), presenter, commands, newsReaderPresenter);

            controller.CurrentNewsArticleChanged(new NewsArticle() {Title = "SomeTitle", Body = "Newsbody"});

            Assert.IsTrue(newsReaderPresenter.SetNewsArticleCalled);
        }

        [TestMethod]
        public void ControllerShowNewsViewWhenArticlePresenterReceivesEvent()
        {
            var presenter = new MockArticlePresenter();
            var commands = new MockStockTraderRICommandProxy();
            var newsReaderPresenter = new MockNewsReaderPresenter();

            var controller = new NewsController(new MockRegionManager(), presenter, commands, newsReaderPresenter);

            controller.ShowNewsReader();

            Assert.IsTrue(newsReaderPresenter.ShowWasCalled);
        }

        class MockArticlePresenter : IArticlePresenter
        {
            public MockArticleView MockArticleView = new MockArticleView();
            public string SetTickerSymbolArgumentCompanySymbol;
            public void SetTickerSymbol(string companySymbol)
            {
                SetTickerSymbolArgumentCompanySymbol = companySymbol;
            }

            public IArticleView View
            {
                get { return MockArticleView; }
            }

            public INewsController Controller { get; set;}

        }

        class MockStockTraderRICommandProxy : StockTraderRICommandProxy
        {
            readonly CompositeCommand _showNewsCommand = new CompositeCommand();
            public override CompositeCommand ShowNewsCommand
            {
                get { return _showNewsCommand; }
            }
        }

        class MockArticlePresenterUnityContainer : IUnityContainer
        {
            public IArticlePresenter ArticlePresenter = null;

            public T Resolve<T>()
            {
                return (T)ArticlePresenter;
            }

            #region IUnityContainer Members

            public IUnityContainer AddExtension(UnityContainerExtension extension)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer AddNewExtension<TExtension>() where TExtension : UnityContainerExtension, new()
            {
                throw new NotImplementedException();
            }

            public object BuildUp(Type t, object existing, string name)
            {
                throw new NotImplementedException();
            }

            public object BuildUp(Type t, object existing)
            {
                throw new NotImplementedException();
            }

            public T BuildUp<T>(T existing, string name)
            {
                throw new NotImplementedException();
            }

            public T BuildUp<T>(T existing)
            {
                throw new NotImplementedException();
            }

            public object Configure(Type configurationInterface)
            {
                throw new NotImplementedException();
            }

            public TConfigurator Configure<TConfigurator>() where TConfigurator : IUnityContainerExtensionConfigurator
            {
                throw new NotImplementedException();
            }

            public IUnityContainer CreateChildContainer()
            {
                throw new NotImplementedException();
            }

            public IUnityContainer Parent
            {
                get { throw new NotImplementedException(); }
            }

            public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterInstance(Type t, string name, object instance)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterInstance(Type t, object instance, LifetimeManager lifetimeManager)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterInstance(Type t, object instance)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterInstance<TInterface>(string name, TInterface instance, LifetimeManager lifetimeManager)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterInstance<TInterface>(string name, TInterface instance)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterInstance<TInterface>(TInterface instance, LifetimeManager lifetimeManager)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterInstance<TInterface>(TInterface instance)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType(Type t, string name, LifetimeManager lifetimeManager)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType(Type t, LifetimeManager lifetimeManager)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType(Type from, Type to, LifetimeManager lifetimeManager)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType(Type from, Type to, string name)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType(Type from, Type to)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType<T>(string name, LifetimeManager lifetimeManager)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType<T>(LifetimeManager lifetimeManager)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType<TFrom, TTo>(string name, LifetimeManager lifetimeManager) where TTo : TFrom
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType<TFrom, TTo>(string name) where TTo : TFrom
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RemoveAllExtensions()
            {
                throw new NotImplementedException();
            }

            public object Resolve(Type t, string name)
            {
                throw new NotImplementedException();
            }

            public object Resolve(Type t)
            {
                throw new NotImplementedException();
            }

            public T Resolve<T>(string name)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager) where TTo : TFrom
            {
                throw new NotImplementedException();
            }

            public IUnityContainer RegisterType<TFrom, TTo>() where TTo : TFrom
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> ResolveAll(Type t)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> ResolveAll<T>()
            {
                throw new NotImplementedException();
            }

            public void Teardown(object o)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            #endregion

        }
    }

    internal class MockNewsReaderPresenter : INewsReaderPresenter
    {
        public bool SetNewsArticleCalled { get; set; }

        public bool ShowWasCalled{ get; private set; }

        public void SetNewsArticle(NewsArticle article)
        {
            SetNewsArticleCalled = true;    
        }

        public void Show()
        {
            ShowWasCalled = true;
        }
    }

   
}
