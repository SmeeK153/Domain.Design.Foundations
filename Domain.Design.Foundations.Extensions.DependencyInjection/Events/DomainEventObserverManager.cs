using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Design.Foundations.Core.Abstract;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// Manages the <see cref="DomainEvent"/> processing strategies throughout the request lifecycle.
    /// </summary>
    public abstract class DomainEventObserverManager : IDomainEventManager
    {
        /// <summary>
        /// Registers a new <see cref="DomainEvent"/> listener with the desired processing behavior.
        /// </summary>
        /// <param name="observableDomainEntity"><see cref="Entity{T}"/> to be observed</param>
        /// <param name="behavior">Processing strategy for received <see cref="DomainEvent"/>s</param>
        /// <returns></returns>
        public Task StartListening(IObservable<DomainEvent> observableDomainEntity, EObserverBehavior behavior)
        {
            switch (behavior)
            {
                case EObserverBehavior.Immediate:
                    Observers.Add(new DomainEventImmediateObserver(observableDomainEntity, ExecuteEvent));
                    break;
                case EObserverBehavior.Deferred:
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