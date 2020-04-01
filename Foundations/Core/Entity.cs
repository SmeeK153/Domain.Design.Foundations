using System;
using Foundations.Core.Abstract;
using Foundations.Events;

namespace Foundations.Core
{
    public class Entity : Entity<Guid>
    {
        public Entity(Action<DomainEvent> domainEventPublisher) : base(Guid.NewGuid(), domainEventPublisher)
        {
        }
        
        public Entity(Guid id, Action<DomainEvent> domainEventPublisher) : base(id, domainEventPublisher)
        {
        }
    }
}