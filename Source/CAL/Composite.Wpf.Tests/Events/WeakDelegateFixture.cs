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
using Microsoft.Practices.Composite.Wpf.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Events
{
    [TestClass]
    public class WeakDelegateFixture
    {
        [TestMethod]
        public void TargetShouldReturnAction()
        {
            string something = null;
            Action<string> myAction = (arg => something = arg);

            var weakAction = new WeakDelegate<Action<string>>(myAction);

            weakAction.Target("payload");
            Assert.AreEqual("payload", something);
        }

        [TestMethod]
        public void ShouldAllowCollectionOfOriginalDelegate()
        {
            string something = null;
            Action<string> myAction = (arg => something = arg);

            var weakAction = new WeakDelegate<Action<string>>(myAction);

            var originalAction = new WeakReference(myAction);
            myAction = null;
            GC.Collect();
            Assert.IsFalse(originalAction.IsAlive);

            weakAction.Target("payload");
            Assert.AreEqual("payload", something);
        }

        [TestMethod]
        public void ShouldReturnNullIfTargetNotAlive()
        {
            SomeClassHandler handler = new SomeClassHandler();
            var weakHandlerRef = new WeakReference(handler);

            var action = new WeakDelegate<Action<string>>(handler.DoEvent);

            handler = null;
            GC.Collect();
            Assert.IsFalse(weakHandlerRef.IsAlive);

            Assert.IsNull(action.Target);
        }

        [TestMethod]
        public void WeakDelegateWorksWithStaticMethodDelegates()
        {
            var action = new WeakDelegate<Action>(SomeClassHandler.StaticMethod);

            Assert.IsNotNull(action.Target);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullDelegateThrows()
        {
            var action = new WeakDelegate<Action>(null);
        }

        class SomeClassHandler
        {
            public void DoEvent(string value)
            {
                string myValue = value;
            }

            public static void StaticMethod()
            {
                int i = 0;
            }
        }

    }
}