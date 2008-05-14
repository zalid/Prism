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
using System.Windows.Controls;
using EventAggregation.Infrastructure;
using Prism.Events;
using Prism.Interfaces;
using Prism.Utility;

namespace ModuleA
{
    public class AddFundPresenter
    {
        private IEventAggregator _EventAggregator;

        public AddFundPresenter(IEventAggregator EventAggregator)
        {
            _EventAggregator = EventAggregator;
            
        }

        void AddFund(object sender, EventArgs e)
        {
            var fundOrder = new FundOrder();
            fundOrder.CustomerID = View.Customer;
            fundOrder.TickerSymbol = View.Fund;

            _EventAggregator.Get<FundAddedEvent>().Fire(fundOrder);
        }

        private IAddFundView _view;
        public IAddFundView View
        {
            get { return _view; }
            set
            {
                 _view = value;
                 _view.AddFund += AddFund;
            }
        }
        
    }
}
