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
using System.Windows.Controls;
using FamilyShow.Infrastructure;
using Microsoft.FamilyShow;
using Microsoft.FamilyShowLib;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;

namespace FamilyShow.Tests
{
    [TestClass]
    public class PersonContentControllerFixture
    {
        public PersonContentControllerFixture()
        {
        }

        [TestMethod]
        public void ControllerSitesViewInItemsControl()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IPersonContentView, MockPersonContentView>("TestView");
            var itemsControl = new ItemsControl();
            var controller = new PersonContentController(container, new MockEventAggregator());
            controller.Attach(itemsControl);

            Assert.AreEqual<int>(1, itemsControl.Items.Count);
            Assert.IsInstanceOfType(itemsControl.Items[0], typeof(MockPersonContentView) );
        }

        [TestMethod]
        public void ControllerFiresPersonContextChangedEvent()
        {
            IUnityContainer container = new UnityContainer();

            var eventAggregator = new MockEventAggregator();
            var view = new MockPersonContentView();
            container.RegisterInstance<IPersonContentView>("TestView", view);

            var itemsControl = new ItemsControl();
            var controller = new PersonContentController(container, eventAggregator);
            controller.Attach(itemsControl);

            var person = new Person("John", "Smith");
            controller.SetPersonContext(person);

            Assert.IsTrue(eventAggregator.PersonContextChangedEvent.WasFired);
        }
    }

    class MockEventAggregator : IEventAggregator
    {
        public MockPersonContextChangedEvent PersonContextChangedEvent = new MockPersonContextChangedEvent();

        public TEventType Get<TEventType>() where TEventType : class, new()
        {
            return PersonContextChangedEvent as TEventType;
        }

        public class MockPersonContextChangedEvent : PersonContextChangedEvent
        {
            public bool WasFired = false;

            public override void Fire(Person payload)
            {
                WasFired = true;
            }
        }
    }

    class MockPersonContentView : IPersonContentView
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}