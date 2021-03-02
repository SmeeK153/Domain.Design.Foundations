using System;

namespace Domain.Design.Foundations.Events
{
    /// <summary>
    /// Represents errors that occur during domain execution.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">Specified error message</param>
        public DomainException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message and a
        /// reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">Specified error message</param>
        /// <param name="innerException">Cause of this exception</param>
        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}