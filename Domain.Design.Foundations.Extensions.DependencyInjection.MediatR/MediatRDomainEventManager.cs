using System.Threading.Tasks;
using Domain.Design.Foundations.Events;
using MediatR;

namespace Domain.Design.Foundations
{
    /// <summary>
    /// 
    /// </summary>
    public class MediatRDomainEventManager : DomainEventObserverManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public MediatRDomainEventManager(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        protected override async Task ExecuteEvent(DomainEvent domainEvent)
        {
            await _mediator.Publish(domainEvent);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private IMediator _mediator { get; }
    }
}