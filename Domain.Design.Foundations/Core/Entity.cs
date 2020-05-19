using System;
using Domain.Design.Foundations.Core.Abstract;

namespace Domain.Design.Foundations.Core
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