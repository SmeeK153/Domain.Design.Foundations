using System;

namespace Foundations.Events
{
    /// <summary>
    /// Observer for domain events to further propagate events across the external infrastructure
    /// </summary>
    public abstract class DomainEventObserver : IObserver<DomainEvent>, IDisposable
    {
        protected DomainEventObserver()
        {
            Subscription = DomainEventPublisher.Instance.Subscribe(this);
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