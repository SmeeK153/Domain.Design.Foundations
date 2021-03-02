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
        private class TestEntity : Entity
        {
            public TestEntity()
            {
            }
            
            public TestEntity(Guid id) : base(id)
            {
            }
        }

        private class ComplexTextEntity : Entity<string>
        {
            public string AnotherAttribute { get; }
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
    }
}
