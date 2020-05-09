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
    public abstract class Entity<T> : ValueObject, IPublishable
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
        Action<DomainEvent> IPublishable.PublishDomainEvent { get; set; } = (domainEvent) => { };
        
        protected Action<DomainEvent> PublishDomainEvent => IPublishable.PublishDomainEvent;

        protected sealed override IEnumerable<object> GetComponentValues()
        {
            if (Id != null) yield return Id;
        }
    }
}
