using System;
using System.Collections.Generic;
using Domain.Design.Foundations.Events;
using Domain.Design.Foundations.Exceptions;

namespace Domain.Design.Foundations.Core.Abstract
{
    /// <summary>
    /// Unique representation of a stateful abstraction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Entity<T> : Value, IObservable<DomainEvent>
    {
        private List<IObserver<DomainEvent>> Observers { get; } = new List<IObserver<DomainEvent>>();
        
        /// <summary>
        /// Identifier of the Entity instance
        /// </summary>
        public T Id { get; }
        
        protected Entity(T id)
        {
            Id = id ?? throw new DomainException($"Id is required for entity {GetType().Name}");
        }
        
        protected sealed override IEnumerable<object> GetComponentValues()
        {
            if (Id != null) yield return Id;
        }
        
        public IDisposable Subscribe(IObserver<DomainEvent> observer)
        {
            if (!Observers.Contains(observer))
            {
                Observers.Add(observer);
            }
            
            var disposable = new DomainSubscription(() => Unsubscribe(observer));

            return disposable;
        }
        
        private void Unsubscribe(IObserver<DomainEvent> observer)
        {
            if (Observers.Contains(observer))
            {
                Observers.Remove(observer);
            }
        }
        
        protected void PublishDomainEvent(DomainEvent domainEvent) =>
            Observers.ForEach(observer => observer.OnNext(domainEvent));

        protected void PublishDomainException(DomainException domainException) =>
            Observers.ForEach(observer => observer.OnError(domainException));
    }
}
