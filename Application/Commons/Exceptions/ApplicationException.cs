using System;

namespace Application.Commons.Exceptions
{
    /// <summary>
    /// Base class for all application-specific exception
    /// </summary>
    public abstract class ApplicationException : Exception
    {
        /// <summary>
        /// Create a new exception
        /// </summary>
        protected ApplicationException() { }

        /// <summary>
        /// Create a new exception with a specific message
        /// </summary>
        /// <param name="message">Error message</param>
        protected ApplicationException(string message)
            : base(message) { }

        /// <summary>
        /// Create a new exception from an existing one
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Exception to wrap</param>
        protected ApplicationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
