using System.Collections.Generic;
using Domain.Design.Foundations.Core;
using FluentAssertions;
using Xunit;

namespace Tests.Foundations.Core
{
    public class ValueObjectTests
    {
        class TestValue : Value
        {
            public TestValue(string testValue1, bool? testBool1)
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

        class DifferentTestValue : Value
        {
            public DifferentTestValue(string differentValue1, bool differentBool1)
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
            var valueObject1 = new TestValue("test", true);
            var valueObject2 = new TestValue("test", true);
            valueObject1.Equals(valueObject2).Should().BeTrue();
            (valueObject1 == valueObject2).Should().BeTrue();

            var differentValueObject1 = new DifferentTestValue("differentTest", false);
            var differentValueObject2 = new DifferentTestValue("differentTest", false);
            differentValueObject1.Equals(differentValueObject2).Should().BeTrue();
            (differentValueObject1 == differentValueObject2).Should().BeTrue();
        }

        [Fact]
        public void ValueObjectsWithDifferentValuesAreResolvedAsDifferent()
        {
            var valueObject1 = new TestValue("test", true);
            var valueObject2 = new TestValue("something", false);
            var valueObject3 = new TestValue("something", null);
            valueObject1.Equals(valueObject2).Should().BeFalse();
            valueObject1.Equals(valueObject3).Should().BeFalse();
        }

        [Fact]
        public void ValueObjectWithNullPropertyIsResolvedAsDifferentWhenOtherIsNonNull()
        {
            var valueObject1 = new TestValue(null, true);
            var valueObject2 = new TestValue(null, null);
            valueObject1.Equals(valueObject2).Should().BeFalse();
        }
        
        [Fact]
        public void DifferentTypesValueObjectsAreResolvedAsDifferent()
        {
            var valueObject1 = new TestValue("test", true);
            var valueObject2 = new DifferentTestValue("test", true);
            valueObject1.Equals(valueObject2).Should().BeFalse();
            valueObject1.TestBool1.Equals(valueObject2.DifferentBool1).Should().BeTrue();
            valueObject1.TestValue1.Equals(valueObject2.DifferentValue1).Should().BeTrue();
        }

        [Fact]
        public void ComparingValueObjectToDifferentTypeResolvesAsDifferent()
        {
            var valueObject1 = new TestValue("test", true);
            var valueObject2 = new {testValue1 = "test", testBool1 = true};
            valueObject1.Equals(valueObject2).Should().BeFalse();
        }

        [Fact]
        public void ComparingValueObjectToNullResolvesAsDifferent()
        {
            var valueObject1 = new TestValue("test", true);
            var valueObject2 = (TestValue) null;
            valueObject1.Equals(valueObject2).Should().BeFalse();
            (valueObject1 == valueObject2).Should().BeFalse();
            (valueObject1 != valueObject2).Should().BeTrue();
            (null == valueObject1).Should().BeFalse();
            (null != valueObject1).Should().BeTrue();
        }
    }
}
