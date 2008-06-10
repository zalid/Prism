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
using System.Windows.Input;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Models;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.Tests.Mocks;
using StockTraderRI.Modules.Position.Tests.Orders;
using StockTraderRI.Infrastructure.Interfaces;

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
            container.RegisterType<IOrdersPresentationModel, MockOrdersPresentationModel>();
            container.RegisterType<IOrderCompositePresentationModel, MockOrderCompositePresentationModel>();

            regionManager.Regions.Add("OrdersRegion", new MockRegion());
            regionManager.Regions.Add("MainRegion", new MockRegion());

            var controller = new TestableOrdersController(regionManager, container, new MockStockTraderRICommandProxy(), null);
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
            container.RegisterType<IOrdersPresentationModel, MockOrdersPresentationModel>();
            container.RegisterType<IOrderCompositePresentationModel, MockOrderCompositePresentationModel>();

            var collapsibleRegion = new MockRegion();
            regionManager.Regions.Add("OrdersRegion", new MockRegion());
            regionManager.Regions.Add("MainRegion", collapsibleRegion);

            var controller = new TestableOrdersController(regionManager, container, new MockStockTraderRICommandProxy(), null);

            Assert.AreEqual<int>(0, collapsibleRegion.AddedViews.Count);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
            Assert.AreEqual<int>(1, collapsibleRegion.AddedViews.Count);
        }

        [TestMethod]
        public void ControllerAddsANewOrderOnStartOrder()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresentationModel, MockOrdersPresentationModel>();
            container.RegisterType<IOrderCompositePresentationModel, MockOrderCompositePresentationModel>();

            var ordersRegion = new MockRegion();
            regionManager.Regions.Add("OrdersRegion", ordersRegion);
            regionManager.Regions.Add("MainRegion", new MockRegion());

            var controller = new TestableOrdersController(regionManager, container, new MockStockTraderRICommandProxy(), null);

            Assert.AreEqual<int>(0, ordersRegion.AddedViews.Count);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
            Assert.AreEqual<int>(1, ordersRegion.AddedViews.Count);
        }

        [TestMethod]
        public void NewOrderIsShownOrder()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresentationModel, MockOrdersPresentationModel>();
            container.RegisterType<IOrderCompositePresentationModel, MockOrderCompositePresentationModel>();

            var ordersRegion = new MockRegion();
            regionManager.Regions.Add("OrdersRegion", ordersRegion);
            regionManager.Regions.Add("MainRegion", new MockRegion());

            var controller = new TestableOrdersController(regionManager, container, new MockStockTraderRICommandProxy(), null);

            Assert.AreEqual<int>(0, ordersRegion.AddedViews.Count);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");
            Assert.AreSame(ordersRegion.SelectedItem, ordersRegion.AddedViews[0]);
        }

        [TestMethod]
        public void StartOrderHooksInstanceCommandsToGlobalSaveAllAndCancelAllCommands()
        {
            var container = new MockUnityResolver();
            var regionManager = new MockRegionManager();
            var orderCompositePresenter = new MockOrderCompositePresentationModel();
            container.Bag.Add(typeof(IOrdersPresentationModel), new MockOrdersPresentationModel());
            container.Bag.Add(typeof(IOrderCompositePresentationModel), orderCompositePresenter);

            var ordersRegion = new MockRegion();
            regionManager.Regions.Add("OrdersRegion", ordersRegion);
            regionManager.Regions.Add("MainRegion", new MockRegion());
            var commandProxy = new MockStockTraderRICommandProxy();

            var controller = new TestableOrdersController(regionManager, container, commandProxy, null);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

            Assert.IsFalse(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);
            commandProxy.SubmitAllOrdersCommand.Execute(null);
            Assert.IsTrue(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);

            Assert.IsFalse(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
            commandProxy.CancelAllOrdersCommand.Execute(null);
            Assert.IsTrue(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
        }

        [TestMethod]
        public void StartOrderHooksInstanceCommandsToGlobalSaveAndCancelCommands()
        {
            var container = new MockUnityResolver();
            var regionManager = new MockRegionManager();
            var orderCompositePresenter = new MockOrderCompositePresentationModel();
            container.Bag.Add(typeof(IOrdersPresentationModel), new MockOrdersPresentationModel());
            container.Bag.Add(typeof(IOrderCompositePresentationModel), orderCompositePresenter);

            var ordersRegion = new MockRegion();
            regionManager.Regions.Add("OrdersRegion", ordersRegion);
            regionManager.Regions.Add("MainRegion", new MockRegion());
            var commandProxy = new MockStockTraderRICommandProxy();

            var controller = new TestableOrdersController(regionManager, container, commandProxy, null);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

            Assert.IsFalse(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);
            commandProxy.SubmitOrderCommand.Execute(null);
            Assert.IsTrue(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);

            Assert.IsFalse(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
            commandProxy.CancelOrderCommand.Execute(null);
            Assert.IsTrue(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
        }


        [TestMethod]
        public void OnCloseViewRequestedTheControllerUnhooksGlobalCommands()
        {
            var container = new MockUnityResolver();
            var regionManager = new MockRegionManager();
            var orderCompositePresenter = new MockOrderCompositePresentationModel();
            container.Bag.Add(typeof(IOrdersPresentationModel), new MockOrdersPresentationModel());
            container.Bag.Add(typeof(IOrderCompositePresentationModel), orderCompositePresenter);

            var ordersRegion = new MockRegion();
            regionManager.Regions.Add("OrdersRegion", ordersRegion);
            regionManager.Regions.Add("MainRegion", new MockRegion());
            var commandProxy = new MockStockTraderRICommandProxy();

            var controller = new TestableOrdersController(regionManager, container, commandProxy, null);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

            Assert.AreEqual(1, ordersRegion.AddedViews.Count);
            orderCompositePresenter.RaiseCloseViewRequested();

            Assert.AreEqual(0, ordersRegion.AddedViews.Count);

            commandProxy.SubmitAllOrdersCommand.Execute(null);
            Assert.IsFalse(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);

            commandProxy.CancelAllOrdersCommand.Execute(null);
            Assert.IsFalse(orderCompositePresenter.MockCancelCommand.ExecuteCalled);

            commandProxy.SubmitOrderCommand.Execute(null);
            Assert.IsFalse(orderCompositePresenter.MockSubmitCommand.ExecuteCalled);

            commandProxy.CancelOrderCommand.Execute(null);
            Assert.IsFalse(orderCompositePresenter.MockCancelCommand.ExecuteCalled);
        }


        [TestMethod]
        public void StartOrderCreatesCompositePresnterAndPassesCorrectInitInfo()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresentationModel, MockOrdersPresentationModel>();
            var presenter = new MockOrderCompositePresentationModel();
            container.RegisterInstance<IOrderCompositePresentationModel>(presenter);

            regionManager.Regions.Add("OrdersRegion", new MockRegion());
            regionManager.Regions.Add("MainRegion", new MockRegion());

            var controller = new TestableOrdersController(regionManager, container, new MockStockTraderRICommandProxy(), null);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

            Assert.AreEqual("STOCK01", presenter.SetTransactionInfoArgumentTickerSymbol);
            Assert.AreEqual(TransactionType.Buy, presenter.SetTransactionInfoArgumentTransactionType);
        }

        [TestMethod]
        public void ControllerMaintainsListOfOrderDetailsPresentationModels()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new MockRegionManager();
            container.RegisterType<IOrdersPresentationModel, MockOrdersPresentationModel>();
            container.RegisterType<IOrderCompositePresentationModel, MockOrderCompositePresentationModel>();

            regionManager.Regions.Add("OrdersRegion", new MockRegion());
            regionManager.Regions.Add("MainRegion", new MockRegion());
            var controller = new TestableOrdersController(regionManager, container, new MockStockTraderRICommandProxy(), null);
            
            controller.InvokeStartOrder(TransactionType.Sell, "stock0");
            controller.InvokeStartOrder(TransactionType.Sell, "stock1");

            Assert.AreEqual(2, controller.OrderModels.Count);
            Assert.AreEqual("stock0", controller.OrderModels[0].TickerSymbol);
            Assert.AreEqual("stock1", controller.OrderModels[1].TickerSymbol);
        }

        [TestMethod]
        public void WhenTwoSellOrdersExceedSharesOwnedCanExecuteReturnsFalse()
        {
            var accountPositionService = new MockAccountPositionService();
            accountPositionService.AddPosition("stock1", 10.0M, 100);
            var controller = new TestableOrdersController(null, null, new MockStockTraderRICommandProxy(), accountPositionService);
            var buyOrderModel1 = new MockOrderDetailsPresentationModel() { TickerSymbol = "stock1", Shares = 200, StopLimitPrice = 1.0M, TransactionType = TransactionType.Buy };
            var sellOrderModel1 = new MockOrderDetailsPresentationModel() { TickerSymbol = "stock1", Shares = 100, StopLimitPrice = 1.0M, TransactionType = TransactionType.Sell };
            var sellOrderModel2 = new MockOrderDetailsPresentationModel() { TickerSymbol = "stock1", Shares = 1000, StopLimitPrice = 1.0M, TransactionType = TransactionType.Sell };
            var buyOrderModel2 = new MockOrderDetailsPresentationModel() { TickerSymbol = "stock1", Shares = 1000, StopLimitPrice = 1.0M, TransactionType = TransactionType.Buy };

            //buy 200, total of 300
            controller.OrderModels.Add(buyOrderModel1);

            Assert.IsTrue(controller.SubmitAllCommand.CanExecute(null));

            //sell 100, total of 200
            controller.OrderModels.Add(sellOrderModel1);

            Assert.IsTrue(controller.SubmitAllCommand.CanExecute(null));

            //sell 1000, INVALID since total would be -800
            controller.OrderModels.Add(sellOrderModel2);

            Assert.IsFalse(controller.SubmitAllCommand.CanExecute(null));

            //buy 1000, total of 200
            controller.OrderModels.Add(buyOrderModel2);

            Assert.IsTrue(controller.SubmitAllCommand.CanExecute(null));
        }

        [TestMethod]
        public void OnCloseViewRequestedTheControllerRemovesReferenceToOrderModel()
        {
            var container = new MockUnityResolver();
            var regionManager = new MockRegionManager();
            var orderCompositePresenter = new MockOrderCompositePresentationModel();
            container.Bag.Add(typeof(IOrdersPresentationModel), new MockOrdersPresentationModel());
            container.Bag.Add(typeof(IOrderCompositePresentationModel), orderCompositePresenter);

            var ordersRegion = new MockRegion();
            regionManager.Regions.Add("OrdersRegion", ordersRegion);
            regionManager.Regions.Add("MainRegion", new MockRegion());
            var commandProxy = new MockStockTraderRICommandProxy();

            var controller = new TestableOrdersController(regionManager, container, commandProxy, null);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

            Assert.AreEqual(1, controller.OrderModels.Count);
            orderCompositePresenter.RaiseCloseViewRequested();

            Assert.AreEqual(0, controller.OrderModels.Count);
        }

        [TestMethod]
        public void SubmitAllInstanceCommandHookedToGlobalSubmitAllCommands()
        {
            var container = new MockUnityResolver();
            var regionManager = new MockRegionManager();
            var orderCompositePresenter = new MockOrderCompositePresentationModel();
            container.Bag.Add(typeof(IOrdersPresentationModel), new MockOrdersPresentationModel());
            container.Bag.Add(typeof(IOrderCompositePresentationModel), orderCompositePresenter);

            var ordersRegion = new MockRegion();
            regionManager.Regions.Add("OrdersRegion", ordersRegion);
            regionManager.Regions.Add("MainRegion", new MockRegion());
            var commandProxy = new MockStockTraderRICommandProxy();
            
            var accountPositionService = new MockAccountPositionService();
            accountPositionService.AddPosition("STOCK01", 10.0M, 100);

            var controller = new TestableOrdersController(regionManager, container, commandProxy, accountPositionService);
            controller.InvokeStartOrder(TransactionType.Buy, "STOCK01");

            Assert.IsFalse(controller.SubmitAllCommandCalled);
            commandProxy.SubmitAllOrdersCommand.CanExecute(null);
            Assert.IsTrue(controller.SubmitAllCommandCalled);
        }
	
    }


    internal class TestableOrdersController : OrdersController
    {

        public TestableOrdersController(IRegionManager regionManager, IUnityContainer container, MockStockTraderRICommandProxy commandProxy, IAccountPositionService accountPositionService)
            : base(regionManager, container, commandProxy, accountPositionService)
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

        public bool SubmitAllCommandCalled = false;

        protected override bool SubmitAllCanExecute(object parameter)
        {
            SubmitAllCommandCalled = true;
            return base.SubmitAllCanExecute(parameter);
        }
    }

    class MockOrdersPresentationModel : IOrdersPresentationModel
    {
        private IOrdersView _view = new MockOrdersView();

        public IOrdersView View
        {
            get { return _view; }
            set { _view = value; }
        }

        public string HeaderInfo
        {
            get { throw new NotImplementedException(); }
        }
    }

    class MockOrderCompositePresentationModel : IOrderCompositePresentationModel
    {
        public MockCommand MockSubmitCommand = new MockCommand();
        public MockCommand MockCancelCommand = new MockCommand();
        public MockOrderDetailsPresentationModel _mockOrderDetailsPresentationModel = new MockOrderDetailsPresentationModel();
        public string SetTransactionInfoArgumentTickerSymbol;
        public TransactionType SetTransactionInfoArgumentTransactionType;
        private IOrderCompositeView _view = new MockOrderCompositeView();

        public void SetTransactionInfo(string tickerSymbol, TransactionType transactionType)
        {
            SetTransactionInfoArgumentTickerSymbol = tickerSymbol;
            SetTransactionInfoArgumentTransactionType = transactionType;
            _mockOrderDetailsPresentationModel.TickerSymbol = tickerSymbol;

        }

        public event EventHandler CloseViewRequested;

        public IOrderCompositeView View
        {
            get { return _view; }
        }

        public ICommand SubmitCommand
        {
            get { return MockSubmitCommand; }
        }

        public ICommand CancelCommand
        {
            get { return MockCancelCommand; }
        }

        public IOrderDetailsPresentationModel OrderDetailsPresentationModel
        {
            get { return _mockOrderDetailsPresentationModel; }
        }

        internal void RaiseCloseViewRequested()
        {
            CloseViewRequested(this, EventArgs.Empty);
        }
    }

    internal class MockCommand : ICommand
    {
        public bool ExecuteCalled;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExecuteCalled = true;
        }
    }
}

