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

using Microsoft.Practices.Composite.Wpf.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Events
{
    [TestClass]
    public class SubscriptionTokenFixture
    {
        [TestMethod]
        public void EqualsShouldReturnFalseIfSubscriptionTokenPassedIsNull()
        {
            SubscriptionToken token = new SubscriptionToken();
            Assert.IsFalse(token.Equals(null));
        }

        [TestMethod]
        public void EqualsShouldReturnTrueWhenTokenIsTheSame()
        {
            SubscriptionToken token = new SubscriptionToken();
            Assert.IsTrue(token.Equals(token));
        }

        [TestMethod]
        public void EqualsShouldReturnTrueWhenComparingSameObjectInstances()
        {
            SubscriptionToken token = new SubscriptionToken();

            object tokenObject = token;

            Assert.IsTrue(token.Equals(tokenObject));
        }
    }
}
