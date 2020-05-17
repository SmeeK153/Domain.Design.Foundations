using System.Threading.Tasks;
using Domain.Design.Foundations.Events;
using MediatR;

namespace Domain.Design.Foundations
{
    public class MediatRDomainEventManager : DomainEventObserverManager
    {
        private IMediator _mediator { get; }
        public MediatRDomainEventManager(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        protected override async Task ExecuteEvent(DomainEvent domainEvent)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}