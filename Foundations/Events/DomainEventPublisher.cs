using System;
using System.Collections.Generic;

namespace Foundations.Events
{
    internal class DomainEventPublisher : IObservable<DomainEvent>
    {
        private DomainEventPublisher()
        {
        }
        
        internal static DomainEventPublisher Instance { get; } = new DomainEventPublisher();

        private List<IObserver<DomainEvent>> Observers { get; } = new List<IObserver<DomainEvent>>();
        
        public IDisposable Subscribe(IObserver<DomainEvent> observer)
        {
            if (!Observers.Contains(observer))
            {
                Observers.Add(observer);
            }
            
            var disposable = new DomainEventPublisherSubscription(() => Unsubscribe(observer));

            return disposable;
        }

        private void Unsubscribe(IObserver<DomainEvent> observer)
        {
            if (Observers.Contains(observer))
            {
                Observers.Remove(observer);
            }
        }
        
        internal void Publish(DomainEvent domainEvent)
        {
            Observers.ForEach(observer => observer.OnNext(domainEvent));
        }
    }
}