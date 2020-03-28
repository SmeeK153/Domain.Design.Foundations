using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Common;
using Foundations.Core;
using Foundations.Events;
using Xunit;

namespace Tests
{
    public class EntityTests
    {
        private class TestEvent : DomainEvent
        {
        }

        private static readonly Action<DomainEvent> DummyAction = new List<DomainEvent>().Add;
        
        private class TestEntity : Entity<int>
        {
            public TestEntity(int id, Action<DomainEvent> testPublisher = null) :
                base(id, testPublisher ?? DummyAction)
            {
            }

            public TestEvent AddTestEvent()
            {
                var testEvent = new TestEvent();
                AddDomainEvent(testEvent);
                return testEvent;
            }

            public void RemoveTestEvent(TestEvent testEvent)
            {
                RemoveDomainEvent(testEvent);
            }

            public void RemoveAllTestEvents() => ClearDomainEvents();
        }

        private class ComplexTextEntity : Entity<string>
        {
            public string AnotherAttribute { get; private set; }
            public ComplexTextEntity(string id, string another, Action<DomainEvent> testPublisher = null) : 
                base(id, testPublisher ?? DummyAction) =>
                (AnotherAttribute) = (another);
        }

        [Fact]
        public void EqualEntityIsResolvedAsEqual()
        {
            var entity = new TestEntity(123);
            var compare = new TestEntity(123);
            entity.Equals(compare).Should().BeTrue();
            (entity == compare).Should().BeTrue();
        }

        [Fact]
        public void DifferentEntityIsResolveAsUnequal()
        {
            var entity = new TestEntity(123);
            var compare = new TestEntity(789);
            entity.Equals(compare).Should().BeFalse();
            (entity == compare).Should().BeFalse();
            (entity != compare).Should().BeTrue();
        }

        [Fact]
        public void DifferentEntityTypesResolveAsUnequal()
        {
            var entity = new TestEntity(123);
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
            var compare = new TestEntity(789);
            (entity == compare).Should().BeFalse();
            (entity != compare).Should().BeTrue();
        }

        [Fact]
        public void EntityCanPublishEventsToHandlerToTriggerEffects()
        {
            var testEvents = new List<DomainEvent>();
            void Action(DomainEvent e) => testEvents.Add(e);
            var entity = new TestEntity(123, Action);
            entity.AddTestEvent();
            entity.AddTestEvent();
            entity.AddTestEvent();
            
            entity.PublishDomainEvents();
            testEvents.Count.Should().Be(3);
            
            testEvents.Clear();
            entity.PublishDomainEvents();
            testEvents.Count.Should().Be(3);
        }

        [Fact]
        public void EntityCanAddAndRemoveEvents()
        {
            var testEvents = new List<DomainEvent>();
            void Action(DomainEvent e) => testEvents.Add(e);
            var entity = new TestEntity(123, Action);
            var testEvent = entity.AddTestEvent();
            entity.AddTestEvent();
            entity.AddTestEvent();
            
            entity.PublishDomainEvents();
            testEvents.Count.Should().Be(3);
            
            testEvents.Clear();
            entity.RemoveTestEvent(testEvent);
            entity.PublishDomainEvents();
            testEvents.Count.Should().Be(2);
            
            testEvents.Clear();
            entity.RemoveAllTestEvents();
            entity.PublishDomainEvents();
            testEvents.Count.Should().Be(0);
        }
    }
}
