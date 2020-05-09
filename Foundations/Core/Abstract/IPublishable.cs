using System;
using Domain.Design.Foundations.Events;

namespace Domain.Design.Foundations.Core.Abstract
{
    public interface IPublishable
    {
        protected Action<DomainEvent> PublishDomainEvent { get; set; }
    }
}