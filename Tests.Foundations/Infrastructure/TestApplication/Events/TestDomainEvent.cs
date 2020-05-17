using Domain.Design.Foundations.Events;
using MediatR;

namespace Tests.Foundations.Infrastructure.TestApplication.Events
{
    public class TestDomainEvent : DomainEvent, INotification
    {
        
    }
}