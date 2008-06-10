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
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Utility;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.PresentationModels;

namespace StockTraderRI.Modules.Position.Orders
{
    public class OrderCompositePresentationModel : DependencyObject, IOrderCompositePresentationModel, IHeaderInfoProvider<string>
    {
        private readonly IOrderCompositeView _view;
        private readonly IOrderDetailsPresenter _orderDetailsPresenter;

        public static readonly DependencyProperty HeaderInfoProperty =
            DependencyProperty.Register("HeaderInfo", typeof(string), typeof(OrderCompositePresentationModel));

        public OrderCompositePresentationModel(IOrderCompositeView orderCompositeView, IOrderDetailsPresenter orderDetailsPresenter,
                                         IOrderCommandsView orderCommandsView)
        {
            _orderDetailsPresenter = orderDetailsPresenter;
            _orderDetailsPresenter.CloseViewRequested += _orderPresenter_CloseViewRequested;
            _view = orderCompositeView;
            orderCommandsView.Model = _orderDetailsPresenter.Model;
            this.OrderDetailsView = _orderDetailsPresenter.View;
            this.OrderCommandsView = orderCommandsView;
            _view.IsActiveChanged += compositeView_IsActiveChanged;
        }

        void compositeView_IsActiveChanged(object sender, EventArgs e)
        {
            _orderDetailsPresenter.View.IsActive = _view.IsActive;
        }

        void _orderPresenter_CloseViewRequested(object sender, EventArgs e)
        {
            OnCloseViewRequested(sender, e);
        }

        public void SetTransactionInfo(string tickerSymbol, TransactionType transactionType)
        {
            //This instance of TransactionInfo acts as a "shared model" between this view and the order details view.
            //The scenario says that these 2 views are decoupled, so they don't share the presentation model, they are only tied
            //with this TransactionInfo
            TransactionInfo transactionInfo = new TransactionInfo { TickerSymbol = tickerSymbol, TransactionType = transactionType };
            _orderDetailsPresenter.TransactionInfo = transactionInfo;

            //Bind the CompositeOrderView header to a string representation of the TransactionInfo shared instance (we expect the details presenter to modify it from user interaction).
            MultiBinding binding = new MultiBinding();
            binding.Bindings.Add(new Binding("TransactionType") { Source = transactionInfo });
            binding.Bindings.Add(new Binding("TickerSymbol") { Source = transactionInfo });
            binding.Converter = new OrderHeaderConverter();
            BindingOperations.SetBinding(this, OrderCompositePresentationModel.HeaderInfoProperty, binding);
            _view.Model = this;
        }

        protected virtual void OnCloseViewRequested(object sender, EventArgs e)
        {
            CloseViewRequested(sender, e);
        }

        public event EventHandler CloseViewRequested = delegate { };

        public IOrderCompositeView View
        {
            get
            {
                return _view;
            }
        }

        public ICommand SubmitCommand
        {
            get { return _orderDetailsPresenter.Model.SubmitCommand; }
        }

        public ICommand CancelCommand
        {
            get { return _orderDetailsPresenter.Model.CancelCommand; }
        }

        public string HeaderInfo
        {
            get { return (string)GetValue(HeaderInfoProperty); }
            set { SetValue(HeaderInfoProperty, value); }
        }

        
        public object OrderDetailsView { get; private set; }
        public object OrderCommandsView { get; private set; }


        public class OrderHeaderConverter : IMultiValueConverter
        {
            /// <summary>
            /// Converts a <see cref="TransactionType"/> and a ticker symbol to a header that can be placed on the TabItem header
            /// </summary>
            /// <param name="values">values[0] should be of type <see cref="TransactionType"/>. values[1] should be a string with the ticker symbol</param>
            /// <param name="targetType"></param>
            /// <param name="parameter"></param>
            /// <param name="culture"></param>
            /// <returns>Returns a human readable string with the transaction type and ticker symbol</returns>
            public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return values[0].ToString() + " " + values[1].ToString();
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    }
}