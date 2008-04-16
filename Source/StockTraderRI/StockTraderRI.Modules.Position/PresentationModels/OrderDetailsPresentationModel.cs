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
using System.Collections;
using System.Windows.Input;
using System.ComponentModel;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Infrastructure;

namespace StockTraderRI.Modules.Position.PresentationModels
{
    public class OrderDetailsPresentationModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public IList<ValueDescription<TimeInForce>> AvailableTimesInForce { get; set; }
        public IList<ValueDescription<OrderType>> AvailableOrderTypes { get; set; }

        private readonly IDictionary<string, string> errors = new Dictionary<string, string>();
        private int _shares;
        public TimeInForce TimeInForce { get; set; }
        private TransactionType _transactionType;
        public decimal StopLimitPrice { get; set; }

        private string _tickerSymbol;

        public string TickerSymbol
        {
            get { return _tickerSymbol; }
            set
            {
                if (_tickerSymbol != value)
                {
                    _tickerSymbol = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TickerSymbol"));
                    }
                }
            }
        }


        public int Shares
        {
            get { return _shares; }
            set
            {
                if (_shares != value)
                {
                    _shares = value;
                    OnPropertyChanged("Shares");
                }
            }
        }

        public TransactionType TransactionType
        {
            get { return _transactionType; }
            set
            {
                if (_transactionType != value)
                {
                    _transactionType = value; 
                    OnPropertyChanged("TransactionType");
                }
            }
        }

        private OrderType _orderType = OrderType.Market;

        public OrderType OrderType
        {
            get { return _orderType; }
            set
            {
                if (!value.Equals(_orderType))
                {
                    _orderType = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("OrderType"));
                    }
                }
            }
        }
        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region IDataErrorInfo Members

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                if (errors.ContainsKey(columnName))
                    return errors[columnName];

                return null;
            }
            set
            {
                errors[columnName] = value;
            }
        }

        #endregion

        internal void ClearErrors()
        {
            errors.Clear();
        }
    }
}
