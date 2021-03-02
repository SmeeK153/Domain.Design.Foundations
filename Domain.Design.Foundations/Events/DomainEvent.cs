using System;
using Domain.Design.Foundations.Core.Abstract;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// Unit of work completed by an <see cref="Entity{T}"/> that is to be communicated to any attached
    /// <see cref="IObserver{T}"/>s.
    /// </summary>
    public abstract class DomainEvent
    {
    }
}
