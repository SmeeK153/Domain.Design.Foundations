using System;
using System.Collections.Generic;
using Domain.Design.Foundations.Events;

namespace Domain.Design.Foundations.Core.Abstract
{
    /// <summary>
    /// Unique representation of a stateful abstraction
    /// </summary>
    /// <typeparam name="T">The type of identity this stateful abstraction will use to distinguish itself from other
    /// instances</typeparam>
    public abstract class Entity<T> : Value, IObservable<DomainEvent>
    {
        /// <summary>
        /// Unique identifier of the <see cref="Entity"/> instance.
        /// </summary>
        public T Id { get; }
        
        /// <summary>
        /// <see cref="IObserver{T}"/>s may request to listen to any <see cref="DomainEvent"/>s emitted by this specific
        /// <see cref="Entity"/> by subscribing, and they may cancel their subscription by unsubscribing through the
        /// <see cref="DomainSubscription"/> instance itself
        /// </summary>
        /// <param name="observer"><see cref="IObserver{T}"/> to add for listening to this <see cref="Entity"/>
        /// instance's <see cref="DomainEvent"/>s</param>
        /// <returns>An <see cref="IDisposable"/> <see cref="DomainSubscription"/> in order to cancel the subscription
        /// in the future</returns>
        public IDisposable Subscribe(IObserver<DomainEvent> observer)
        {
            if (!Observers.Contains(observer))
            {
                Observers.Add(observer);
            }

            var disposable = new DomainSubscription(() => Unsubscribe(observer));

            return disposable;
        }
        
        /// <summary>
        /// Create a new instance of a pre-existing <see cref="Entity"/>.
        /// </summary>
        /// <param name="id">Unique identity of the pre-existing <see cref="Entity"/> instance</param>
        protected Entity(T id)
        {
            Id = id ?? throw new DomainException($"Id is required for entity {GetType().Name}");
        }

        /// <summary>
        /// Communicates a change of this <see cref="Entity"/> instance to each of its current <see cref="DomainEvent"/>
        /// <see cref="IObserver{T}"/>s.
        /// </summary>
        /// <param name="domainEvent">Domain event to communicate to each current <see cref="IObserver{T}"/>
        /// listening to this <see cref="Entity"/> instance's <see cref="DomainEvent"/>s</param>
        protected void PublishDomainEvent(DomainEvent domainEvent) =>
            Observers.ForEach(observer => observer.OnNext(domainEvent));

        /// <summary>
        /// Communicates an error from this <see cref="Entity"/> instance to each of its current <see cref="DomainEvent"/>
        /// <see cref="IObserver{T}"/>s.
        /// </summary>
        /// <param name="domainException">Domain error to communicate to each current <see cref="IObserver{T}"/>
        /// listening to this <see cref="Entity"/> instance's <see cref="DomainEvent"/>s</param>
        protected void PublishDomainException(DomainException domainException) =>
            Observers.ForEach(observer => observer.OnError(domainException));
        
        /// <summary>
        /// Retrieves the <see cref="Entity"/> instance's component values that comprise its identity for determining
        /// equality between different instances. An <see cref="Entity"/> instance's uniqueness is defined only by the
        /// value of its <see cref="Id"/> and nothing else. If the <see cref="Id"/> between two <see cref="Entity"/>
        /// instances match, and they are the same class type, then they are considered the same, even if their other
        /// properties differ.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of a single property, the <see cref="Entity"/> instance's
        /// <see cref="Id"/> value <see cref="T"/></returns>
        protected sealed override IEnumerable<object> GetComponentValues()
        {
            if (Id != null) yield return Id;
        }
        
        /// <summary>
        /// All of the current <see cref="Entity"/> instance's <see cref="IObserver{T}"/>s, in the order they were added.
        /// </summary>
        private List<IObserver<DomainEvent>> Observers { get; } = new List<IObserver<DomainEvent>>();
        
        /// <summary>
        /// Used within this class only to process requests to unsubscribe from this <see cref="Entity"/> instance's
        /// <see cref="DomainEvent"/>s by passing this method into the <see cref="DomainSubscription"/>'s disposal
        /// delegate; this should not be called in any other way
        /// </summary>
        /// <param name="observer"><see cref="IObserver{T}"/>, to be removed, listening to this <see cref="Entity"/>
        /// instance's <see cref="DomainEvent"/>s</param>
        private void Unsubscribe(IObserver<DomainEvent> observer)
        {
            if (Observers.Contains(observer))
            {
                Observers.Remove(observer);
            }
        }
    }
}