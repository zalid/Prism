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
using System.Threading;
using System.Windows.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Prism.Interfaces;

namespace Prism.Tests.Events
{
    [TestClass]
    public class EventFixture
    {
        [TestMethod]
        public void CanSubscribeAndRaiseEvent()
        {
            TestablePrismEvent<string> prismEvent = new TestablePrismEvent<string>();
            bool published = false;
            prismEvent.Subscribe(delegate { published = true; }, ThreadOption.PublisherThread, true, delegate { return true; });
            prismEvent.Publish(null);

            Assert.IsTrue(published);
        }

        [TestMethod]
        public void CanSubscribeAndRaiseCustomEvent()
        {
            CustomEvent customEvent = new CustomEvent();
            Payload payload = new Payload();
            Payload received = null;
            customEvent.Subscribe(delegate(Payload args) { received = args; });

            customEvent.Publish(payload);

            Assert.AreSame(received, payload);
        }

        [TestMethod]
        public void CanHaveMultipleSubscribersAndRaiseCustomEvent()
        {
            CustomEvent customEvent = new CustomEvent();
            Payload payload = new Payload();
            Payload received1 = null;
            Payload received2 = null;
            customEvent.Subscribe(delegate(Payload args) { received1 = args; });
            customEvent.Subscribe(delegate(Payload args) { received2 = args; });

            customEvent.Publish(payload);

            Assert.AreSame(received1, payload);
            Assert.AreSame(received2, payload);
        }

        [TestMethod]
        public void SubscriberReceivesNotificationOnDispatcherThread()
        {
            TestablePrismEvent<string> prismEvent = new TestablePrismEvent<string>();
            int threadId = -1;
            int calledThreadId = -1;
            ManualResetEvent setupEvent = new ManualResetEvent(false);
            bool completed = false;
            Thread mockUIThread = new Thread(delegate()
                                                 {
                                                     threadId = Thread.CurrentThread.ManagedThreadId;
                                                     prismEvent.SettableUIDispatcher = Dispatcher.CurrentDispatcher;
                                                     setupEvent.Set();

                                                     while (!completed)
                                                     {
                                                         WPFThreadHelper.DoEvents();
                                                     }
                                                 }
                                                 );
            mockUIThread.Start();
            string receivedPayload = null;
            prismEvent.Subscribe(delegate(string args)
            {
                calledThreadId = Thread.CurrentThread.ManagedThreadId;
                receivedPayload = args;
                completed = true;
            }, ThreadOption.UIThread);

            setupEvent.WaitOne();
            prismEvent.Publish("Test Payload");

            bool joined = mockUIThread.Join(5000);
            Assert.IsTrue(joined);

            Assert.AreEqual(threadId, calledThreadId);
            Assert.AreSame("Test Payload", receivedPayload);
        }



        [TestMethod]
        public void SubscriberReceivesNotificationOnDifferentThread()
        {
            TestablePrismEvent<string> prismEvent = new TestablePrismEvent<string>();
            int calledThreadId = -1;
            ManualResetEvent completeEvent = new ManualResetEvent(false);
            prismEvent.Subscribe(delegate
            {
                calledThreadId = Thread.CurrentThread.ManagedThreadId;
                completeEvent.Set();
            }, ThreadOption.BackgroundThread);

            prismEvent.Publish(null);
            completeEvent.WaitOne();
            Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, calledThreadId);
        }

        [TestMethod]
        public void PayloadGetPassedInBackgroundHandler()
        {
            CustomEvent customEvent = new CustomEvent();
            Payload payload = new Payload();
            ManualResetEvent backgroundWait = new ManualResetEvent(false);

            Payload backGroundThreadReceived = null;
            customEvent.Subscribe(delegate(Payload passedPayload)
            {
                backGroundThreadReceived = passedPayload;
                backgroundWait.
                    Set();
            }, ThreadOption.BackgroundThread);

            customEvent.Publish(payload);
            backgroundWait.WaitOne();
            Assert.AreSame(backGroundThreadReceived, payload);
        }

        [TestMethod]
        public void SubscribeTakesExecuteDelegateThreadOptionAndFilter()
        {
            TestablePrismEvent<string> prismEvent = new TestablePrismEvent<string>();
            string receivedValue = null;
            prismEvent.Subscribe(delegate(string value) { receivedValue = value; });

            prismEvent.Publish("test");

            Assert.AreEqual("test", receivedValue);

        }

        [TestMethod]
        public void FilterEnablesActionTarget()
        {
            TestablePrismEvent<string> prismEvent = new TestablePrismEvent<string>();
            bool goodFilterPublished = false;
            bool badFilterPublished = false;
            prismEvent.Subscribe(delegate { goodFilterPublished = true; }, ThreadOption.PublisherThread, true, delegate { return true; });
            prismEvent.Subscribe(delegate { badFilterPublished = true; }, ThreadOption.PublisherThread, true, delegate { return false; });

            prismEvent.Publish("test");

            Assert.IsTrue(goodFilterPublished);
            Assert.IsFalse(badFilterPublished);

        }

        [TestMethod]
        public void SubscribeDefaultsThreadOptionAndNoFilter()
        {
            TestablePrismEvent<string> prismEvent = new TestablePrismEvent<string>();
            int calledThreadID = -1;

            prismEvent.Subscribe(delegate { calledThreadID = Thread.CurrentThread.ManagedThreadId; });

            prismEvent.Publish("test");

            Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, calledThreadID);
        }

        [TestMethod]
        public void ShouldUnsubscribeFromPublisherThread()
        {
            var prismEvent = new TestablePrismEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            prismEvent.Subscribe(
                    actionEvent,
                    ThreadOption.PublisherThread);

            Assert.IsTrue(prismEvent.Contains(actionEvent));
            prismEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(prismEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void UnsubscribeShouldNotFailWithNonSubscriber()
        {
            TestablePrismEvent<string> prismEvent = new TestablePrismEvent<string>();

            Action<string> subscriber = delegate { };
            prismEvent.Unsubscribe(subscriber);
        }

        [TestMethod]
        public void ShouldUnsubscribeFromBackgroundThread()
        {
            var prismEvent = new TestablePrismEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            prismEvent.Subscribe(
                    actionEvent,
                    ThreadOption.BackgroundThread);

            Assert.IsTrue(prismEvent.Contains(actionEvent));
            prismEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(prismEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void ShouldUnsubscribeFromUIThread()
        {
            var prismEvent = new TestablePrismEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            prismEvent.Subscribe(
                    actionEvent,
                    ThreadOption.UIThread);

            Assert.IsTrue(prismEvent.Contains(actionEvent));
            prismEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(prismEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void ShouldUnsubscribeASingleDelegate()
        {
            var prismEvent = new TestablePrismEvent<string>();

            int callCount = 0;

            Action<string> actionEvent = delegate { callCount++; };
            prismEvent.Subscribe(actionEvent);
            prismEvent.Subscribe(actionEvent);

            prismEvent.Publish(null);
            Assert.AreEqual<int>(2, callCount);

            callCount = 0;
            prismEvent.Unsubscribe(actionEvent);
            prismEvent.Publish(null);
            Assert.AreEqual<int>(1, callCount);
        }

        [TestMethod]
        public void ShouldNotExecuteOnGarbageCollectedDelegateReferenceWhenNotKeepAlive()
        {
            var prismEvent = new TestablePrismEvent<string>();

            ExternalAction externalAction = new ExternalAction();
            prismEvent.Subscribe(externalAction.ExecuteAction);

            prismEvent.Publish("testPayload");
            Assert.AreEqual("testPayload", externalAction.PassedValue);

            WeakReference actionEventReference = new WeakReference(externalAction);
            externalAction = null;
            GC.Collect();
            Assert.IsFalse(actionEventReference.IsAlive);

            prismEvent.Publish("testPayload");
        }

        [TestMethod]
        public void ShouldNotExecuteOnGarbageCollectedFilterReferenceWhenNotKeepAlive()
        {
            var prismEvent = new TestablePrismEvent<string>();

            bool wasCalled = false;
            Action<string> actionEvent = delegate { wasCalled = true; };

            ExternalFilter filter = new ExternalFilter();
            prismEvent.Subscribe(actionEvent, ThreadOption.PublisherThread, false, filter.AlwaysTrueFilter);

            prismEvent.Publish("testPayload");
            Assert.IsTrue(wasCalled);

            wasCalled = false;
            WeakReference filterReference = new WeakReference(filter);
            filter = null;
            GC.Collect();
            Assert.IsFalse(filterReference.IsAlive);

            prismEvent.Publish("testPayload");
            Assert.IsFalse(wasCalled);
        }

        [TestMethod]
        public void CanAddDescriptionWhileEventIsFiring()
        {
            TestablePrismEvent<string> prismEvent = new TestablePrismEvent<string>();
            Action<string> emptyDelegate = delegate { };
            prismEvent.Subscribe(delegate { prismEvent.Subscribe(emptyDelegate); });

            Assert.IsFalse(prismEvent.Contains(emptyDelegate));

            prismEvent.Publish(null);

            Assert.IsTrue((prismEvent.Contains(emptyDelegate)));
        }

        [TestMethod]
        public void InlineDelegateDeclarationsDoesNotGetCollectedIncorrectlyWithWeakReferences()
        {
            TestablePrismEvent<string> prismEvent = new TestablePrismEvent<string>();
            bool published = false;
            prismEvent.Subscribe(delegate { published = true; }, ThreadOption.PublisherThread, false, delegate { return true; });
            GC.Collect();
            prismEvent.Publish(null);

            Assert.IsTrue(published);
        }

        [TestMethod]
        public void ShouldNotGarbageCollectDelegateReferenceWhenUsingKeepAlive()
        {
            var prismEvent = new TestablePrismEvent<string>();

            var externalAction = new ExternalAction();
            prismEvent.Subscribe(externalAction.ExecuteAction, ThreadOption.PublisherThread, true);

            WeakReference actionEventReference = new WeakReference(externalAction);
            externalAction = null;
            GC.Collect();
            GC.Collect();
            Assert.IsTrue(actionEventReference.IsAlive);

            prismEvent.Publish("testPayload");

            Assert.AreEqual("testPayload", ((ExternalAction)actionEventReference.Target).PassedValue);
        }

        [TestMethod]
        public void RegisterReturnsTokenThatCanBeUsedToUnsubscribe()
        {
            var prismEvent = new TestablePrismEvent<string>();
            Action<string> action = delegate { };

            var token = prismEvent.Subscribe(action);
            prismEvent.Unsubscribe(token);

            Assert.IsFalse(prismEvent.Contains(action));
        }

        [TestMethod]
        public void ContainsShouldSearchByToken()
        {
            var prismEvent = new TestablePrismEvent<string>();
            Action<string> action = delegate { };
            var token = prismEvent.Subscribe(action);

            Assert.IsTrue(prismEvent.Contains(token));

            prismEvent.Unsubscribe(action);
            Assert.IsFalse(prismEvent.Contains(token));
        }


    }

    class ExternalFilter
    {
        public bool AlwaysTrueFilter(string value)
        {
            return true;
        }
    }

    class ExternalAction
    {
        public string PassedValue;
        public void ExecuteAction(string value)
        {
            PassedValue = value;
        }
    }



    class TestablePrismEvent<TPayload> : PrismEvent<TPayload>
    {
        private Dispatcher _uiDispatcher;
        protected override Dispatcher UIDispatcher
        {
            get
            {
                if (_uiDispatcher == null)
                    return Dispatcher.CurrentDispatcher;
                return _uiDispatcher;
            }
        }

        public Dispatcher SettableUIDispatcher
        {
            set { _uiDispatcher = value; }
        }
    }

    class CustomEvent : TestablePrismEvent<Payload> { }
    class Payload { }
}
