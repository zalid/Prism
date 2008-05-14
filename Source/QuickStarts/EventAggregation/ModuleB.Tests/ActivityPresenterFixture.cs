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
using EventAggregation.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModuleB.Tests.Mocks;
using Prism.Events;

namespace ModuleB.Tests
{
    [TestClass]
    public class ActivityPresenterFixture
    {
        [TestMethod]
        public void PresenterAddsFundOnEvent()
        {
            var view = new MockActivityView();
            var mockEventAggregator = new MockEventAggregator();

            MockFundAddedEvent mockEvent = new MockFundAddedEvent();
            mockEventAggregator.AddMapping<FundAddedEvent>(mockEvent);

            ActivityPresenter presenter = new ActivityPresenter(mockEventAggregator);
            presenter.View = view;
            string customerId = "ALFKI";
            presenter.CustomerID = customerId;
            FundOrder payload = new FundOrder() { CustomerID = customerId, TickerSymbol = "MSFT" };
            mockEvent.SubscribeArgumentAction(payload);
            StringAssert.Contains(view.AddContentArgumentContent, "MSFT");
        }

        [TestMethod]
        public void PresenterSubscribesToFundAddedForCorrectCustomer()
        {
            var mockEventAggregator = new MockEventAggregator();

            MockFundAddedEvent mockEvent = new MockFundAddedEvent();
            mockEventAggregator.AddMapping<FundAddedEvent>(mockEvent);
            ActivityPresenter presenter = new ActivityPresenter(mockEventAggregator);
            presenter.View = new MockActivityView();

            presenter.CustomerID = "ALFKI";

            Assert.IsTrue(mockEvent.SubscribeArgumentFilter(new FundOrder { CustomerID = "ALFKI" }));
            Assert.IsFalse(mockEvent.SubscribeArgumentFilter(new FundOrder { CustomerID = "FilteredOutCustomer" }));
        }


    }

    internal class MockFundAddedEvent : FundAddedEvent
    {
        public Action<FundOrder> SubscribeArgumentAction;
        public Predicate<FundOrder> SubscribeArgumentFilter;
        public override SubscriptionToken Subscribe(Action<FundOrder> action, Prism.Interfaces.ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<FundOrder> filter)
        {
            SubscribeArgumentAction = action;
            SubscribeArgumentFilter = filter;
            return null;
        }
    }

    class MockActivityView : IActivityView
    {
        public string AddContentArgumentContent;
        public void AddContent(string content)
        {
            AddContentArgumentContent = content;
        }

        public string Title
        {
            set { }
        }

        public string CustomerId
        {
            set { }
        }
    }
}
