using System;
using System.Collections.Generic;
using Foundations.Exceptions;

namespace Foundations.Core
{
    public abstract class Entity<T>
    {
        protected Entity(T id)
        {
            Id = id ?? throw new DomainException($"Id is required for entity {this.GetType().Name}");
        }

        public T Id { get; }

        private int? RequestedHashCode { get; set; }

        public bool IsTransient { get => EqualityComparer<T>.Default.Equals(Id, default); }

        public override bool Equals(object? obj)
        {
            if (!(obj is Entity<T> entity))
                return false;

            if (Object.ReferenceEquals(this, entity))
                return true;

            if (this.GetType() != entity.GetType())
                return false;

            if (entity.IsTransient || this.IsTransient)
                return false;
            else
                return EqualityComparer<T>.Default.Equals(entity.Id, Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient)
            {
                if (!RequestedHashCode.HasValue)
                    RequestedHashCode = Id!.GetHashCode() ^ 31;

                return RequestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            if (Object.Equals(left, null))
                return Object.Equals(right, null) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right) => !(left == right);
    }
}
