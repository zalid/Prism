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

namespace StockTraderRI.Infrastructure
{

    public static class StockTraderRICommands
    {
        public static ActiveAwareCompositeCommand SubmitOrderCommand = new ActiveAwareCompositeCommand();
        public static ActiveAwareCompositeCommand CancelOrderCommand = new ActiveAwareCompositeCommand();
        public static CompositeCommand SubmitAllOrdersCommand = new CompositeCommand();
        public static CompositeCommand CancelAllOrdersCommand = new CompositeCommand();
    }

    public class StockTraderRICommandProxy
    {
        virtual public ActiveAwareCompositeCommand SubmitOrderCommand
        {
            get { return StockTraderRICommands.SubmitOrderCommand; }
        }

        virtual public ActiveAwareCompositeCommand CancelOrderCommand
        {
            get { return StockTraderRICommands.CancelOrderCommand; }
        }

        virtual public CompositeCommand SubmitAllOrdersCommand
        {
            get { return StockTraderRICommands.SubmitAllOrdersCommand; }
        }

        virtual public CompositeCommand CancelAllOrdersCommand
        {
            get { return StockTraderRICommands.CancelAllOrdersCommand; }
        }
    }
}
