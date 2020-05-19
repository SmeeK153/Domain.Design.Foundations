using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Design.Foundations.Events
{
    public abstract class DomainEventObserverManager : IDomainEventManager
    {
        private List<DomainEventObserver> _observers { get; } = new List<DomainEventObserver>();
        
        public Task StartListening(IObservable<DomainEvent> observableDomainEntity)
        {
            var observer = new DomainEventObserver(observableDomainEntity);
            _observers.Add(observer);
            return Task.CompletedTask;
        }

        public async Task ExecuteEvents()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
                foreach (var domainEvent in observer.DomainEvents)
                {
                    await ExecuteEvent(domainEvent);
                }
            }
        }

        protected abstract Task ExecuteEvent(DomainEvent domainEvent);
    }
}