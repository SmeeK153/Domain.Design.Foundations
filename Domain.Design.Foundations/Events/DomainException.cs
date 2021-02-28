using System;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}