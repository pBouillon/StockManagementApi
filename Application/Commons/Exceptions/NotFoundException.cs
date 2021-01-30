using System;

namespace Application.Commons.Exceptions
{
    /// <summary>
    /// Exception thrown when the application cannot find a resource
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NotFoundException() { }

        /// <summary>
        /// Create a new exception with a specific message
        /// </summary>
        /// <param name="message">Error message</param>
        public NotFoundException(string message)
            : base(message) { }

        /// <summary>
        /// Create a new exception from an existing one
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Exception to wrap</param>
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Create a new exception from the item name and the key used for the search
        /// </summary>
        /// <param name="name">Name of the entity</param>
        /// <param name="key">Key used for the search</param>
        public NotFoundException(string name, object key)
            : base($"Entity '{name}' ({key}) was not found") { }
    }
}
