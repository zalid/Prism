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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Commands;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.News.Controllers;
using Prism;
using System.Windows.Controls;
using Prism.Regions;
using Prism.Interfaces;
using System.Windows;
using Prism.Services;
using StockTraderRI.Modules.News.Tests.Mocks;
using Microsoft.Practices.Unity;
using Prism.Utility;
using StockTraderRI.Modules.News.Article;

namespace StockTraderRI.Modules.News.Tests.Controllers
{
    [TestClass]
    public class NewsControllerFixture
    {
        [TestMethod]
        public void ShowNewsResolvesPresenterAndCallsSetTickerSymbolOnItAndAddsNamedViewToRegion()
        {
            var regionManagerService = new MockRegionManagerService();
            var container = new MockArticlePresenterUnityContainer();
            var presenter = new MockArticlePresenter();
            container.ArticlePresenter = presenter;
            var controller = new NewsController(regionManagerService, container, new MockStockTraderRICommandProxy());

            controller.ShowNews("Test");

            Assert.IsNotNull(presenter.SetTickerSymbolArgumentCompanySymbol);
            Assert.AreEqual("Test", presenter.SetTickerSymbolArgumentCompanySymbol);
            Assert.AreEqual("News.Test", regionManagerService.MockNewsRegion.AddArgumentName);
        }


        [TestMethod]
        public void WhenViewWithSameNameExistsInRegionDoesNotCreatePresenter()
        {
            var regionManagerService = new MockRegionManagerService();
            regionManagerService.MockNewsRegion.GetViewReturnValue = new MockArticleView();
            var container = new MockArticlePresenterUnityContainer();
            var presenter = new MockArticlePresenter();
            container.ArticlePresenter = presenter;
            var controller = new NewsController(regionManagerService, container, new MockStockTraderRICommandProxy());

            controller.ShowNews("EXISTING");

            Assert.AreEqual("News.EXISTING", regionManagerService.MockNewsRegion.GetViewArgumentName);
            Assert.IsNull(presenter.SetTickerSymbolArgumentCompanySymbol);
        }

        [TestMethod]
        public void WhenViewWithSameTitleExistsInRegionShowTheOldOneAgain()
        {
            var regionManagerService = new MockRegionManagerService();
            UIElement view = new MockArticleView();
            regionManagerService.MockNewsRegion.GetViewReturnValue = view;

            var controller = new NewsController(regionManagerService, null, new MockStockTraderRICommandProxy());

            controller.ShowNews("EXISTING");

            Assert.AreSame(view, regionManagerService.MockNewsRegion.ShowArgumentView);
        }

        [TestMethod]
        public void ControllerShowNewsWhenExecutingGlobalCommandInstance()
        {
            var container = new MockArticlePresenterUnityContainer();
            var presenter = new MockArticlePresenter();
            container.ArticlePresenter = presenter;
            var commands = new MockStockTraderRICommandProxy();
            var controller = new NewsController(new MockRegionManagerService(), container, commands);

            commands.ShowNewsCommand.Execute("TEST_SYMBOL");
            Assert.IsNotNull(presenter.SetTickerSymbolArgumentCompanySymbol);
            Assert.AreEqual("TEST_SYMBOL", presenter.SetTickerSymbolArgumentCompanySymbol);
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
            public IArticlePresenter ArticlePresenter;

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
}