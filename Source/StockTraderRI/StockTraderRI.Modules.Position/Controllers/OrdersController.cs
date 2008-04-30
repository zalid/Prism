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
using Microsoft.Practices.Unity;
using Prism.Interfaces;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.Interfaces;
using System.Windows;
using Prism.Commands;

namespace StockTraderRI.Modules.Position.Controllers
{
    public class OrdersController : IOrdersController
    {
        private IRegionManagerService _regionManager;
        private IUnityContainer _container;
        private IOrdersView _ordersView;

        private readonly string ORDERS_REGION = "OrdersRegion";

        public OrdersController(IRegionManagerService regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
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
                throw new ArgumentException("Ticker Symbol must be a string");
            }

            IRegion region = _regionManager.GetRegion("CollapsibleRegion");

            //Make Sure OrdersView is in CollapsibleRegion
            if (region.GetView("OrdersView") == null)
            {
                var ordersPresenter = _container.Resolve<IOrdersPresenter>();
                _ordersView = ordersPresenter.View;
                region.Add((UIElement)_ordersView, "OrdersView");
            }

            IRegion ordersRegion = _regionManager.GetRegion(ORDERS_REGION);
           
            var orderCompositePresenter = _container.Resolve<IOrderCompositePresenter>();
            orderCompositePresenter.SetTransactionInfo(tickerSymbol, transactionType);
            orderCompositePresenter.CloseViewRequested += delegate
            {
                ordersRegion.Remove((UIElement)orderCompositePresenter.View);
                IDisposable disposablePresenter = orderCompositePresenter as IDisposable;
                if (disposablePresenter != null)
                {
                    disposablePresenter.Dispose();
                }
            };

            ordersRegion.Add((UIElement)orderCompositePresenter.View);     
        }

        #region IOrdersController Members

        public DelegateCommand<string> BuyCommand { get; private set; }

        public DelegateCommand<string> SellCommand {get; private set; }

        #endregion
    }
}
