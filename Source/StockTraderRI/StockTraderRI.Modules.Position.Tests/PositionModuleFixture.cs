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

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.Services;
using StockTraderRI.Modules.Position.Tests.Mocks;

namespace StockTraderRI.Modules.Position.Tests
{
    [TestClass]
    public class PositionModuleFixture
    {
        [TestMethod]
        [DeploymentItem("Data", "Data")]
        public void RegisterViewsServicesRegistersViewsAndServices()
        {
            MockUnityContainer container = new MockUnityContainer();

            TestablePositionModule module = new TestablePositionModule(container, new MockRegionManager());

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(typeof(AccountPositionService), container.Types[typeof(IAccountPositionService)]);
            Assert.AreEqual(typeof(PositionSummaryView), container.Types[typeof(IPositionSummaryView)]);
            Assert.AreEqual(typeof(PositionSummaryPresenter), container.Types[typeof(IPositionSummaryPresenter)]);
            Assert.AreEqual(typeof(OrdersView), container.Types[typeof(IOrdersView)]);
            Assert.AreEqual(typeof(OrdersPresenter), container.Types[typeof(IOrdersPresenter)]);
            Assert.AreEqual(typeof(OrderDetailsView), container.Types[typeof(IOrderDetailsView)]);
            Assert.AreEqual(typeof(OrderDetailsPresenter), container.Types[typeof(IOrderDetailsPresenter)]);
            Assert.AreEqual(typeof(OrderCommandsView), container.Types[typeof(IOrderCommandsView)]);
            Assert.AreEqual(typeof(OrderCompositeView), container.Types[typeof(IOrderCompositeView)]);
            Assert.AreEqual(typeof(OrderCompositePresenter), container.Types[typeof(IOrderCompositePresenter)]);
            Assert.AreEqual(typeof(OrdersController), container.Types[typeof(IOrdersController)]);
            Assert.AreEqual(typeof(XmlOrdersService), container.Types[typeof(IOrdersService)]);

        }

        [TestMethod]
        public void InitAddsOrdersToolbarViewToToolbarRegion()
        {
            MockRegion toolbarRegion = new MockRegion();
            MockRegion mainRegion = new MockRegion();
            MockRegionManager regionManager = new MockRegionManager();
            var container = new MockUnityResolver();
            container.Bag.Add(typeof(IOrdersController), new MockOrdersController());
            container.Bag.Add(typeof(IPositionSummaryPresenter), new MockPositionSummaryPresenter());
            PositionModule module = new PositionModule(container, regionManager);
            regionManager.Register("MainRegion", mainRegion);
            regionManager.Register("CollapsibleRegion", new MockRegion());
            regionManager.Register("MainToolbarRegion", toolbarRegion);

            Assert.AreEqual(0, toolbarRegion.AddedViews.Count);
            Assert.AreEqual(0, mainRegion.AddedViews.Count);

            module.Initialize();

            Assert.AreEqual(1, mainRegion.AddedViews.Count);
            Assert.AreEqual(1, toolbarRegion.AddedViews.Count);

        }


        internal class TestablePositionModule : PositionModule
        {
            public TestablePositionModule(IUnityContainer container, IRegionManager regionManager)
                : base(container, regionManager)
            {
            }

            public void InvokeRegisterViewsAndServices()
            {
                base.RegisterViewsAndServices();
            }
        }
    }

    internal class MockPositionSummaryPresenter : IPositionSummaryPresenter
    {
        #region IPositionSummaryPresenter Members

        private IPositionSummaryView _view = new MockPositionSummaryView();

        public IPositionSummaryView View
        {
            get { return _view; }
            set { _view = value; }
        }

        #endregion
    }
}
