using System;
using Foundations.Core.Abstract;
using Foundations.Events;

namespace Foundations.Core
{
    public abstract class Entity : Entity<Guid>
    {
        protected Entity(Action<DomainEvent> domainEventPublisher) : base(Guid.NewGuid(), domainEventPublisher)
        {
        }

        protected Entity(Guid id, Action<DomainEvent> domainEventPublisher) : base(id, domainEventPublisher)
        {
        }
    }
}