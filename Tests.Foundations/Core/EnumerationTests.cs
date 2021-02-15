using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Design.Foundations.Core;
using Domain.Design.Foundations.Core.Abstract;
using FluentAssertions;
using Xunit;

namespace Tests.Foundations.Core
{
    public class TestEnumeration : Enumeration<int>
    {
        public static readonly TestEnumeration Enumeration1 = new TestEnumeration(1, "TestEnumeration1");
        public static readonly TestEnumeration Enumeration2 = new TestEnumeration(2, "TestEnumeration2");
        public static readonly TestEnumeration Enumeration3 = new TestEnumeration(3, "TestEnumeration3");
        
        public static TestEnumeration AutomaticPropertyEnumeration { get; } = new TestEnumeration(4, "AutomaticPropertyEnumeration");
        
        public static IEnumerable<TestEnumeration> AutomaticPropertyEnumerationList { get; } = new List<TestEnumeration>
        {
            new TestEnumeration(5, "ListEnumeration1"),
            new TestEnumeration(6, "ListEnumeration2"),
            new TestEnumeration(7, "ListEnumeration3"),
        };
        
        public static readonly IEnumerable<TestEnumeration> PropertyEnumerationList  = new List<TestEnumeration>
        {
            new TestEnumeration(8, "PropertyListEnumeration1"),
            new TestEnumeration(9, "PropertyListEnumeration2"),
            new TestEnumeration(10, "PropertyListEnumeration3"),
        };
        
        public static string Test { get; } = string.Empty;
        
        internal TestEnumeration(int id, string name) : base(id, name)
        {
        }
    }
    
    public class EnumerationTests
    {
        [Fact]
        public void EnumerationCanLookupAllOfItsTypes()
        {
            var enumerations = Enumeration<int>.GetAll<TestEnumeration>().ToList();
            enumerations.Should().NotBeNullOrEmpty();
            enumerations.Should().Contain(TestEnumeration.Enumeration1);
            enumerations.Should().Contain(TestEnumeration.Enumeration2);
            enumerations.Should().Contain(TestEnumeration.Enumeration3);
            enumerations.Should().Contain(TestEnumeration.AutomaticPropertyEnumeration);
            enumerations.Should().NotContain(new List<object>{TestEnumeration.Test});
            enumerations.Should().Contain(TestEnumeration.AutomaticPropertyEnumerationList);
            enumerations.Should().Contain(TestEnumeration.PropertyEnumerationList);
        }
        
        [Fact]
        public void LookupExistingEnumerationById()
        {
            Enumeration<int>.FromId<TestEnumeration>(1).Should().Be(TestEnumeration.Enumeration1);
        }

        [Fact]
        public void LookupNonexistentEnumerationByIdFails()
        {
            Action lookupInvalidEnumerationId = () => Enumeration<int>.FromId<TestEnumeration>(-1);
            lookupInvalidEnumerationId.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void LookupExistingEnumerationByName()
        {
            Enumeration<int>.FromName<TestEnumeration>("TestEnumeration1").Should().Be(TestEnumeration.Enumeration1);
        }

        [Fact]
        public void LookupNonexistentEnumerationByNameFails()
        {
            Action lookupInvalidEnumerationName = () => Enumeration<int>.FromName<TestEnumeration>("InvalidEnumerationName");
            lookupInvalidEnumerationName.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ComparingEqualEnumerationInstancesYieldsSuccessfulResult()
        {
            TestEnumeration.Enumeration1.Equals(TestEnumeration.Enumeration1).Should().BeTrue();
            (TestEnumeration.Enumeration1 == TestEnumeration.Enumeration1).Should().BeTrue();
        }

        [Fact]
        public void ComparingUnequalEnumerationInstancesYieldsFailureResult()
        {
            TestEnumeration.Enumeration1.Equals(TestEnumeration.Enumeration2).Should().BeFalse();
            (TestEnumeration.Enumeration1 == TestEnumeration.Enumeration2).Should().BeFalse();
        }
        
        [Fact]
        public void NullEnumerationDoesNotEqualNonNullEnumeration()
        {
            TestEnumeration.Enumeration1.Equals(null).Should().BeFalse();
            (TestEnumeration.Enumeration1 == null).Should().BeFalse();
        }

        [Fact]
        public void DifferentInstancesOfTheSameEnumerationAreEqual()
        {
            var test1 = new TestEnumeration(1, "test");
            var test2 = new TestEnumeration(1, "test");
            test1.Equals(test2).Should().BeTrue();
            (test1 == test2).Should().BeTrue();
        }

        [Fact]
        public void EnumerationToStringShouldSerializeItByName()
        {
            TestEnumeration.Enumeration1.ToString().Should().BeEquivalentTo("TestEnumeration1");
        }
    }
}
