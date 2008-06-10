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
using System.Windows;
using System.Windows.Threading;
using Prism.Interfaces;

namespace Prism.Events
{
    public class PrismEvent<TPayload>
    {
        readonly List<PrismEventSubscription> _subscriptions = new List<PrismEventSubscription>();
        private readonly object _lockObject = new object();

        protected virtual Dispatcher UIDispatcher { get { return Application.Current.Dispatcher; } }

        public SubscriptionToken Subscribe(Action<TPayload> action)
        {
            return Subscribe(action, ThreadOption.PublisherThread);
        }

        public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption)
        {
            return Subscribe(action, threadOption, false);
        }

        public SubscriptionToken Subscribe(Action<TPayload> action, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
        }

        public SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, threadOption, keepSubscriberReferenceAlive, delegate { return true; });
        }

        /// <summary>
        /// Subscribes a delegate to an event.
        /// </summary>
        /// <param name="action">The delegate that gets executed when the event is raised.</param>
        /// <param name="threadOption">Specifies on which thread to receive the delegate callback.</param>
        /// <param name="keepSubscriberReferenceAlive">If <see langword="true"/>, the <seealso cref="PrismEvent{TPayload}"/> keeps a reference to the subscriber so it does not get garbage collected. </param>
        /// <param name="filter">Filter to decide if the subscriber will receive the event.</param>
        /// <remarks>
        /// If setting <paramref name="keepSubscriberReferenceAlive"/> is set to false, PrismEvent will maintain a <seealso cref="WeakReference"/> to the Target of the supplied <paramref name="action"/> delegate.
        /// If not using a WeakReference, the user must explicitly Unsubscribe to the event when disposing the subscriber in order to avoid memory leaks or unexepcted behavior.
        /// 
        /// The PrismEvent collection is thread-safe.
        /// </remarks>
        public virtual SubscriptionToken Subscribe(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayload> filter)
        {
            SubscriptionToken token = new SubscriptionToken();
            PrismEventSubscription subscription = null;

            if (keepSubscriberReferenceAlive)
            {
                subscription = new PrismEventSubscription { Action = action, Filter = filter, ThreadOption = threadOption, SubscriptionToken = token };
            }
            else
            {
                subscription = new WeakReferencedPrismEventSubscription { Action = action, Filter = filter, ThreadOption = threadOption, SubscriptionToken = token };
            }

            lock (_lockObject)
            {
                _subscriptions.Add(subscription);
            }
            return token;
        }

        private List<PrismEventSubscription> PruneAndCloneList()
        {
            List<PrismEventSubscription> returnList = new List<PrismEventSubscription>();

            lock (_lockObject)
            {
                for (var i = _subscriptions.Count - 1; i >= 0; i--)
                {
                    PrismEventSubscription listItem =
                        _subscriptions[i].ToPrismEventSubscription();

                    if (listItem == null)
                    {
                        // Prune from main list. Log?
                        _subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        returnList.Add(listItem);
                    }
                }
            }

            return returnList;
        }

        public virtual void Publish(TPayload payload)
        {
            List<PrismEventSubscription> list = PruneAndCloneList();

            foreach (var subscription in list.Where(evt => evt.ThreadOption == ThreadOption.PublisherThread))
            {
                if (subscription.Filter(payload))
                {
                    subscription.Action(payload);
                }
            }

            foreach (var subscription in list.Where(evt => evt.ThreadOption == ThreadOption.UIThread))
            {
                if (subscription.Filter(payload))
                {
                    UIDispatcher.BeginInvoke(DispatcherPriority.Normal, subscription.Action, payload);
                }
            }

            foreach (var subscription in list.Where(evt => evt.ThreadOption == ThreadOption.BackgroundThread))
            {
                if (subscription.Filter(payload))
                {
                    subscription.Action.BeginInvoke(payload, null, null);
                }
            }
        }

        /// <summary>
        /// Removes the first subscriber matching <seealso cref="Action{T}"/>from the list.
        /// </summary>
        /// <param name="subscriber"></param>
        public void Unsubscribe(Action<TPayload> subscriber)
        {
            lock (_lockObject)
            {
                PrismEventSubscription prismEvent = _subscriptions.FirstOrDefault(evt => evt.Action == subscriber);
                if (prismEvent != null)
                {
                    _subscriptions.Remove(prismEvent);
                }
            }
        }


        /// <summary>
        /// Removes the subscriber matching the <seealso cref="SubscriptionToken"/>.
        /// </summary>
        /// <param name="token"></param>
        public void Unsubscribe(SubscriptionToken token)
        {
            lock (_lockObject)
            {
                PrismEventSubscription item = _subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
                if (item != null)
                {
                    _subscriptions.Remove(item);
                }
            }
        }


        /// <summary>
        /// Returns true if there is a subscriber matching <seealso cref="Action{T}"/>.
        /// </summary>
        /// <param name="actionEvent"></param>
        /// <returns>true if there is an <seealso cref="Action{T}"/> that matches</returns>
        public bool Contains(Action<TPayload> actionEvent)
        {
            lock (_lockObject)
            {
                PrismEventSubscription prismEvent = _subscriptions.FirstOrDefault(evt => evt.Action == actionEvent);
                return prismEvent != null;
            }
        }

        /// <summary>
        /// Returns true if there is a subscriber matching <seealso cref="SubscriptionToken"/>.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>true if there is a <seealso cref="SubscriptionToken"/> that matches.</returns>
        public bool Contains(SubscriptionToken token)
        {
            lock (_lockObject)
            {
                PrismEventSubscription prismEvent = _subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
                return prismEvent != null;
            }
        }

        private class PrismEventSubscription
        {
            public PrismEventSubscription()
            {
                this.ThreadOption = ThreadOption.PublisherThread;
            }

            public virtual Action<TPayload> Action { get; set; }
            public virtual Predicate<TPayload> Filter { get; set; }
            public ThreadOption ThreadOption { get; set; }
            public SubscriptionToken SubscriptionToken { get; set; }

            public virtual PrismEventSubscription ToPrismEventSubscription()
            {
                return this;
            }
        }

        private class WeakReferencedPrismEventSubscription : PrismEventSubscription
        {
            private WeakDelegate<Action<TPayload>> _action;
            private WeakDelegate<Predicate<TPayload>> _filter;

            public override Action<TPayload> Action
            {
                get { return _action.Target; }
                set { _action = new WeakDelegate<Action<TPayload>>(value); }
            }

            public override Predicate<TPayload> Filter
            {
                get { return _filter.Target; }
                set { _filter = new WeakDelegate<Predicate<TPayload>>(value); }
            }

            public override PrismEventSubscription ToPrismEventSubscription()
            {
                Action<TPayload> action = this.Action;
                Predicate<TPayload> filter = this.Filter;
                if (action != null && filter != null)
                {
                    return new PrismEventSubscription() { Action = action, Filter = filter, ThreadOption = this.ThreadOption, SubscriptionToken = this.SubscriptionToken };
                }
                return null;
            }
        }
    }
}