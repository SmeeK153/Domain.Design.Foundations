using System;
using System.Collections.Generic;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// Observer for domain events to further propagate events across the external infrastructure
    /// </summary>
    internal class DomainEventObserver : IObserver<DomainEvent>, IDisposable
    {
        public DomainEventObserver(IObservable<DomainEvent> observable)
        {
            Subscription = (DomainSubscription) observable.Subscribe(this);
        }

        public Queue<DomainEvent> DomainEvents => 
            Subscription.IsDisposed ? new Queue<DomainEvent>(_domainEventQueue) : new Queue<DomainEvent>();
        
        private Queue<DomainEvent> _domainEventQueue { get; } = new Queue<DomainEvent>();
        
        private DomainSubscription Subscription { get; }

        public virtual void OnCompleted()
        {
            Dispose();
        }

        public virtual void OnError(Exception error)
        {
            _domainEventQueue.Clear();
            Dispose();
            throw error;
        }

        public virtual void OnNext(DomainEvent value)
        {
            _domainEventQueue.Enqueue(value);
        }

        public void Dispose() => Subscription.Dispose();
    }
}