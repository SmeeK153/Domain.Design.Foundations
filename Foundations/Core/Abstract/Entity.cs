using System;
using System.Collections.Generic;
using Foundations.Events;
using Foundations.Exceptions;

namespace Foundations.Core.Abstract
{
    /// <summary>
    /// Unique representation of a stateful abstraction
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Entity<T> : ValueObject
    {
        /// <summary>
        /// Identifier of the Entity instance
        /// </summary>
        public T Id { get; }
        
        protected Entity(T id)
        {
            Id = id ?? throw new DomainException($"Id is required for entity {GetType().Name}");
        }

        /// <summary>
        /// Publishes a domain event to the domain and external infrastructure observer(s)
        /// </summary>
        protected Action<DomainEvent> PublishDomainEvent { get; } = DomainEventPublisher.Instance.Publish;

        protected sealed override IEnumerable<object> GetComponentValues()
        {
            if (Id != null) yield return Id;
        }
    }
}
