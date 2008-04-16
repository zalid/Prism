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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.PresentationModels;
using StockTraderRI.Modules.Position.Tests.Mocks;
using Microsoft.Practices.Unity;
using StockTraderRI.Infrastructure;

namespace StockTraderRI.Modules.Position.Tests.Orders
{
    /// <summary>
    /// Summary description for OrderCompositePresenterFixture
    /// </summary>
    [TestClass]
    public class OrderCompositePresenterFixture
    {
        [TestMethod]
        public void ShouldCreateOrderDetailsPresenter()
        {
            var detailsPresenter = new MockOrderDetailsPresenter();
            IOrderCompositeView compositeView = new MockOrderCompositeView();

            var composite = new OrderCompositePresenter(compositeView, detailsPresenter, new OrderCommandsView());

            composite.SetTransactionInfo("FXX01", TransactionType.Sell);

            Assert.IsNotNull(detailsPresenter.TransactionInfo);
        }

        [TestMethod]
        public void ShouldAddDetailsViewANDControlsViewToContentArea()
        {
            MockOrderCompositeView compositeView = new MockOrderCompositeView();
            var detailsPresenter = new MockOrderDetailsPresenter();

            var composite = new OrderCompositePresenter(compositeView, detailsPresenter, new OrderCommandsView());

            Assert.AreEqual(detailsPresenter.View, compositeView.DetailView);
            Assert.IsNotNull(compositeView.CommandView as OrderCommandsView);
        }

        [TestMethod]
        public void PresenterExposesChildOrderPresentersCloseRequested()
        {
            var detailsPresenter = new MockOrderDetailsPresenter();
            MockOrderCompositeView compositeView = new MockOrderCompositeView();

            var composite = new OrderCompositePresenter(compositeView, detailsPresenter, new OrderCommandsView());

            var closeViewRequestedFired = false;
            composite.CloseViewRequested += delegate
                                                {
                                                    closeViewRequestedFired = true;
                                                };

            detailsPresenter.RaiseCloseViewRequested();

            Assert.IsTrue(closeViewRequestedFired);

        }

        [TestMethod]
        public void ShouldDelegateIsActivePropertyChangesToDetailView()
        {
            var detailsPresenter = new MockOrderDetailsPresenter();
            MockOrderCompositeView compositeView = new MockOrderCompositeView();

            var composite = new OrderCompositePresenter(compositeView, detailsPresenter, new OrderCommandsView());
            compositeView.IsActive = false;

            Assert.IsFalse(detailsPresenter.View.IsActive);
            compositeView.IsActive = true;
            Assert.IsTrue(detailsPresenter.View.IsActive);
        }

        [TestMethod]
        public void ShouldAddModelWithHeaderInfo()
        {
            var view = new MockOrderCompositeView();

            var composite = new OrderCompositePresenter(view, new MockOrderDetailsPresenter(), new OrderCommandsView());

            composite.SetTransactionInfo("FXX01", TransactionType.Sell);

            Assert.IsNotNull(view.Model);
            Assert.IsNotNull(view.Model.HeaderInfo);
            Assert.IsTrue(view.Model.HeaderInfo.Contains("FXX01"));
            Assert.IsTrue(view.Model.HeaderInfo.Contains("Sell"));
            Assert.AreEqual("Sell FXX01", view.Model.HeaderInfo);
        }

        [TestMethod]
        public void ShouldUpdateHeaderInfoWhenUpdatingTransactionInfo()
        {
            var presenter = new MockOrderDetailsPresenter();
            var view = new MockOrderCompositeView();

            var composite = new OrderCompositePresenter(view, presenter, new OrderCommandsView());

            composite.SetTransactionInfo("FXX01", TransactionType.Sell);

            presenter.TransactionInfo.TickerSymbol = "NEW_SYMBOL";
            Assert.AreEqual("Sell NEW_SYMBOL", view.Model.HeaderInfo);

            presenter.TransactionInfo.TransactionType = TransactionType.Buy;
            Assert.AreEqual("Buy NEW_SYMBOL", view.Model.HeaderInfo);
        }


    }

    internal class MockOrderCompositeView : UIElement, IOrderCompositeView
    {
        private bool _IsActive = false;
        public UIElement DetailView { get; private set; }

        public UIElement CommandView { get; private set; }

        public void SetDetailView(UIElement detailView)
        {
            DetailView = detailView;
        }

        public void SetCommandView(UIElement commandView)
        {
            CommandView = commandView;
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                _IsActive = value;
                IsActiveChanged(this, EventArgs.Empty);

            }
        }

        public event EventHandler IsActiveChanged;

        public OrderCompositePresentationModel Model { get; set; }
    }
}
