using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Design.Foundations.Core;
using Domain.Design.Foundations.Events;
using FluentAssertions;
using Xunit;

namespace Tests.Foundations.Core
{
    public class DomainEventTests
    {
        private class TestEvent : DomainEvent
        {
        }

        private class TestException : DomainException
        {
            public TestException() : base("Test exception")
            {
            }
        }

        private class TestEntity : Entity
        {
            public void PublishTestEvent() => PublishDomainEvent(new TestEvent());

            public void PublishTestException() => PublishDomainException(new TestException());
        }

        private class TestDomainEventManager : DomainEventObserverManager
        {
            public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

            protected override Task ExecuteEvent(DomainEvent domainEvent)
            {
                DomainEvents.Add(domainEvent);
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task EntityCanPublishEventsToHandlerToTriggerEffectsWithDeferredBehavior()
        {
            var entity = new TestEntity();
            var manager = new TestDomainEventManager();
            await manager.StartListening(entity, EObserverBehavior.Deferred);
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            manager.Dispose();
            manager.DomainEvents.Count.Should().Be(3);
        }

        [Fact]
        public async Task EntityCanPublishEventsToHandlerToTriggerEffectsWithImmediateBehavior()
        {
            var entity = new TestEntity();
            var manager = new TestDomainEventManager();
            await manager.StartListening(entity, EObserverBehavior.Immediate);
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            manager.Dispose();
            manager.DomainEvents.Count.Should().Be(3);
        }

        [Fact]
        public async Task NoMoreEventsAreReceivedAfterEventsAreExecutedWithDeferredBehavior()
        {
            var entity = new TestEntity();
            var manager = new TestDomainEventManager();
            await manager.StartListening(entity, EObserverBehavior.Deferred);
            entity.PublishTestEvent();
            manager.Dispose();
            manager.DomainEvents.Count.Should().Be(1);

            entity.PublishTestEvent();
            manager.DomainEvents.Count.Should().Be(1);

            manager.IsDisposed.Should().BeTrue();
        }

        [Fact]
        public async Task NoEventsAreReceivedAfterPublisherYieldsAnExceptionWithDeferredBehavior()
        {
            var entity = new TestEntity();
            var manager = new TestDomainEventManager();
            await manager.StartListening(entity, EObserverBehavior.Deferred);
            entity.PublishTestEvent();
            Action action = () => entity.PublishTestException();
            action.Should().Throw<TestException>();

            manager.Dispose();
            manager.DomainEvents.Count.Should().Be(0);

            entity.PublishTestEvent();
            manager.DomainEvents.Count.Should().Be(0);

            manager.IsDisposed.Should().BeTrue();
        }

        [Fact]
        public async Task NoEventsAreReceivedAfterPublisherYieldsAnExceptionWithImmediateBehavior()
        {
            var entity = new TestEntity();
            var manager = new TestDomainEventManager();
            await manager.StartListening(entity, EObserverBehavior.Immediate);
            entity.PublishTestEvent();
            Action action = () => entity.PublishTestException();
            action.Should().Throw<TestException>();

            manager.Dispose();
            manager.DomainEvents.Count.Should().Be(1);

            entity.PublishTestEvent();
            manager.DomainEvents.Count.Should().Be(1);

            manager.IsDisposed.Should().BeTrue();
        }

        [Fact]
        public async Task DomainEventDeferredObserverCanRecallReceivedEvents()
        {
            var entity = new TestEntity();
            var events = new List<DomainEvent>();
            Func<DomainEvent, Task> task = (domainEvent) =>
            {
                events.Add(domainEvent);
                return Task.CompletedTask;
            };
            var observer = new DomainEventDeferredObserver(entity, task);

            entity.PublishTestEvent();
            entity.PublishTestEvent();
            entity.PublishTestEvent();

            observer.DomainEvents.Count.Should().Be(3);

            observer.Dispose();

            observer.IsDisposed.Should().BeTrue();
            observer.DomainEvents.Count.Should().Be(0);
        }
    }
}