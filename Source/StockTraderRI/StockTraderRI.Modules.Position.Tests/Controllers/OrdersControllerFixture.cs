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
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.Tests.Mocks;
using StockTraderRI.Modules.Position.Tests.Orders;

namespace StockTraderRI.Modules.Position.Tests.Controllers
{
    [TestClass]
    public class OrdersControllerFixture
    {
        [TestMethod]
        public void BuyAndSellCommandsInvokeController()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresenter, MockOrdersPresenter>();
            container.RegisterType<IOrderCompositePresenter, MockOrderCompositePresenter>();

            regionManager.Register("OrdersRegion", new MockRegion());
            regionManager.Register("MainRegion", new MockRegion());

            var controller = new TestableOrdersController(regionManager, container);
            controller.BuyCommand.Execute("STOCK01");

            Assert.AreEqual("STOCK01", controller.StartOrderArgumentTickerSymbol);
            Assert.AreEqual(TransactionType.Buy, controller.StartOrderArgumentTransactionType);

            controller.SellCommand.Execute("STOCK02");

            Assert.AreEqual("STOCK02", controller.StartOrderArgumentTickerSymbol);
            Assert.AreEqual(TransactionType.Sell, controller.StartOrderArgumentTransactionType);
        }

        [TestMethod]
        public void ControllerAddsViewIfNotPresent()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresenter, MockOrdersPresenter>();
            container.RegisterType<IOrderCompositePresenter, MockOrderCompositePresenter>();

            var collapsibleRegion = new MockRegion();
            regionManager.Register("OrdersRegion", new MockRegion());
            regionManager.Register("MainRegion", collapsibleRegion);

            var controller = new TestableOrdersController(regionManager, container);

            Assert.AreEqual<int>(0, collapsibleRegion.AddedViews.Count);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
            Assert.AreEqual<int>(1, collapsibleRegion.AddedViews.Count);
        }

        [TestMethod]
        public void ControllerAddsANewOrderOnStartOrder()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresenter, MockOrdersPresenter>();
            container.RegisterType<IOrderCompositePresenter, MockOrderCompositePresenter>();

            var ordersRegion = new MockRegion();
            regionManager.Register("OrdersRegion", ordersRegion);
            regionManager.Register("MainRegion", new MockRegion());

            var controller = new TestableOrdersController(regionManager, container);

            Assert.AreEqual<int>(0, ordersRegion.AddedViews.Count);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
            Assert.AreEqual<int>(1, ordersRegion.AddedViews.Count);
        }

        [TestMethod]
        public void NewOrderIsShownOrder()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresenter, MockOrdersPresenter>();
            container.RegisterType<IOrderCompositePresenter, MockOrderCompositePresenter>();

            var ordersRegion = new MockRegion();
            regionManager.Register("OrdersRegion", ordersRegion);
            regionManager.Register("MainRegion", new MockRegion());

            var controller = new TestableOrdersController(regionManager, container);

            Assert.AreEqual<int>(0, ordersRegion.AddedViews.Count);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
            Assert.AreSame(ordersRegion.SelectedItem, ordersRegion.AddedViews[0]);
        }



        [TestMethod]
        public void OnCloseViewRequestedTheControllerRemovesTheViewFromTheRegionAndDisposesThePresenter()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresenter, MockOrdersPresenter>();
            var presenter = new MockOrderCompositePresenter();
            container.RegisterInstance<IOrderCompositePresenter>(presenter);

            var ordersRegion = new MockRegion();
            regionManager.Register("OrdersRegion", ordersRegion);
            regionManager.Register("MainRegion", new MockRegion());

            var controller = new TestableOrdersController(regionManager, container);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

            Assert.AreEqual(1, ordersRegion.AddedViews.Count);
            presenter.RaiseCloseViewRequested();

            Assert.AreEqual(0, ordersRegion.AddedViews.Count);
            Assert.IsTrue(presenter.DisposeCalled);
        }


        [TestMethod]
        public void StartOrderCreatesCompositePresnterAndPassesCorrectInitInfo()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresenter, MockOrdersPresenter>();
            var presenter = new MockOrderCompositePresenter();
            container.RegisterInstance<IOrderCompositePresenter>(presenter);

            regionManager.Register("OrdersRegion", new MockRegion());
            regionManager.Register("MainRegion", new MockRegion());

            var controller = new TestableOrdersController(regionManager, container);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

            Assert.AreEqual("STOCK01", presenter.SetTransactionInfoArgumentTickerSymbol);
            Assert.AreEqual(TransactionType.Buy, presenter.SetTransactionInfoArgumentTransactionType);
        }
    }


    internal class TestableOrdersController : OrdersController
    {

        public TestableOrdersController(IRegionManager regionManager, IUnityContainer container)
            : base(regionManager, container)
        {
        }

        public string StartOrderArgumentTickerSymbol { get; set; }
        public TransactionType StartOrderArgumentTransactionType { get; set; }

        protected override void StartOrder(string tickerSymbol, TransactionType transactionType)
        {
            base.StartOrder(tickerSymbol, transactionType);

            StartOrderArgumentTickerSymbol = tickerSymbol;
            StartOrderArgumentTransactionType = transactionType;
        }

        public void InvokeStartOrder(TransactionType transactionType, string symbol)
        {
            StartOrder(symbol, transactionType);
        }
    }

    class MockOrdersPresenter : IOrdersPresenter
    {
        private IOrdersView _view = new MockOrdersView();

        public IOrdersView View
        {
            get { return _view; }
            set { _view = value; }
        }
    }

    class MockOrderCompositePresenter : IOrderCompositePresenter, IDisposable
    {
        public string SetTransactionInfoArgumentTickerSymbol;
        public TransactionType SetTransactionInfoArgumentTransactionType;
        private IOrderCompositeView _view = new MockOrderCompositeView();
        public bool DisposeCalled;

        public void SetTransactionInfo(string tickerSymbol, TransactionType transactionType)
        {
            SetTransactionInfoArgumentTickerSymbol = tickerSymbol;
            SetTransactionInfoArgumentTransactionType = transactionType;
        }

        public event EventHandler CloseViewRequested;

        public IOrderCompositeView View
        {
            get { return _view; }
        }

        internal void RaiseCloseViewRequested()
        {
            CloseViewRequested(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            DisposeCalled = true;
        }
    }


}

