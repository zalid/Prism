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
using System.ComponentModel;
using StockTraderRI.Modules.Position.Models;
using StockTraderRI.Modules.Position.Tests.Mocks;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.Tests.Mocks;
using System.Windows.Input;
using StockTraderRI.Infrastructure.Interfaces;
using Prism.Commands;
using System.Xml;
using StockTraderRI.Modules.Position.PresentationModels;
using StockTraderRI.Modules.Position.Interfaces;

namespace StockTraderRI.Modules.Position.Tests.Orders
{
    [TestClass]
    public class OrderDetailsPresenterFixture
    {
        [TestMethod]
        public void PresenterProvidesViewModelToBindTo()
        {
            var view = new MockOrderDetailsView();
            using (var presenter = new OrderDetailsPresenter(view, null, null, new MockStockTraderRICommandProxy()))
            {
                Assert.IsNotNull(view.Model);
            }
        }

        [TestMethod]
        public void PresenterCreatesPublicSubmitCommand()
        {
            using (var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null))
            {
                Assert.IsNotNull(presenter.SubmitCommand);
            }
        }

        [TestMethod]
        public void CanExecuteChangedIsRaisedForSubmitCommandWhenModelChanges()
        {
            var view = new MockOrderDetailsView();
              bool canExecuteChanged = false;

            using (var presenter = new TestableOrderDetailsPresenter(view, null))
            {
                presenter.SubmitCommand.CanExecuteChanged += delegate { canExecuteChanged = true; };

                view.Model.Shares = 2;

                Assert.IsTrue(canExecuteChanged);
            }
        }

        [TestMethod]
        public void CannotSubmitWhenSharesIsNotPositive()
        {
            using (var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null))
            {
                var model = presenter.View.Model;

                model.Shares = 2;
                model.Shares = 2;
                Assert.IsTrue(presenter.SubmitCommand.CanExecute(null));

                model.Shares = 0;
                Assert.IsFalse(presenter.SubmitCommand.CanExecute(null));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubmitThrowsIfCanExecuteIsFalse()
        {
            using (var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null))
            {
                var model = presenter.View.Model;
                model.Shares = 0;
                Assert.IsFalse(presenter.SubmitCommand.CanExecute(null));

                presenter.SubmitCommand.Execute(null);
            }
        }

        [TestMethod]
        public void CancelRaisesCloseViewEvent()
        {
            bool closeViewRaised = false;

            using (var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null))
            {
                presenter.CloseViewRequested += delegate
                {
                    closeViewRaised = true;
                };

                presenter.CancelCommand.Execute(null);

                Assert.IsTrue(closeViewRaised);
            }
        }

        [TestMethod]
        public void SubmitRaisesCloseViewEvent()
        {
            bool closeViewRaised = false;

            using (var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null))
            {
                presenter.CloseViewRequested += delegate
                {
                    closeViewRaised = true;
                };

                presenter.Model.Shares = 1;
                Assert.IsTrue(presenter.SubmitCommand.CanExecute(null));
                presenter.SubmitCommand.Execute(null);

                Assert.IsTrue(closeViewRaised);
            }
        }

        [TestMethod]
        public void CannotSubmitOnSellWhenSharesIsHigherThanCurrentPosition()
        {
            var accountPositionService = new MockAccountPositionService();
            accountPositionService.AddPosition(new AccountPosition("TESTFUND", 10m, 15));
            using (var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), accountPositionService))
            {
                var model = presenter.View.Model;

                model.TickerSymbol = "TESTFUND";
                model.TransactionType = TransactionType.Sell;
                model.Shares = 5;
                Assert.IsTrue(presenter.SubmitCommand.CanExecute(null));

                model.Shares = 16;
                Assert.IsFalse(presenter.SubmitCommand.CanExecute(null));
            }
        }

        [TestMethod]
        public void PresenterCreatesCallSetOrderTypes()
        {
            var view = new MockOrderDetailsView();
            using (var presenter = new OrderDetailsPresenter(view, null, null, new MockStockTraderRICommandProxy()))
            {
                Assert.IsNotNull(view.Model.AvailableOrderTypes);
                Assert.IsTrue(view.Model.AvailableOrderTypes.Count > 0);
                Assert.AreEqual(Enum.GetValues(typeof(OrderType)).Length, view.Model.AvailableOrderTypes.Count);

            }
        }

        [TestMethod]
        public void PresenterCreatesCallSetTimeInForce()
        {
            var view = new MockOrderDetailsView();
            using (var presenter = new OrderDetailsPresenter(view, null, null, new MockStockTraderRICommandProxy()))
            {
                Assert.IsNotNull(view.Model.AvailableTimesInForce);
                Assert.IsTrue(view.Model.AvailableTimesInForce.Count > 0);
                Assert.AreEqual(Enum.GetValues(typeof(TimeInForce)).Length, view.Model.AvailableTimesInForce.Count);
            }
        }

        [TestMethod]
        public void SetTransactionInfoShouldUpdateTheModel()
        {
            var view = new MockOrderDetailsView();
            using (var presenter = new OrderDetailsPresenter(view, null, null, new MockStockTraderRICommandProxy()))
            {
                presenter.TransactionInfo = new TransactionInfo
                                                {TickerSymbol = "T000", TransactionType = TransactionType.Sell};

                Assert.AreEqual("T000", presenter.Model.TickerSymbol);
                Assert.AreEqual(TransactionType.Sell, presenter.Model.TransactionType);
            }
        }

        [TestMethod]
        public void PresenterUpdatesCommandsBasedOnActiveChangedOfView()
        {
            var view = new MockOrderDetailsView();
            using (var presenter = new TestableOrderDetailsPresenter(view, null))
            {
                view.IsActive = true;
                view.RaiseIsActiveChanged();

                Assert.IsTrue(presenter.CancelCommand.IsActive);

                view.IsActive = false;
                view.RaiseIsActiveChanged();

                Assert.IsFalse(presenter.CancelCommand.IsActive);

                view.IsActive = true;
                view.RaiseIsActiveChanged();

                Assert.IsTrue(presenter.CancelCommand.IsActive);

            }
        }

        [TestMethod]
        public void PresenterInitializesCommandsBasedOnActiveView()
        {
            var view = new MockOrderDetailsView();
            view.IsActive = true;

            using (var presenter = new TestableOrderDetailsPresenter(view, null))
            {
                Assert.IsTrue(presenter.CancelCommand.IsActive);
            }
        }

        [TestMethod]
        public void SubmitCallsServiceWithCorrectOrder()
        {
            var ordersService = new MockOrdersService();
            using (var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null, ordersService, new MockStockTraderRICommandProxy()))
            {
                presenter.Model.Shares = 2;
                presenter.Model.TickerSymbol = "AAAA";
                presenter.Model.TransactionType = TransactionType.Buy;
                presenter.Model.TimeInForce = TimeInForce.EndOfDay;
                presenter.Model.OrderType = OrderType.Limit;
                presenter.Model.StopLimitPrice = 15;

                Assert.IsNull(ordersService.SubmitArgumentOrder);
                presenter.SubmitCommand.Execute(null);

                var submittedOrder = ordersService.SubmitArgumentOrder;
                Assert.IsNotNull(submittedOrder);
                Assert.AreEqual("AAAA", submittedOrder.TickerSymbol);
                Assert.AreEqual(TransactionType.Buy, submittedOrder.TransactionType);
                Assert.AreEqual(TimeInForce.EndOfDay, submittedOrder.TimeInForce);
                Assert.AreEqual(OrderType.Limit, submittedOrder.OrderType);
                Assert.AreEqual(15, submittedOrder.StopLimitPrice);
            }
        }

        [TestMethod]
        public void VerifyTransactionInfoModificationsInOrderDetails()
        {
            var view = new MockOrderDetailsView();
            var orderDetailsPresenter = new OrderDetailsPresenter(view, null, null, new MockStockTraderRICommandProxy());
            var transactionInfo = new TransactionInfo { TickerSymbol = "Fund0", TransactionType = TransactionType.Buy };
            orderDetailsPresenter.TransactionInfo = transactionInfo;
            view.Model.TransactionType = TransactionType.Sell;
            Assert.AreEqual(TransactionType.Sell, transactionInfo.TransactionType);
            
            view.Model.TickerSymbol = "Fund1";
            Assert.AreEqual("Fund1", transactionInfo.TickerSymbol);
        }




        //[TestMethod]
        //public void DisposeUnregistersLocalCommandsFromGlobalCommands()
        //{
        //    var presenter = new TestableOrderDetailsPresenter(new MockOrderDetailsView(), null);
        //    Assert.IsTrue(StockTraderRICommands.SubmitOrderCommand.
        //}
	
    }

    internal class MockOrdersService : IOrdersService
    {
        public Order SubmitArgumentOrder;

        public void Submit(Order order)
        {
            SubmitArgumentOrder = order;
        }
    }

    class TestableOrderDetailsPresenter : OrderDetailsPresenter
    {
        public TestableOrderDetailsPresenter(IOrderDetailsView view, IAccountPositionService accountPositionService)
          :  this (view, accountPositionService, new MockOrdersService(), new MockStockTraderRICommandProxy() )
        {
        }

        public TestableOrderDetailsPresenter(IOrderDetailsView view, IAccountPositionService accountPositionService, IOrdersService ordersService, StockTraderRICommandProxy commandProxy)
            : base(view, accountPositionService, ordersService, commandProxy)
        {
            
        }

        public ActiveAwareDelegateCommand<string> SubmitCommand
        {
            get { return submitCommand; }
        }

        public ActiveAwareDelegateCommand<string> CancelCommand
        {
            get { return cancelCommand; }
        }

    }
}
