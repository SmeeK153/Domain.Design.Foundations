using System;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// Observer for domain events to further propagate events across the external infrastructure
    /// </summary>
    public abstract class DomainEventObserver : IObserver<DomainEvent>, IDisposable
    {
        protected DomainEventObserver(IObservable<DomainEvent> observable)
        {
            Subscription = observable.Subscribe(this);
        }
        
        private IDisposable Subscription { get; }

        public virtual void OnCompleted()
        {
            Dispose();
        }

        public virtual void OnError(Exception error)
        {
            Dispose();
        }
        public abstract void OnNext(DomainEvent value);

        public void Dispose() => Subscription.Dispose();
    }
}