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
using System.ComponentModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FamilyShow.Infrastructure;
using FamilyShow.MapModule;
using Microsoft.FamilyShowLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Prism.Interfaces;

namespace FamilyShow.HistoricalFactModule.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SubsribesToPersonContextChangedEvent()
        {
            var eventAggregator = new MockPersonContextEventAggregator();
            var presenter = new FactsByBirthYearPersonContentPresenter(eventAggregator);
            string changedProperty = string.Empty;
            presenter.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
                                             {
                                                 changedProperty = args.PropertyName;
                                             };

            Person person = new Person("firstname", "lastname", Gender.Male);
            person.BirthDate = new DateTime(1967, 1, 1);
            eventAggregator.PersonContextChangedEvent.Fire(person);

            Assert.AreEqual(new Uri("http://wapedia.mobi/en/1967"), presenter.URL);
            Assert.AreEqual("URL", changedProperty);
        }
    }


    internal class MockPersonContextEventAggregator : IEventAggregator
    {
        public MockPersonContextChangedEvent PersonContextChangedEvent = new MockPersonContextChangedEvent();

        public TEventType Get<TEventType>() where TEventType : class, new()
        {
            return this.PersonContextChangedEvent as TEventType;
        }
    }

    internal class MockPersonContextChangedEvent : PersonContextChangedEvent
    {
        private Action<Person> action;
        public override void Fire(Person payload)
        {
            action(payload);
        }

        public override SubscriptionToken Subscribe(Action<Person> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<Person> filter)
        {
            this.action = action;
            return null;
        }
    }
}
