using System;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Design.Foundations.Events;
using Domain.Design.Foundations.Extensions;
using Domain.Design.Foundations.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tests.Foundations.Events.TestApplication;
using Tests.Foundations.Events.TestApplication.Events;
using Xunit;

namespace Tests.Foundations.Events.MediatR
{
    public class DomainMiddlewareTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        [Fact]
        public async Task DomainEventsAreExecutedByApplicationBuilderDuringResponseBuilding()
        {
            var webhostBuilder = Program.CreateHostBuilder();
            var mockRepository = new Mock<IDomainEventRepository>();
            webhostBuilder.ConfigureServices(services =>
            {
                services.AddMediatRDomainEvents(Assembly.GetExecutingAssembly());
                services.AddScoped<IDomainEventRepository>(provider => mockRepository.Object);
            });
            var server = new TestServer(webhostBuilder);
            var client = server.CreateClient();
            var response = await client.GetAsync("/test/success");
            response.IsSuccessStatusCode.Should().BeTrue();
            
            mockRepository.Verify(x => x.LogDomainEvent(It.IsAny<DomainEvent>()), Times.Exactly(3));
        }
        
        [Fact]
        public async Task NoQueuedDomainEventsAreExecutedByApplicationBuilderDuringResponseBuildingAfterException()
        {
            var webhostBuilder = Program.CreateHostBuilder();
            var mockRepository = new Mock<IDomainEventRepository>();
            webhostBuilder.ConfigureServices(services =>
            {
                services.AddMediatRDomainEvents(Assembly.GetExecutingAssembly());
                services.AddScoped<IDomainEventRepository>(provider => mockRepository.Object);
            });
            var server = new TestServer(webhostBuilder);
            var client = server.CreateClient();
            var response = await client.GetAsync("/test/fail");
            response.IsSuccessStatusCode.Should().BeFalse();
            
            mockRepository.Verify(x => x.LogDomainEvent(It.IsAny<DomainEvent>()), Times.Never);
        }
        
        [Fact]
        public async Task SkippingAddDomainEventsCallYieldsErrorForUseDomainEventsCall()
        {
            var webhostBuilder = Program.CreateHostBuilder();
            var mockRepository = new Mock<IDomainEventRepository>();
            webhostBuilder.ConfigureServices(services =>
            {
                services.AddScoped<IDomainEventRepository>(provider => mockRepository.Object);
            });
            var server = new TestServer(webhostBuilder);
            var client = server.CreateClient();
            Func<Task> startup = async () => await client.GetAsync("/test/fail");
            startup
                .Should()
                .ThrowExactly<InvalidOperationException>()
                .And
                .Message
                .Should()
                .Be($"Unable to resolve service for type '{typeof(IDomainEventManager)}' while attempting to Invoke middleware '{typeof(DomainMiddleware)}'.");
        }
    }
}