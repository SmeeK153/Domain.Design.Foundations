using System;
using System.Threading.Tasks;

namespace Domain.Design.Foundations.Events
{
    public interface IDomainEventManager
    {
        Task StartListening(IObservable<DomainEvent> observableDomainEntity);

        Task ExecuteEvents();
    }
}