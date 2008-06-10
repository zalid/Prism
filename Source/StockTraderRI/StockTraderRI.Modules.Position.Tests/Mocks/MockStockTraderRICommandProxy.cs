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
using Microsoft.Practices.Composite.Wpf.Commands;
using StockTraderRI.Infrastructure;

namespace StockTraderRI.Modules.Position.Tests.Mocks
{
    public class MockStockTraderRICommandProxy : StockTraderRICommandProxy
    {
        CompositeCommand _submitAllOrdersCommand = new CompositeCommand();
        CompositeCommand _cancelAllOrdersCommand = new CompositeCommand();
        ActiveAwareCompositeCommand _submitOrderCommand = new ActiveAwareCompositeCommand();
        ActiveAwareCompositeCommand _cancelOrderCommand = new ActiveAwareCompositeCommand();

        public override ActiveAwareCompositeCommand SubmitOrderCommand
        {
            get
            {
                return this._submitOrderCommand;
            }
        }

        public override CompositeCommand SubmitAllOrdersCommand
        {
            get
            {
                return this._submitAllOrdersCommand;
            }
        }
        public override ActiveAwareCompositeCommand CancelOrderCommand
        {
            get
            {
                return this._cancelOrderCommand;
            }
        }

        public override CompositeCommand CancelAllOrdersCommand
        {
            get
            {
                return this._cancelAllOrdersCommand;
            }
        }
    }
}
