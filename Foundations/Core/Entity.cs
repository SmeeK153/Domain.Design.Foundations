using System;
using System.Collections.Generic;
using Foundations.Events;
using Foundations.Exceptions;

namespace Foundations.Core
{
    /// <summary>
    /// Unique representation of a stateful abstraction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Entity<T> : ValueObject
    {
        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
        private Action<DomainEvent> DomainEventPublisher { get; }
        /// <summary>
        /// Identifier of the Entity instance
        /// </summary>
        public T Id { get; }
        
        protected Entity(T id, Action<DomainEvent> domainEventPublisher)
        {
            Id = id ?? throw new DomainException($"Id is required for entity {GetType().Name}");
            DomainEventPublisher = 
                domainEventPublisher ?? 
                throw new DomainException("Domain publisher is required for domain changes to take effect");
        }

        /// <summary>
        /// Adds a new domain event to this entity instance
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        
        /// <summary>
        /// Removes a specific domain event from this entity instance
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
        
        /// <summary>
        /// Removes all domain events from this entity instance
        /// </summary>
        protected void ClearDomainEvents() => _domainEvents.Clear();
        
        /// <summary>
        /// Publishes the existing domain events created by this entity to the corresponding handler
        /// </summary>
        public void PublishDomainEvents() => _domainEvents.ForEach(DomainEventPublisher);
        
        protected sealed override IEnumerable<object> GetComponentValues()
        {
            if (Id != null) yield return Id;
        }
    }
}
