using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Design.Foundations.Core;
using Domain.Design.Foundations.Core.Abstract;
using Domain.Design.Foundations.Events;
using FluentAssertions;
using Xunit;

namespace Tests.Foundations.Core
{
    public class EntityTests
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
            public TestEntity()
            {
            }
            
            public TestEntity(Guid id) : base(id)
            {
            }

            public TestEvent PublishTestEvent()
            {
                var testEvent = new TestEvent();
                PublishDomainEvent(testEvent);
                return testEvent;
            }

            public void PublishTestException() =>
                PublishDomainException(new TestException());
        }

        private class ComplexTextEntity : Entity<string>
        {
            public string AnotherAttribute { get; }
            public ComplexTextEntity(string id, string another) : base(id) =>
                (AnotherAttribute) = (another);
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
        public void EqualEntityIsResolvedAsEqual()
        {
            var guid = Guid.NewGuid();
            var entity = new TestEntity(guid);
            var compare = new TestEntity(guid);
            entity.Equals(compare).Should().BeTrue();
            (entity == compare).Should().BeTrue();
        }

        [Fact]
        public void DifferentEntityIsResolveAsUnequal()
        {
            var entity = new TestEntity();
            var compare = new TestEntity();
            entity.Equals(compare).Should().BeFalse();
            (entity == compare).Should().BeFalse();
            (entity != compare).Should().BeTrue();
        }

        [Fact]
        public void DifferentEntityTypesResolveAsUnequal()
        {
            var entity = new TestEntity();
            var complexEntity = new ComplexTextEntity("123", "another");
            entity.Equals(complexEntity).Should().BeFalse();
        }

        [Fact]
        public void EqualIdEntitiesWithDifferentAttributesAreResolvedAsEqual()
        {
            var complexEntity = new ComplexTextEntity("123", "another");
            var anotherComplexEntity = new ComplexTextEntity("123", "different");
            complexEntity.Equals(anotherComplexEntity).Should().BeTrue();
            (complexEntity == anotherComplexEntity).Should().BeTrue();
            (complexEntity != anotherComplexEntity).Should().BeFalse();
            complexEntity.AnotherAttribute.Equals(anotherComplexEntity.AnotherAttribute).Should().BeFalse();
        }

        [Fact]
        public void EntitiesWithDifferingValuesAndSameIdResolveToSameHash()
        {
            var complexEntityHash = new ComplexTextEntity("123", "another").GetHashCode();
            var anotherComplexEntityHash = new ComplexTextEntity("123", "different").GetHashCode();
            complexEntityHash.Should().Be(anotherComplexEntityHash);
        }
        
        [Fact]
        public void NullEntityDoesNotEqualNonNullEntity()
        {
            TestEntity entity = null;
            var compare = new TestEntity();
            (entity == compare).Should().BeFalse();
            (entity != compare).Should().BeTrue();
        }

        [Fact]
        public async Task EntityCanPublishEventsToHandlerToTriggerEffects()
        {
            var entity = new TestEntity();
            var manager = new TestDomainEventManager();
            await manager.StartListening(entity);
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            await manager.ExecuteEvents();
            manager.DomainEvents.Count.Should().Be(3);
        }

        [Fact]
        public async Task NoMoreEventsAreReceivedAfterEventsAreExecuted()
        {
            var entity = new TestEntity();
            var manager = new TestDomainEventManager();
            await manager.StartListening(entity);
            entity.PublishTestEvent();
            await manager.ExecuteEvents();
            manager.DomainEvents.Count.Should().Be(1);

            entity.PublishTestEvent();
            manager.DomainEvents.Count.Should().Be(1);
        }

        [Fact]
        public async Task NoEventsAreReceivedAfterPublisherYieldsAnException()
        {
            var entity = new TestEntity();
            var manager = new TestDomainEventManager();
            await manager.StartListening(entity);
            entity.PublishTestEvent();
            Action action = () => entity.PublishTestException();
            action.Should().Throw<TestException>();

            await manager.ExecuteEvents();
            manager.DomainEvents.Count.Should().Be(0);

            entity.PublishTestEvent();
            manager.DomainEvents.Count.Should().Be(0);
        }
    }
}
