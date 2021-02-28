using System;
using System.Collections.Generic;
using Domain.Design.Foundations.Core.Abstract;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// <see cref="IObserver{T}"/> listening for <see cref="DomainEvent"/>s communicated by a specific
    /// <see cref="Entity{T}"/> instance.
    /// </summary>
    internal class DomainEventObserver : IObserver<DomainEvent>, IDisposable
    {
        /// <summary>
        /// Create a new observer of a particular <see cref="Entity{T}"/> instance
        /// </summary>
        /// <param name="observable"><see cref="Entity{T}"/> instance to listen for <see cref="DomainEvent"/>s</param>
        public DomainEventObserver(IObservable<DomainEvent> observable)
        {
            Subscription = (DomainSubscription) observable.Subscribe(this);
        }

        /// <summary>
        /// Current <see cref="DomainEvent"/>s that have been received, but haven't been processed yet.
        /// </summary>
        public Queue<DomainEvent> DomainEvents => 
            Subscription.IsDisposed ? new Queue<DomainEvent>(DomainEventQueue) : new Queue<DomainEvent>();
        
        /// <summary>
        /// Internal queue of unprocessed <see cref="DomainEvent"/>s, in the order they were received.
        /// </summary>
        private Queue<DomainEvent> DomainEventQueue { get; } = new Queue<DomainEvent>();
        
        /// <summary>
        /// Subscription to the <see cref="DomainEvent"/>s of the particular <see cref="Entity{T}"/> instance
        /// </summary>
        private DomainSubscription Subscription { get; }

        /// <summary>
        /// Cancels this <see cref="IObserver{T}"/>'s <see cref="DomainSubscription"/>.
        /// </summary>
        public virtual void OnCompleted()
        {
            Dispose();
        }

        /// <summary>
        /// Clears the current, unprocessed <see cref="DomainEvent"/>s received by the specific <see cref="Entity{T}"/>
        /// instance, cancels this <see cref="IObserver{T}"/>'s <see cref="DomainSubscription"/>, and throws the
        /// received <see cref="Exception"/> received. This will end all further processing by this instance and will
        /// require a new instance to resume listening to <see cref="DomainEvent"/>s.
        /// </summary>
        /// <param name="error"></param>
        /// <exception cref="Exception"></exception>
        public virtual void OnError(Exception error)
        {
            DomainEventQueue.Clear();
            Dispose();
            throw error;
        }

        /// <summary>
        /// Receives and stores the <see cref="DomainEvent"/> received by the specific <see cref="Entity{T}"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="DomainEvent"/> communicated by the specific <see cref="Entity{T}"/>
        /// instance</param>
        public virtual void OnNext(DomainEvent value)
        {
            DomainEventQueue.Enqueue(value);
        }

        /// <summary>
        /// Cancels the current <see cref="DomainSubscription"/> to dispose this <see cref="IObserver{T}"/> and
        /// discontinue listening to <see cref="DomainEvent"/>s from the specific <see cref="Entity{T}"/> instance.
        /// </summary>
        public void Dispose() => Subscription.Dispose();
    }
}