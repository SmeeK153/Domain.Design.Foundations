using MediatR;

namespace Domain.Design.Foundations.Events
{
    public abstract class MediatRDomainEvent : DomainEvent, INotification
    {
    }
}