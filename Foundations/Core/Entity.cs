using System;
using System.Collections.Generic;
using Foundations.Events;
using Foundations.Exceptions;

namespace Foundations.Core
{
    public abstract class Entity<T> : ValueObject
    {
        protected Entity(T id, Action<DomainEvent> domainEventPublisher)
        {
            Id = id ?? throw new DomainException($"Id is required for entity {GetType().Name}");
            DomainEventPublisher = 
                domainEventPublisher ?? 
                throw new DomainException("Domain publisher is required for domain changes to take effect");
        }

        public T Id { get; }

        public bool IsTransient => EqualityComparer<T>.Default.Equals(Id, default);
        
        private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
        private Action<DomainEvent> DomainEventPublisher { get; }
        protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        protected void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
        protected void ClearDomainEvents() => _domainEvents.Clear();
        public void PublishDomainEvents() => _domainEvents.ForEach(DomainEventPublisher);
        
        protected sealed override IEnumerable<object> GetComponentValues()
        {
            yield return Id;
        }
        
        public sealed override bool Equals(object? obj)
        {
            if (IsTransient || !(obj is Entity<T> entity) || entity.IsTransient)
                return false;

            return base.Equals(obj);
        }

        public bool Equals(Entity<T>? obj) => Equals((object?) obj);
        
        public sealed override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            return left.Equals(right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right) => !(left == right);
    }
}
