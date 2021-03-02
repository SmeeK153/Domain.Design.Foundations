using Domain.Design.Foundations.Core.Abstract;
using MediatR;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// Unit of work completed by an <see cref="Entity{T}"/> that is to be communicated to any registered
    /// <see cref="INotificationHandler{TNotification}"/>.
    /// </summary>
    public abstract class MediatRDomainEvent : DomainEvent, INotification
    {
    }
}