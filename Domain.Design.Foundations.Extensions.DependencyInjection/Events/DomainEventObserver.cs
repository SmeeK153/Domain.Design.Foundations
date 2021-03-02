using System;
using System.Threading.Tasks;
using Domain.Design.Foundations.Core.Abstract;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// <see cref="IObserver{T}"/> listening for any <see cref="DomainEvent"/>s  received from the subscribed
    /// <see cref="Entity{T}"/> instance.
    /// </summary>
    public abstract class DomainEventObserver : IObserver<DomainEvent>, IDisposable
    {
        /// <summary>
        /// Create a new observer of a particular <see cref="Entity{T}"/> instance that listens for
        /// <see cref="DomainEvent"/>s and processes them in the order they were received.
        /// </summary>
        /// <param name="observable"><see cref="Entity{T}"/> instance to listen for <see cref="DomainEvent"/>s</param>
        /// <param name="domainEventProcessingDelegate">Delegate to use for processing the <see cref="DomainEvent"/></param>
        public DomainEventObserver(
            IObservable<DomainEvent> observable,
            Func<DomainEvent, Task> domainEventProcessingDelegate
        )
        {
            Subscription = (DomainSubscription) observable.Subscribe(this);
            ProcessDomainEvent = domainEventProcessingDelegate;
        }

        /// <summary>
        /// Handler for closing-out this instance after reaching the end of domain processing.
        /// </summary>
        public abstract void OnCompleted();

        /// <summary>
        /// Handler for any <see cref="DomainException"/> received by the <see cref="IObserver{T}"/>, and closing-out
        /// this instance from future processing.
        /// </summary>
        /// <param name="error"><see cref="DomainException"/> received</param>
        public abstract void OnError(Exception error);

        /// <summary>
        /// Handler for new <see cref="DomainEvent"/>s received by the <see cref="IObserver{T}"/>.
        /// </summary>
        /// <param name="value"><see cref="DomainEvent"/> received</param>
        public abstract void OnNext(DomainEvent value);

        /// <summary>
        /// Cancels the current <see cref="DomainSubscription"/> to dispose this <see cref="IObserver{T}"/> and
        /// discontinue listening to <see cref="DomainEvent"/>s from the specific <see cref="Entity{T}"/> instance.
        /// </summary>
        public void Dispose() => Subscription.Dispose();

        /// <summary>
        /// Checks whether this instance has previously been disposed and may no longer process any more
        /// <see cref="DomainEvent"/>s.
        /// </summary>
        public Boolean IsDisposed => Subscription.IsDisposed;

        /// <summary>
        /// Executes the <see cref="DomainEvent"/> processing strategy provided by the
        /// <see cref="DomainEventObserverManager"/>. This should only be called internally, when this instance
        /// is completed.
        /// </summary>
        protected Func<DomainEvent, Task> ProcessDomainEvent { get; }

        /// <summary>
        /// Subscription to the <see cref="DomainEvent"/>s of the particular <see cref="Entity{T}"/> instance
        /// </summary>
        private DomainSubscription Subscription { get; }
    }
}