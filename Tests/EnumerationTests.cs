using System;
using FluentAssertions;
using Foundations.Core;
using Xunit;

namespace Tests
{
    public class EnumerationTests
    {
        class TestEnumeration : Enumeration
        {
            public static TestEnumeration Enumeration1 { get; } = new TestEnumeration(1, "TestEnumeration1");
            public static TestEnumeration Enumeration2 { get; } = new TestEnumeration(2, "TestEnumeration2");
            public static TestEnumeration Enumeration3 { get; } = new TestEnumeration(3, "TestEnumeration3");
            protected TestEnumeration(int id, string name) : base(id, name)
            {
            }
        }
        
        [Fact]
        public void LookupExistingEnumerationById()
        {
            Enumeration.FromId<TestEnumeration>(1).Should().Be(TestEnumeration.Enumeration1);
        }

        [Fact]
        public void LookupNonexistentEnumerationByIdFails()
        {
            Action lookupInvalidEnumerationId = () => Enumeration.FromId<TestEnumeration>(-1);
            lookupInvalidEnumerationId.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void LookupExistingEnumerationByName()
        {
            Enumeration.FromName<TestEnumeration>("TestEnumeration1").Should().Be(TestEnumeration.Enumeration1);
        }

        [Fact]
        public void LookupNonexistentEnumerationByNameFails()
        {
            Action lookupInvalidEnumerationName = () => Enumeration.FromName<TestEnumeration>("InvalidEnumerationName");
            lookupInvalidEnumerationName.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ComparingEqualEnumerationInstancesYieldsSuccessfulResult()
        {
            TestEnumeration.Enumeration1.Equals(TestEnumeration.Enumeration1).Should().BeTrue();
        }

        [Fact]
        public void ComparingUnequalEnumerationInstancesYieldsFailureResult()
        {
            TestEnumeration.Enumeration1.Equals(TestEnumeration.Enumeration2).Should().BeFalse();
        }
    }
}
