using System;
namespace CX.Interfaces
{
    public interface IEventBrokerService
    {
        void Publish<TSubscriber>(Listener<TSubscriber> listener);
        void Subscribe<TSubscriber>(TSubscriber subscriber);
    }

    public delegate void Listener<TSubscriber>(TSubscriber subscriber);
}
