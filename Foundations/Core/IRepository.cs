using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundations.Core
{
    public interface IRepository<Entity, TId> where Entity : IAggregateRoot
    {
        Task<Entity<TId>> Get(TId id);

        Task<IEnumerable<Entity<TId>>> GetMany(Func<Entity<TId>, bool> predicate);

        Task Save(Entity<TId> entity);
    }
}
