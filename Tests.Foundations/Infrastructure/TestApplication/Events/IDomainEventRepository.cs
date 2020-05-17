using System.Collections.Generic;
using Domain.Design.Foundations.Events;

namespace Tests.Foundations.Infrastructure.TestApplication.Events
{
    public interface IDomainEventRepository
    {
        public void LogDomainEvent(DomainEvent domainEvent);
        
        public IEnumerable<DomainEvent> LoggedEvents { get; }
    }
}