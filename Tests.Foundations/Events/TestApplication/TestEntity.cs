using Domain.Design.Foundations.Core;
using Domain.Design.Foundations.Events;
using Tests.Foundations.Events.TestApplication.Events;

namespace Tests.Foundations.Events.TestApplication
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