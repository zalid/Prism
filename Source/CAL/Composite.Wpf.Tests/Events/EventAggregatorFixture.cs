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
using Microsoft.Practices.Composite.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Events
{
    [TestClass]
    public class EventAggregatorFixture
    {
        [TestMethod]
        public void GetReturnsSingleInstancesOfEBEventType()
        {
            EventAggregator eventAggregator = new EventAggregator();
            TestEBEvent instance1 = eventAggregator.GetInstance<TestEBEvent>();
            TestEBEvent instance2 = eventAggregator.GetInstance<TestEBEvent>();

            Assert.AreSame(instance2, instance1);
        }
    }

    class TestEBEvent{}
}