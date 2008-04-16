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

using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Prism.Interfaces;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.WatchList.Services;
using StockTraderRI.Modules.WatchList.Tests.Mocks;
using Prism.Services;
using System.Windows;
using StockTraderRI.Modules.Watch;
using StockTraderRI.Modules.Watch.WatchList;
using StockTraderRI.Modules.Watch.AddWatch;

namespace StockTraderRI.Modules.WatchList.Tests
{
    [TestClass]
    public class WatchModuleFixture
    {
        [TestMethod]
        public void RegisterViewsServicesRegistersWatchListView()
        {
            var container = new MockUnityContainer();

            var module = new TestableWatchModule(container, new RegionManagerService());

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(typeof(WatchListService), container.Types[typeof(IWatchListService)]);
            Assert.AreEqual(typeof(WatchListView), container.Types[typeof(IWatchListView)]);
            Assert.AreEqual(typeof(WatchListPresenter), container.Types[typeof(IWatchListPresenter)]);
            Assert.AreEqual(typeof(AddWatchView), container.Types[typeof(IAddWatchView)]);
            Assert.AreEqual(typeof(AddWatchPresenter), container.Types[typeof(IAddWatchPresenter)]);
        }

        [TestMethod]
        public void InitAddsAddWatchControlViewToToolbarRegion()
        {
            var toolbarRegion = new MockRegion();
            var collapsibleRegion = new MockRegion();
            var regionManager = new RegionManagerService();
            var container = new MockUnityResolver();
            container.Bag.Add(typeof(IAddWatchPresenter), new MockAddWatchPresenter());
            container.Bag.Add(typeof(IWatchListPresenter), new MockWatchListPresenter());
            IModule module = new WatchModule(container, regionManager);
            regionManager.Register("CollapsibleRegion", collapsibleRegion);
            regionManager.Register("MainToolbarRegion", toolbarRegion);

            Assert.AreEqual(0, toolbarRegion.Views.Count);
            Assert.AreEqual(0, collapsibleRegion.Views.Count);

            module.Initialize();

            Assert.AreEqual(1, toolbarRegion.Views.Count);
            Assert.AreEqual(1, collapsibleRegion.Views.Count);
        }

        internal class TestableWatchModule : WatchModule
        {
            public TestableWatchModule(IUnityContainer container, IRegionManagerService regionManagerService)
                : base(container, regionManagerService)
            {
            }

            public void InvokeRegisterViewsAndServices()
            {
                base.RegisterViewsAndServices();
            }
        }

        class MockAddWatchPresenter : IAddWatchPresenter
        {
            private IAddWatchView _view = new MockAddWatchView();
            public IAddWatchView View
            {
                get { return _view; }
            }
        }

        class MockWatchListPresenter : IWatchListPresenter
        {
            private IWatchListView _view = new MockWatchListView();

            public IWatchListView View
            {
                get { return _view; }
                set { _view = value; }
            }
        }

    }
}
