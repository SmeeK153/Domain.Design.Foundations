using System;
using Domain.Design.Foundations.Events;
using FluentAssertions;
using Xunit;

namespace Tests.Foundations.Core
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