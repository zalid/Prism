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
using System.Globalization;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Commands;
using Microsoft.Practices.Unity;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.Properties;
using System.Linq;

namespace StockTraderRI.Modules.Position.Controllers
{
    public class OrdersController : IOrdersController
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;
        private readonly StockTraderRICommandProxy commandProxy;
        private IOrdersView _ordersView;
        private IAccountPositionService _accountPositionService;

        private readonly string ORDERS_REGION = "OrdersRegion";

        public OrdersController(IRegionManager regionManager, IUnityContainer container, StockTraderRICommandProxy commandProxy, IAccountPositionService accountPositionService)
        {
            _regionManager = regionManager;
            _container = container;
            _accountPositionService = accountPositionService;
            this.commandProxy = commandProxy;
            BuyCommand = new DelegateCommand<string>(OnBuyExecuted);
            SellCommand = new DelegateCommand<string>(OnSellExecuted);
            SubmitAllCommand = new DelegateCommand<object>(null, SubmitAllCanExecute);
            OrderModels = new List<IOrderDetailsPresentationModel>();
            commandProxy.SubmitAllOrdersCommand.RegisterCommand(SubmitAllCommand);
            
        }

        void OnSellExecuted(string parameter)
        {
            StartOrder(parameter, TransactionType.Sell);
        }

        void OnBuyExecuted(string parameter)
        {
            StartOrder(parameter, TransactionType.Buy);
        }

        virtual protected bool SubmitAllCanExecute(object parameter)
        {
            bool canExecute = true;
            Dictionary<string,long> shareSums = new Dictionary<string, long>();
            foreach(var order in OrderModels)
            {
                if(!shareSums.ContainsKey(order.TickerSymbol))
                    shareSums.Add(order.TickerSymbol,0);

                if(order.Shares.HasValue)
                {
                    if(order.TransactionType == TransactionType.Buy)
                    {
                        shareSums[order.TickerSymbol] += order.Shares.Value;
                    }
                    else
                    {
                        shareSums[order.TickerSymbol] -= order.Shares.Value;
                    }
                }
                    
            }

            IList<AccountPosition> positions = _accountPositionService.GetAccountPositions();
            
            foreach(string key in shareSums.Keys)
            {
                if(shareSums[key] < 0) //More sell than buy order shares for this tickersymbol
                {
                    if ((positions.First(x=> x.TickerSymbol == key).Shares) < Math.Abs(shareSums[key]))
                    {
                        //trying to sell more shares than we own
                        canExecute = false;
                        break;
                    }
                }
            }

            return canExecute;
            
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
                var ordersPresentationModel = _container.Resolve<IOrdersPresentationModel>();
                _ordersView = ordersPresentationModel.View;
                region.Add(_ordersView, "OrdersView");
                region.Activate(_ordersView);
            }

            IRegion ordersRegion = _regionManager.Regions[ORDERS_REGION];

            var orderCompositePresentationModel = _container.Resolve<IOrderCompositePresentationModel>();
            orderCompositePresentationModel.SetTransactionInfo(tickerSymbol, transactionType);
            orderCompositePresentationModel.CloseViewRequested += delegate
            {
                commandProxy.SubmitAllOrdersCommand.UnregisterCommand(orderCompositePresentationModel.SubmitCommand);
                commandProxy.CancelAllOrdersCommand.UnregisterCommand(orderCompositePresentationModel.CancelCommand);
                commandProxy.SubmitOrderCommand.UnregisterCommand(orderCompositePresentationModel.SubmitCommand);
                commandProxy.CancelOrderCommand.UnregisterCommand(orderCompositePresentationModel.CancelCommand);
                ordersRegion.Remove(orderCompositePresentationModel.View);
                OrderModels.Remove(orderCompositePresentationModel.OrderDetailsPresentationModel);
            };

            ordersRegion.Add(orderCompositePresentationModel.View);
            OrderModels.Add(orderCompositePresentationModel.OrderDetailsPresentationModel);

            commandProxy.SubmitAllOrdersCommand.RegisterCommand(orderCompositePresentationModel.SubmitCommand);
            commandProxy.CancelAllOrdersCommand.RegisterCommand(orderCompositePresentationModel.CancelCommand);

            //The following commands are Active Aware
            commandProxy.SubmitOrderCommand.RegisterCommand(orderCompositePresentationModel.SubmitCommand);
            commandProxy.CancelOrderCommand.RegisterCommand(orderCompositePresentationModel.CancelCommand);

            ordersRegion.Activate(orderCompositePresentationModel.View);
        }

        #region IOrdersController Members

        public DelegateCommand<string> BuyCommand { get; private set; }
        public DelegateCommand<string> SellCommand { get; private set; }
        public DelegateCommand<object> SubmitAllCommand{ get; private set; }

        public List<IOrderDetailsPresentationModel> OrderModels { get; private set;}

        #endregion
    }
}
