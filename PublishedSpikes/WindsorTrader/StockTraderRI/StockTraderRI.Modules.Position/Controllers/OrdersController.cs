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
using System.Globalization;
using Castle.Windsor;
using Prism.Commands;
using Prism.Interfaces;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.Properties;

namespace StockTraderRI.Modules.Position.Controllers
{
    public class OrdersController : IOrdersController
    {
        private IRegionManager _regionManager;
        private IWindsorContainer _container;
        private readonly StockTraderRICommandProxy commandProxy;
        private IOrdersView _ordersView;

        private readonly string ORDERS_REGION = "OrdersRegion";

        public OrdersController(IRegionManager regionManager, IWindsorContainer container, StockTraderRICommandProxy commandProxy)
        {
            _regionManager = regionManager;
            _container = container;
            this.commandProxy = commandProxy;
            BuyCommand = new DelegateCommand<string>(OnBuyExecuted);
            SellCommand = new DelegateCommand<string>(OnSellExecuted);
        }

        void OnSellExecuted(string parameter)
        {
            StartOrder(parameter, TransactionType.Sell);
        }

        void OnBuyExecuted(string parameter)
        {
            StartOrder(parameter, TransactionType.Buy);
        }

        virtual protected void StartOrder(string tickerSymbol, TransactionType transactionType)
        {
            if (String.IsNullOrEmpty(tickerSymbol))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeNullOrEmpty, "tickerSymbol"));
            }

            IRegion region = _regionManager.Regions["MainRegion"];

            //Make Sure OrdersView is in CollapsibleRegion
            if (region.GetView("OrdersView") == null)
            {
                var ordersPresenter = _container.Resolve<IOrdersPresenter>();
                _ordersView = ordersPresenter.View;
                region.Add(_ordersView, "OrdersView");
                region.Activate(_ordersView);
            }

            IRegion ordersRegion = _regionManager.Regions[ORDERS_REGION];

            var orderCompositePresenter = _container.Resolve<IOrderCompositePresentationModel>();
            orderCompositePresenter.SetTransactionInfo(tickerSymbol, transactionType);
            orderCompositePresenter.CloseViewRequested += delegate
            {
                commandProxy.SubmitAllOrdersCommand.UnregisterCommand(orderCompositePresenter.SubmitCommand);
                commandProxy.CancelAllOrdersCommand.UnregisterCommand(orderCompositePresenter.CancelCommand);
                commandProxy.SubmitOrderCommand.UnregisterCommand(orderCompositePresenter.SubmitCommand);
                commandProxy.CancelOrderCommand.UnregisterCommand(orderCompositePresenter.CancelCommand);
                ordersRegion.Remove(orderCompositePresenter.View);
            };

            ordersRegion.Add(orderCompositePresenter.View);
            commandProxy.SubmitAllOrdersCommand.RegisterCommand(orderCompositePresenter.SubmitCommand);
            commandProxy.CancelAllOrdersCommand.RegisterCommand(orderCompositePresenter.CancelCommand);

            //The following commands are Active Aware
            commandProxy.SubmitOrderCommand.RegisterCommand(orderCompositePresenter.SubmitCommand);
            commandProxy.CancelOrderCommand.RegisterCommand(orderCompositePresenter.CancelCommand);

            ordersRegion.Activate(orderCompositePresenter.View);
        }

        #region IOrdersController Members

        public DelegateCommand<string> BuyCommand { get; private set; }

        public DelegateCommand<string> SellCommand { get; private set; }

        #endregion
    }
}
