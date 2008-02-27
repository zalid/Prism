using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using CX.Interfaces;

namespace CX.Services
{
    public class EventBrokerService : IEventBrokerService
    {
        public EventBrokerService()
        {
        }

        private Dictionary<Type, List<object>> subscribers = new Dictionary<Type, List<object>>();
        
        public void Publish<TSubscriber>(Listener<TSubscriber> listener) {

            foreach (TSubscriber subscriber in subscribers.GetItem(typeof(TSubscriber)))
                listener(subscriber);
        }

        public void Subscribe<TSubscriber>(TSubscriber subscriber) {
            subscribers.EnsureGetItem(typeof(TSubscriber)).Add(subscriber);
        }

        
    }
    
    


}
