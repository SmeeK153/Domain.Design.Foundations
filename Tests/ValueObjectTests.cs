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
            public TestValueObject(string testValue1, bool? testBool1)
            {
                TestValue1 = testValue1;
                TestBool1 = testBool1;
            }
            public string TestValue1 { get; }
            public bool? TestBool1 { get; }
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
            (valueObject1 == valueObject2).Should().BeTrue();

            var differentValueObject1 = new DifferentTestValueObject("differentTest", false);
            var differentValueObject2 = new DifferentTestValueObject("differentTest", false);
            differentValueObject1.Equals(differentValueObject2).Should().BeTrue();
            (differentValueObject1 == differentValueObject2).Should().BeTrue();
        }

        [Fact]
        public void ValueObjectsWithDifferentValuesAreResolvedAsDifferent()
        {
            var valueObject1 = new TestValueObject("test", true);
            var valueObject2 = new TestValueObject("something", false);
            var valueObject3 = new TestValueObject("something", null);
            valueObject1.Equals(valueObject2).Should().BeFalse();
            valueObject1.Equals(valueObject3).Should().BeFalse();
        }

        [Fact]
        public void ValueObjectWithNullPropertyIsResolvedAsDifferentWhenOtherIsNonNull()
        {
            var valueObject1 = new TestValueObject(null, true);
            var valueObject2 = new TestValueObject(null, null);
            valueObject1.Equals(valueObject2).Should().BeFalse();
        }
        
        [Fact]
        public void DifferentTypesValueObjectsAreResolvedAsDifferent()
        {
            var valueObject1 = new TestValueObject("test", true);
            var valueObject2 = new DifferentTestValueObject("test", true);
            valueObject1.Equals(valueObject2).Should().BeFalse();
            valueObject1.TestBool1.Equals(valueObject2.DifferentBool1).Should().BeTrue();
            valueObject1.TestValue1.Equals(valueObject2.DifferentValue1).Should().BeTrue();
        }

        [Fact]
        public void ComparingValueObjectToDifferentTypeResolvesAsDifferent()
        {
            var valueObject1 = new TestValueObject("test", true);
            var valueObject2 = new {testValue1 = "test", testBool1 = true};
            valueObject1.Equals(valueObject2).Should().BeFalse();
        }

        [Fact]
        public void ComparingValueObjectToNullResolvesAsDifferent()
        {
            var valueObject1 = new TestValueObject("test", true);
            var valueObject2 = (TestValueObject) null;
            valueObject1.Equals(valueObject2).Should().BeFalse();
            (valueObject1 == valueObject2).Should().BeFalse();
            (valueObject1 != valueObject2).Should().BeTrue();
            (null == valueObject1).Should().BeFalse();
            (null != valueObject1).Should().BeTrue();
        }
    }
}
