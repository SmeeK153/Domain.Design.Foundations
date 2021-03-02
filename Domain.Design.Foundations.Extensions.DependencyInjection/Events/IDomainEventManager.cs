using System;
using System.Threading.Tasks;
using Domain.Design.Foundations.Core.Abstract;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// Manager instance for controlling the various <see cref="IObserver{T}"/>s for a given <see cref="Entity{T}"/>'s
    /// <see cref="DomainEvent"/>s.
    /// </summary>
    public interface IDomainEventManager : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="observableDomainEntity"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        Task StartListening(IObservable<DomainEvent> observableDomainEntity, ObserverBehavior behavior);
    }
}