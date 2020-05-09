using System;
using Domain.Design.Foundations.Exceptions;
using FluentAssertions;
using Xunit;

namespace Tests.Foundations
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