using System.Collections.Generic;
using System.Linq;
using Domain.Design.Foundations.Events;
using Domain.Design.Foundations.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests.Foundations
{
    public class DomainExtensionsTests
    {
        private IServiceCollection _services { get; } = new ServiceCollection();
        
        
        [Fact]
        public void DomainEventsAreRegisteredWithServiceCollection()
        {
            _services.AddDomainEvents();

            var domainEventQueueDescriptor = new ServiceDescriptor(
                typeof(IEnumerable<DomainEvent>),
                typeof(DomainEventQueue),
                ServiceLifetime.Scoped);

            var lookup = _services.FirstOrDefault(descriptor =>
                descriptor.Lifetime == domainEventQueueDescriptor.Lifetime &&
                descriptor.ImplementationType == domainEventQueueDescriptor.ImplementationType &&
                descriptor.ServiceType == domainEventQueueDescriptor.ServiceType);

            lookup.Should().NotBeNull();
        }
        
        [Fact]
        public void DomainEventsAreUsedByApplicationBuilder()
        {
            
        }
    }
}