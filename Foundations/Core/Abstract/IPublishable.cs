using Domain.Design.Foundations.Events;

namespace Domain.Design.Foundations.Core.Abstract
{
    public interface IPublishable
    {
        public void  PublishDomainEvent(DomainEvent domainEvent);
    }
}