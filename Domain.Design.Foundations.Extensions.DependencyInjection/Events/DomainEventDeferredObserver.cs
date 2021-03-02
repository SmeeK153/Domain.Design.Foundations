using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Design.Foundations.Core.Abstract;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// <see cref="IObserver{T}"/> listening for, and queuing first-in-first-out, any <see cref="DomainEvent"/>s
    /// received from the subscribed <see cref="Entity{T}"/> instance to be processed all at once.
    /// </summary>
    public class DomainEventDeferredObserver : DomainEventObserver
    {
        /// <summary>
        /// Create a new observer of a particular <see cref="Entity{T}"/> instance that queues <see cref="DomainEvent"/>s
        /// until the manager executes them in the order they were received.
        /// </summary>
        /// <param name="observable"><see cref="Entity{T}"/> instance to listen for <see cref="DomainEvent"/>s</param>
        /// <param name="domainEventProcessingDelegate">Communication strategy for received <see cref="DomainEvent"/>s</param>
        public DomainEventDeferredObserver(
            IObservable<DomainEvent> observable,
            Func<DomainEvent, Task> domainEventProcessingDelegate
        ) : base(observable, domainEventProcessingDelegate)
        {
        }

        /// <summary>
        /// Current <see cref="DomainEvent"/>s that have been received, but haven't been processed yet.
        /// </summary>
        public Queue<DomainEvent> DomainEvents =>
            IsDisposed ? new Queue<DomainEvent>() : new Queue<DomainEvent>(DomainEventQueue);

        /// <summary>
        /// Cancels this <see cref="IObserver{T}"/>'s <see cref="DomainSubscription"/>.
        /// </summary>
        public override void OnCompleted()
        {
            foreach (var domainEvent in DomainEventQueue)
            {
                ProcessDomainEvent(domainEvent);
            }

            Dispose();
        }

        /// <summary>
        /// Clears the current, unprocessed <see cref="DomainEvent"/>s received by the specific <see cref="Entity{T}"/>
        /// instance, cancels this <see cref="IObserver{T}"/>'s <see cref="DomainSubscription"/>, and throws the
        /// received <see cref="Exception"/> received. This will end all further processing by this instance and will
        /// require a new instance to resume listening to <see cref="DomainEvent"/>s.
        /// </summary>
        /// <param name="error">Error raised by the <see cref="Entity{T}"/></param>
        /// <exception cref="Exception"><see cref="DomainException"/> error during domain processing</exception>
        public override void OnError(Exception error)
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
        public override void OnNext(DomainEvent value) => DomainEventQueue.Enqueue(value);

        /// <summary>
        /// Internal queue of unprocessed <see cref="DomainEvent"/>s, in the order they were received.
        /// </summary>
        private Queue<DomainEvent> DomainEventQueue { get; } = new Queue<DomainEvent>();
    }
}