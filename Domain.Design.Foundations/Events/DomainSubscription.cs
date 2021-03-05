using System;
using Domain.Design.Foundations.Core.Abstract;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// <see cref="IObserver{T}"/>'s subscription to listen to any <see cref="DomainEvent"/>s published by the
    /// associated <see cref="Entity{T}"/>.
    /// </summary>
    public class DomainSubscription : IDisposable
    {
        /// <summary>
        /// Create a new subscription to handle the process of unsubscribing from an <see cref="IObservable{T}"/>
        /// defined in the disposal <see cref="Action"/>.
        /// </summary>
        /// <param name="disposal">Delegate to be executed during disposal in order to unsubscribe from the
        /// <see cref="IObservable{T}"/>'s <see cref="DomainEvent"/>s</param>
        internal DomainSubscription(Action disposal)
        {
            Disposal = disposal;
        }

        /// <summary>
        /// Disposal processing <see cref="Action"/> defined by the <see cref="IObservable{T}"/> that this
        /// <see cref="DomainSubscription"/> was created for.
        /// </summary>
        private Action Disposal { get; }
        
        /// <summary>
        /// Invokes the disposal <see cref="Action"/> provided by the <see cref="IObservable{T}"/> that created this
        /// instance.
        /// </summary>
        public void Dispose()
        {
            Disposal.Invoke();
            IsDisposed = true;
        }

        /// <summary>
        /// Whether the <see cref="DomainSubscription"/> has previously be disposed yet or not
        /// </summary>
        public bool IsDisposed { get; private set; } = false;
    }
}