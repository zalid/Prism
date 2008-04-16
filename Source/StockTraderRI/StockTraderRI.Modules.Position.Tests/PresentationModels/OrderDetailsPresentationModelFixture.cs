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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockTraderRI.Modules.Position.PresentationModels;
using System.ComponentModel;

namespace StockTraderRI.Modules.Position.Tests.PresentationModels
{
    [TestClass]
    public class OrderDetailsPresentationModelFixture
    {
        [TestMethod]
        public void PropertyChangedIsRaisedWhenSharesIsChanged()
        {
            var model = new OrderDetailsPresentationModel();
            model.Shares = 5;

            bool sharesPropertyChangedRaised = false;
            model.PropertyChanged += delegate(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "Shares")
                    sharesPropertyChangedRaised = true;
            };
            model.Shares = 0;
            Assert.IsTrue(sharesPropertyChangedRaised);
        }
    }
}
