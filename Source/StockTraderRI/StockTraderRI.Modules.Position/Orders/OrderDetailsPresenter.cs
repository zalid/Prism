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
using System.ComponentModel;
using System.Globalization;
using Prism.Commands;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Models;
using StockTraderRI.Modules.Position.PresentationModels;
using StockTraderRI.Modules.Position.Properties;

namespace StockTraderRI.Modules.Position.Orders
{
    public class OrderDetailsPresenter : IOrderDetailsPresenter
    {
        private readonly IAccountPositionService accountPositionService;
        private readonly StockTraderRICommandProxy commandProxy;
        private readonly IOrdersService ordersService;
        private TransactionInfo _transactionInfo;
        protected ActiveAwareDelegateCommand<string> cancelCommand;
        protected ActiveAwareDelegateCommand<string> submitCommand;

        public OrderDetailsPresenter(IOrderDetailsView view, IAccountPositionService accountPositionService, IOrdersService ordersService, StockTraderRICommandProxy commandProxy)
        {
            View = view;
            this.accountPositionService = accountPositionService;
            this.ordersService = ordersService;
            this.commandProxy = commandProxy;

            Model = new OrderDetailsPresentationModel();
            //use localizable enum descriptions
            Model.AvailableOrderTypes = new List<ValueDescription<OrderType>>
                                        {
                                            new ValueDescription<OrderType>(OrderType.Limit, Resources.OrderType_Limit),
                                            new ValueDescription<OrderType>(OrderType.Market, Resources.OrderType_Market),
                                            new ValueDescription<OrderType>(OrderType.Stop, Resources.OrderType_Stop)
                                        };

            Model.AvailableTimesInForce = new List<ValueDescription<TimeInForce>>
                                          {
                                              new ValueDescription<TimeInForce>(TimeInForce.EndOfDay, Resources.TimeInForce_EndOfDay),
                                              new ValueDescription<TimeInForce>(TimeInForce.ThirtyDays, Resources.TimeInForce_ThirtyDays)
                                          };

            View.Model = Model;

            submitCommand = new ActiveAwareDelegateCommand<string>(Submit, CanSubmit);
            cancelCommand = new ActiveAwareDelegateCommand<string>(Cancel);
            submitCommand.IsActive = view.IsActive;
            cancelCommand.IsActive = view.IsActive;

            Model.PropertyChanged += Model_OnPropertyChangedEvent;
            view.IsActiveChanged += view_IsActiveChanged;

            // Register with buttons on other views
            commandProxy.SubmitAllOrdersCommand.RegisterCommand(submitCommand);
            commandProxy.CancelAllOrdersCommand.RegisterCommand(cancelCommand);
            commandProxy.SubmitOrderCommand.RegisterCommand(submitCommand);
            commandProxy.CancelOrderCommand.RegisterCommand(cancelCommand);

        }

        public OrderDetailsPresentationModel Model { get; set; }

        #region IOrderDetailsPresenter Members

        public event EventHandler CloseViewRequested = delegate { };
        public IOrderDetailsView View { get; set; }

        public TransactionInfo TransactionInfo
        {
            get { return _transactionInfo; }
            set
            {
                _transactionInfo = value;
                Model.TransactionType = _transactionInfo.TransactionType;
                Model.TickerSymbol = _transactionInfo.TickerSymbol;
            }
        }

        #endregion

        private void Model_OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            submitCommand.RaiseCanExecuteChanged();

            if (TransactionInfo != null)
            {
                if (e.PropertyName == "TickerSymbol")
                    TransactionInfo.TickerSymbol = Model.TickerSymbol;

                if (e.PropertyName == "TransactionType")
                    TransactionInfo.TransactionType = Model.TransactionType;
            }
        }

        void view_IsActiveChanged(object sender, EventArgs e)
        {
            submitCommand.IsActive = View.IsActive;
            cancelCommand.IsActive = View.IsActive;
        }

        bool CanSubmit(object parameter)
        {
            //Validate the model
            if (Model.Shares <= 0)
            {
                Model["Shares"] = Resources.InvalidSharesRange;
                return false;
            }

            if ((Model.TransactionType == TransactionType.Sell) && !HoldsEnoughShares(Model.TickerSymbol, Model.Shares))
            {
                Model["Shares"] = String.Format(CultureInfo.InvariantCulture, Resources.NotEnoughSharesToSell, Model.Shares);
                return false;
            }

            Model.ClearErrors();
            return true;
        }

        private bool HoldsEnoughShares(string symbol, long sharesToSell)
        {
            foreach (AccountPosition accountPosition in accountPositionService.GetAccountPositions())
            {
                if (accountPosition.TickerSymbol.Equals(symbol, StringComparison.OrdinalIgnoreCase))
                {
                    if (accountPosition.Shares >= sharesToSell)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        void Submit(object parameter)
        {
            if (!CanSubmit(parameter))
                throw new InvalidOperationException();

            OrderDetailsPresentationModel presentationModel = Model;
            var order = new Order()
                            {
                                TransactionType = presentationModel.TransactionType,
                                OrderType = presentationModel.OrderType,
                                Shares = presentationModel.Shares,
                                StopLimitPrice = presentationModel.StopLimitPrice,
                                TickerSymbol = presentationModel.TickerSymbol,
                                TimeInForce = presentationModel.TimeInForce
                            };
            ordersService.Submit(order);

            CloseViewRequested(this, EventArgs.Empty);
        }


        void Cancel(object parameter)
        {
            CloseViewRequested(this, EventArgs.Empty);
        }

        #region Disposable Pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                commandProxy.SubmitAllOrdersCommand.UnregisterCommand(submitCommand);
                commandProxy.CancelAllOrdersCommand.UnregisterCommand(cancelCommand);
                commandProxy.SubmitOrderCommand.UnregisterCommand(submitCommand);
                commandProxy.CancelOrderCommand.UnregisterCommand(cancelCommand);
            }
        }
        #endregion

    }
}
