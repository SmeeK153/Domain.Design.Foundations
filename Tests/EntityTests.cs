using System;
using FluentAssertions;
using Foundations.Core;
using Xunit;

namespace Tests
{
    public class EntityTests
    {
        public class TestEntity : Entity<int>
        {
            public TestEntity(int id) : base(id)
            {
            }
        }

        public class ComplexTextEntity : Entity<string>
        {
            public string AnotherAttribute { get; private set; }
            public ComplexTextEntity(string id, string another) : base(id) =>
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
        }
    }
}
