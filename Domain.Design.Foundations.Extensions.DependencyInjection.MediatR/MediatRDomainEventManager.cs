using System;
using System.Threading.Tasks;
using Domain.Design.Foundations.Core.Abstract;
using Domain.Design.Foundations.Events;
using MediatR;

namespace Domain.Design.Foundations
{
    /// <summary>
    /// Forwards received <see cref="DomainEvent"/>s to the <see cref="INotificationHandler{TNotification}"/> during the
    /// request lifecycle.
    /// </summary>
    public class MediatRDomainEventManager : DomainEventObserverManager
    {
        /// <summary>
        /// Creates a new event manager that routes any <see cref="DomainEvent"/>s received by known
        /// <see cref="IObserver{T}"/>s to the <see cref="INotificationHandler{TNotification}"/>.
        /// </summary>
        /// <param name="mediator">MediatR instance to use for publishing <see cref="DomainEvent"/>s</param>
        /// <param name="handler"><see cref="INotificationHandler{TNotification}"/> to received published
        /// <see cref="DomainEvent"/>s</param>
        public MediatRDomainEventManager(IMediator mediator, INotificationHandler<MediatRDomainEvent> handler)
        {
            Mediator = mediator;
        }

        /// <summary>
        /// Processing strategy for forwarding <see cref="DomainEvent"/>s to the
        /// <see cref="INotificationHandler{TNotification}"/> for separate handling.
        /// </summary>
        /// <param name="domainEvent"><see cref="DomainEvent"/> received from the <see cref="Entity{T}"/> the
        /// <see cref="IObserver{T}"/> has been listening to</param>
        protected override async Task ExecuteEvent(DomainEvent domainEvent)
        {
            await Mediator.Publish(domainEvent);
        }
        
        /// <summary>
        /// MediatR instance for publishing <see cref="DomainEvent"/>s to the
        /// <see cref="INotificationHandler{TNotification}"/>.
        /// </summary>
        private IMediator Mediator { get; }
    }
}