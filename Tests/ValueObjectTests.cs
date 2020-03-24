using System;
using System.Collections.Generic;
using FluentAssertions;
using Foundations.Core;
using Xunit;

namespace Tests
{
    public class ValueObjectTests
    {
        class TestValueObject : ValueObject
        {
            public TestValueObject(string testValue1, bool testBool1)
            {
                TestValue1 = testValue1;
                TestBool1 = testBool1;
            }
            public string TestValue1 { get; }
            public bool TestBool1 { get; }
            protected override IEnumerable<object> GetComponentValues()
            {
                yield return TestValue1;
                yield return TestBool1;
            }
        }

        class DifferentTestValueObject : ValueObject
        {
            public DifferentTestValueObject(string differentValue1, bool differentBool1)
            {
                DifferentValue1 = differentValue1;
                DifferentBool1 = differentBool1;
            }
            public string DifferentValue1 { get; }
            public bool DifferentBool1 { get; }
            protected override IEnumerable<object> GetComponentValues()
            {
                yield return DifferentValue1;
                yield return DifferentBool1;
            }
        }
        
        [Fact]
        public void ValueObjectsWithSameValuesAreResolvedAsEqual()
        {
            var valueObject1 = new TestValueObject("test", true);
            var valueObject2 = new TestValueObject("test", true);
            valueObject1.Equals(valueObject2).Should().BeTrue();
        }

        [Fact]
        public void ValueObjectsWithDifferentValuesAreResolvedAsDifferent()
        {
            var valueObject1 = new TestValueObject("test", true);
            var valueObject2 = new TestValueObject("something", false);
            valueObject1.Equals(valueObject2).Should().BeTrue();
        }

        [Fact]
        public void DifferentTypesValueObjectsAreResolvedAsDifferent()
        {
            var valueObject1 = new TestValueObject("test", true);
            var valueObject2 = new DifferentTestValueObject("test", true);
            valueObject1.Equals(valueObject2).Should().BeFalse();
        }
    }
}
