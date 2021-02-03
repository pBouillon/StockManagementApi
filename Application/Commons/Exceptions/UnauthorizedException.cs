using System;

namespace Application.Commons.Exceptions
{
    /// <summary>
    /// Exception thrown when a user is attempting to perform an operation he has not the right to do
    /// </summary>
    public class UnauthorizedException : ApplicationException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UnauthorizedException() { }

        /// <summary>
        /// Create a new exception with a specific message
        /// </summary>
        /// <param name="message">Error message</param>
        public UnauthorizedException(string message)
            : base(message) { }

        /// <summary>
        /// Create a new exception from an existing one
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Exception to wrap</param>
        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Create a new exception from the id of the user attempting to perform the operation and the description
        /// of the operation
        /// </summary>
        /// <remarks>
        /// The structure is "The user of id XXX is not authorized to ...
        /// </remarks>
        /// <param name="originId">Id of the user attempting to perform the operation</param>
        /// <param name="attemptedOperationDescription">Key used for the search</param>
        public UnauthorizedException(Guid originId, string attemptedOperationDescription)
            : base($"The user of id {originId} is not authorized to {attemptedOperationDescription}") { }
    }
}
