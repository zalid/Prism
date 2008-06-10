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
using Microsoft.Practices.Composite.Wpf.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Events
{
    [TestClass]
    public class EventFixture
    {
        [TestMethod]
        public void CanSubscribeAndRaiseEvent()
        {
            TestableWpfEvent<string> wpfEvent = new TestableWpfEvent<string>();
            bool published = false;
            wpfEvent.Subscribe(delegate { published = true; }, ThreadOption.PublisherThread, true, delegate { return true; });
            wpfEvent.Publish(null);

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
            TestableWpfEvent<string> wpfEvent = new TestableWpfEvent<string>();
            int threadId = -1;
            int calledThreadId = -1;
            ManualResetEvent setupEvent = new ManualResetEvent(false);
            bool completed = false;
            Thread mockUIThread = new Thread(delegate()
                                                 {
                                                     threadId = Thread.CurrentThread.ManagedThreadId;
                                                     wpfEvent.SettableUIDispatcher = Dispatcher.CurrentDispatcher;
                                                     setupEvent.Set();

                                                     while (!completed)
                                                     {
                                                         WPFThreadHelper.DoEvents();
                                                     }
                                                 }
                );
            mockUIThread.Start();
            string receivedPayload = null;
            wpfEvent.Subscribe(delegate(string args)
                                     {
                                         calledThreadId = Thread.CurrentThread.ManagedThreadId;
                                         receivedPayload = args;
                                         completed = true;
                                     }, ThreadOption.UIThread);

            setupEvent.WaitOne();
            wpfEvent.Publish("Test Payload");

            bool joined = mockUIThread.Join(5000);
            Assert.IsTrue(joined);

            Assert.AreEqual(threadId, calledThreadId);
            Assert.AreSame("Test Payload", receivedPayload);
        }



        [TestMethod]
        public void SubscriberReceivesNotificationOnDifferentThread()
        {
            TestableWpfEvent<string> wpfEvent = new TestableWpfEvent<string>();
            int calledThreadId = -1;
            ManualResetEvent completeEvent = new ManualResetEvent(false);
            wpfEvent.Subscribe(delegate
                                     {
                                         calledThreadId = Thread.CurrentThread.ManagedThreadId;
                                         completeEvent.Set();
                                     }, ThreadOption.BackgroundThread);

            wpfEvent.Publish(null);
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
            TestableWpfEvent<string> wpfEvent = new TestableWpfEvent<string>();
            string receivedValue = null;
            wpfEvent.Subscribe(delegate(string value) { receivedValue = value; });

            wpfEvent.Publish("test");

            Assert.AreEqual("test", receivedValue);

        }

        [TestMethod]
        public void FilterEnablesActionTarget()
        {
            TestableWpfEvent<string> wpfEvent = new TestableWpfEvent<string>();
            bool goodFilterPublished = false;
            bool badFilterPublished = false;
            wpfEvent.Subscribe(delegate { goodFilterPublished = true; }, ThreadOption.PublisherThread, true, delegate { return true; });
            wpfEvent.Subscribe(delegate { badFilterPublished = true; }, ThreadOption.PublisherThread, true, delegate { return false; });

            wpfEvent.Publish("test");

            Assert.IsTrue(goodFilterPublished);
            Assert.IsFalse(badFilterPublished);

        }

        [TestMethod]
        public void SubscribeDefaultsThreadOptionAndNoFilter()
        {
            TestableWpfEvent<string> wpfEvent = new TestableWpfEvent<string>();
            int calledThreadID = -1;

            wpfEvent.Subscribe(delegate { calledThreadID = Thread.CurrentThread.ManagedThreadId; });

            wpfEvent.Publish("test");

            Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, calledThreadID);
        }

        [TestMethod]
        public void ShouldUnsubscribeFromPublisherThread()
        {
            var wpfEvent = new TestableWpfEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            wpfEvent.Subscribe(
                actionEvent,
                ThreadOption.PublisherThread);

            Assert.IsTrue(wpfEvent.Contains(actionEvent));
            wpfEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(wpfEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void UnsubscribeShouldNotFailWithNonSubscriber()
        {
            TestableWpfEvent<string> wpfEvent = new TestableWpfEvent<string>();

            Action<string> subscriber = delegate { };
            wpfEvent.Unsubscribe(subscriber);
        }

        [TestMethod]
        public void ShouldUnsubscribeFromBackgroundThread()
        {
            var wpfEvent = new TestableWpfEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            wpfEvent.Subscribe(
                actionEvent,
                ThreadOption.BackgroundThread);

            Assert.IsTrue(wpfEvent.Contains(actionEvent));
            wpfEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(wpfEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void ShouldUnsubscribeFromUIThread()
        {
            var wpfEvent = new TestableWpfEvent<string>();

            Action<string> actionEvent = delegate(string args) { };
            wpfEvent.Subscribe(
                actionEvent,
                ThreadOption.UIThread);

            Assert.IsTrue(wpfEvent.Contains(actionEvent));
            wpfEvent.Unsubscribe(actionEvent);
            Assert.IsFalse(wpfEvent.Contains(actionEvent));
        }

        [TestMethod]
        public void ShouldUnsubscribeASingleDelegate()
        {
            var wpfEvent = new TestableWpfEvent<string>();

            int callCount = 0;

            Action<string> actionEvent = delegate { callCount++; };
            wpfEvent.Subscribe(actionEvent);
            wpfEvent.Subscribe(actionEvent);

            wpfEvent.Publish(null);
            Assert.AreEqual<int>(2, callCount);

            callCount = 0;
            wpfEvent.Unsubscribe(actionEvent);
            wpfEvent.Publish(null);
            Assert.AreEqual<int>(1, callCount);
        }

        [TestMethod]
        public void ShouldNotExecuteOnGarbageCollectedDelegateReferenceWhenNotKeepAlive()
        {
            var wpfEvent = new TestableWpfEvent<string>();

            ExternalAction externalAction = new ExternalAction();
            wpfEvent.Subscribe(externalAction.ExecuteAction);

            wpfEvent.Publish("testPayload");
            Assert.AreEqual("testPayload", externalAction.PassedValue);

            WeakReference actionEventReference = new WeakReference(externalAction);
            externalAction = null;
            GC.Collect();
            Assert.IsFalse(actionEventReference.IsAlive);

            wpfEvent.Publish("testPayload");
        }

        [TestMethod]
        public void ShouldNotExecuteOnGarbageCollectedFilterReferenceWhenNotKeepAlive()
        {
            var wpfEvent = new TestableWpfEvent<string>();

            bool wasCalled = false;
            Action<string> actionEvent = delegate { wasCalled = true; };

            ExternalFilter filter = new ExternalFilter();
            wpfEvent.Subscribe(actionEvent, ThreadOption.PublisherThread, false, filter.AlwaysTrueFilter);

            wpfEvent.Publish("testPayload");
            Assert.IsTrue(wasCalled);

            wasCalled = false;
            WeakReference filterReference = new WeakReference(filter);
            filter = null;
            GC.Collect();
            Assert.IsFalse(filterReference.IsAlive);

            wpfEvent.Publish("testPayload");
            Assert.IsFalse(wasCalled);
        }

        [TestMethod]
        public void CanAddDescriptionWhileEventIsFiring()
        {
            TestableWpfEvent<string> wpfEvent = new TestableWpfEvent<string>();
            Action<string> emptyDelegate = delegate { };
            wpfEvent.Subscribe(delegate { wpfEvent.Subscribe(emptyDelegate); });

            Assert.IsFalse(wpfEvent.Contains(emptyDelegate));

            wpfEvent.Publish(null);

            Assert.IsTrue((wpfEvent.Contains(emptyDelegate)));
        }

        [TestMethod]
        public void InlineDelegateDeclarationsDoesNotGetCollectedIncorrectlyWithWeakReferences()
        {
            TestableWpfEvent<string> wpfEvent = new TestableWpfEvent<string>();
            bool published = false;
            wpfEvent.Subscribe(delegate { published = true; }, ThreadOption.PublisherThread, false, delegate { return true; });
            GC.Collect();
            wpfEvent.Publish(null);

            Assert.IsTrue(published);
        }

        [TestMethod]
        public void ShouldNotGarbageCollectDelegateReferenceWhenUsingKeepAlive()
        {
            var wpfEvent = new TestableWpfEvent<string>();

            var externalAction = new ExternalAction();
            wpfEvent.Subscribe(externalAction.ExecuteAction, ThreadOption.PublisherThread, true);

            WeakReference actionEventReference = new WeakReference(externalAction);
            externalAction = null;
            GC.Collect();
            GC.Collect();
            Assert.IsTrue(actionEventReference.IsAlive);

            wpfEvent.Publish("testPayload");

            Assert.AreEqual("testPayload", ((ExternalAction)actionEventReference.Target).PassedValue);
        }

        [TestMethod]
        public void RegisterReturnsTokenThatCanBeUsedToUnsubscribe()
        {
            var wpfEvent = new TestableWpfEvent<string>();
            Action<string> action = delegate { };

            var token = wpfEvent.Subscribe(action);
            wpfEvent.Unsubscribe(token);

            Assert.IsFalse(wpfEvent.Contains(action));
        }

        [TestMethod]
        public void ContainsShouldSearchByToken()
        {
            var wpfEvent = new TestableWpfEvent<string>();
            Action<string> action = delegate { };
            var token = wpfEvent.Subscribe(action);

            Assert.IsTrue(wpfEvent.Contains(token));

            wpfEvent.Unsubscribe(action);
            Assert.IsFalse(wpfEvent.Contains(token));
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

    class TestableWpfEvent<TPayload> : WpfEvent<TPayload>
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

    class CustomEvent : TestableWpfEvent<Payload> { }

    class Payload { }
}