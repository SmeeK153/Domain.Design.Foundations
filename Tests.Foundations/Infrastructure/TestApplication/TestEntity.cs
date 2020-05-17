using Domain.Design.Foundations.Core;
using Domain.Design.Foundations.Exceptions;
using Tests.Foundations.Infrastructure.TestApplication.Events;

namespace Tests.Foundations.Infrastructure.TestApplication
{
    public class TestEntity : Entity
    {
        public void PublishTestEvent()
        {
            PublishDomainEvent(new TestDomainEvent());
        }

        public void RaiseTestException()
        {
            PublishDomainException(new DomainException("Test integration exception"));
        }
    }
}