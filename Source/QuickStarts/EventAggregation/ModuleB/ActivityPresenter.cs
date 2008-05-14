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

using System.Diagnostics;
using EventAggregation.Infrastructure;
using Prism.Interfaces;

namespace ModuleB
{
    public class ActivityPresenter
    {
        private string _customerID;
        private IEventAggregator _EventAggregator;

        public ActivityPresenter(IEventAggregator EventAggregator)
        {
            _EventAggregator = EventAggregator;
        }

        void FundAddedEventHandler(FundOrder fundOrder)
        {
            Debug.Assert(View != null);
            View.AddContent(fundOrder.TickerSymbol);
        }

        public IActivityView View { get; set; }

        public string CustomerID
        {
            get { return _customerID; }
            set
            {
                _customerID = value;
                _EventAggregator.Get<FundAddedEvent>().Subscribe(FundAddedEventHandler, ThreadOption.PublisherThread, false, fundOrder => fundOrder.CustomerID == _customerID);

                View.Title = string.Format("Activity for: {0}", CustomerID);
            }
        }
    }
}
