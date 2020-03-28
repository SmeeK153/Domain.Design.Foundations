using System;
using FluentAssertions;
using Foundations.Exceptions;
using Xunit;

namespace Tests
{
    public class DomainExceptionTests
    {
        [Fact]
        public void DomainExceptionCanBeThrownWithMessage()
        {
            Action action = () => throw new DomainException("Message");
            action.Should().Throw<DomainException>().WithMessage("Message");
        }

        [Fact]
        public void DomainExceptionCanBeThrownWithInnerException()
        {
            Action action = () => throw new DomainException("Message", new Exception());
            action.Should().Throw<DomainException>().WithInnerException<Exception>();
            action.Should().Throw<DomainException>().WithMessage("Message");
        }
    }
}