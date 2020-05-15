using System;
using System.Collections.Generic;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// Observer for domain events to further propagate events across the external infrastructure
    /// </summary>
    public sealed class DomainEventObserver : IObserver<DomainEvent>, IDisposable
    {
        public DomainEventObserver(IObservable<DomainEvent> observable, Queue<DomainEvent> domainEventQueue)
        {
            Subscription = observable.Subscribe(this);
            _domainEventQueue = domainEventQueue;
        }
        private Queue<DomainEvent> _domainEventQueue { get; }
        private IDisposable Subscription { get; }

        public void OnCompleted()
        {
            Dispose();
        }

        public void OnError(Exception error)
        {
            Dispose();
        }

        public void OnNext(DomainEvent value)
        {
            _domainEventQueue.Enqueue(value);
        }

        public void Dispose() => Subscription.Dispose();
    }
}