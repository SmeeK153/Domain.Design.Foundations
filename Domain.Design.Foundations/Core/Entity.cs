using System;
using Domain.Design.Foundations.Core.Abstract;
using Domain.Design.Foundations.Events;

namespace Domain.Design.Foundations.Core
{
    /// <summary>
    /// Unique representation of a stateful abstraction using a <see cref="Guid"/> as the identity.
    /// </summary>
    public abstract class Entity : Entity<Guid>
    {
        /// <summary>
        /// Create a new identity instance of the <see cref="Entity"/>.
        /// </summary>
        protected Entity() : base(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Create a new instance of a pre-existing <see cref="Entity"/>.
        /// </summary>
        /// <param name="id">Unique identity of the pre-existing <see cref="Entity"/> instance</param>
        protected Entity(Guid id) : base(id)
        {
        }
    }
}