using System;
using System.Collections.Generic;
using FluentAssertions;
using Foundations.Core;
using Foundations.Core.Abstract;
using Foundations.Events;
using Xunit;

namespace Tests
{
    public class EntityTests
    {
        private class TestEventObserver : DomainEventObserver
        {
            public List<DomainEvent> Events { get; } = new List<DomainEvent>();

            public override void OnNext(DomainEvent value)
            {
                Events.Add(value);
            }
        }
        
        private class TestEvent : DomainEvent
        {
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
        }

        private class ComplexTextEntity : Entity<string>
        {
            public string AnotherAttribute { get; private set; }
            public ComplexTextEntity(string id, string another) : base(id) =>
                (AnotherAttribute) = (another);
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
        public void EntityCanPublishEventsToHandlerToTriggerEffects()
        {
            var observer = new TestEventObserver();
            var entity = new TestEntity();
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            entity.PublishTestEvent();
            observer.Events.Count.Should().Be(3);
        }

        [Fact]
        public void NoMoreEventsAreReceivedAfterObserverIsDisposed()
        {
            var observer = new TestEventObserver();
            var entity = new TestEntity();
            entity.PublishTestEvent();
            observer.Events.Count.Should().Be(1);
            observer.Dispose();

            entity.PublishTestEvent();
            observer.Events.Count.Should().Be(1);
        }
        
        [Fact]
        public void NoMoreEventsAreReceivedAfterPublisherIsCompleted()
        {
            var observer = new TestEventObserver();
            var entity = new TestEntity();
            entity.PublishTestEvent();
            observer.Events.Count.Should().Be(1);
            observer.OnCompleted();

            entity.PublishTestEvent();
            observer.Events.Count.Should().Be(1);
        }
        
        [Fact]
        public void NoMoreEventsAreReceivedAfterPublisherYieldsAnException()
        {
            var observer = new TestEventObserver();
            var entity = new TestEntity();
            entity.PublishTestEvent();
            observer.Events.Count.Should().Be(1);
            observer.OnError(new Exception());

            entity.PublishTestEvent();
            observer.Events.Count.Should().Be(1);
        }
    }
}
