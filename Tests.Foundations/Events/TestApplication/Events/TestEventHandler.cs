using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Tests.Foundations.Events.TestApplication.Events
{
    public class TestEventHandler : INotificationHandler<TestDomainEvent>
    {
        private IDomainEventRepository _repository { get; }
        public TestEventHandler(IDomainEventRepository repository) => _repository = repository;

        public Task Handle(TestDomainEvent notification, CancellationToken cancellationToken)
        {
            _repository.LogDomainEvent(notification);
            return Task.CompletedTask;
        }
    }
}