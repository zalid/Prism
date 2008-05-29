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
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Position.PresentationModels;

namespace StockTraderRI.Modules.Position.Tests.Mocks
{
    class MockOrderDetailsPresenter : IOrderDetailsPresenter
    {
        public bool DisposeCalled = false;

        public MockOrderDetailsPresenter()
        {
            View = new MockOrderDetailsView();
        }

        public event EventHandler CloseViewRequested;

        public IOrderDetailsView View { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            DisposeCalled = true;
        }

        #endregion

        internal void RaiseCloseViewRequested()
        {
            CloseViewRequested(this, EventArgs.Empty);
        }

        public TransactionInfo TransactionInfo { get; set; }

        #region IOrderDetailsPresenter Members


        public OrderDetailsPresentationModel Model { get; set;}

        #endregion
    }
}
