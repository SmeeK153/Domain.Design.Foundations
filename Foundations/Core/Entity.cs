using System;
using Foundations.Core.Abstract;
using Foundations.Events;

namespace Foundations.Core
{
    public abstract class Entity : Entity<Guid>
    {
        protected Entity() : base(Guid.NewGuid())
        {
        }

        protected Entity(Guid id) : base(id)
        {
        }
    }
}