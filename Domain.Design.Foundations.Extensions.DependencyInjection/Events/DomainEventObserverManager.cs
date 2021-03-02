using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Design.Foundations.Core.Abstract;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DomainEventObserverManager : IDomainEventManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="observableDomainEntity"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public Task StartListening(IObservable<DomainEvent> observableDomainEntity, ObserverBehavior behavior)
        {
            switch (behavior)
            {
                case ObserverBehavior.Immediate:
                    Observers.Add(new DomainEventImmediateObserver(observableDomainEntity, ExecuteEvent));
                    break;
                case ObserverBehavior.Deferred:
                    Observers.Add(new DomainEventDeferredObserver(observableDomainEntity, ExecuteEvent));
                    break;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Completes all of the registered <see cref="IObserver{T}"/>s.
        /// </summary>
        public void Dispose()
        {
            Observers.ForEach(observer => observer.OnCompleted());
        }

        /// <summary>
        /// Checks whether this instance has previously been disposed and may no longer process any more
        /// <see cref="DomainEvent"/>s.
        /// </summary>
        public Boolean IsDisposed => Observers.All(observer => observer.IsDisposed);
        
        /// <summary>
        /// Processing strategy for a <see cref="DomainEvent"/>; defined by each derived type.
        /// </summary>
        /// <param name="domainEvent"><see cref="DomainEvent"/> received from the <see cref="Entity{T}"/> the
        /// <see cref="IObserver{T}"/> has been listening to</param>
        protected abstract Task ExecuteEvent(DomainEvent domainEvent);

        /// <summary>
        /// Internal list of the registered <see cref="IObserver{T}"/>s that are listening to the
        /// <see cref="Entity{T}"/>.
        /// </summary>
        private List<DomainEventObserver> Observers { get; } = new List<DomainEventObserver>();
    }
}