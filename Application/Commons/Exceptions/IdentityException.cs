using System;
using System.Collections.Generic;
using Application.Commons.Models;

namespace Application.Commons.Exceptions
{
    /// <summary>
    /// TODO
    /// </summary>
    public class IdentityException : ApplicationException
    {
        /// <summary>
        /// TODO
        /// </summary>
        public IEnumerable<string> Errors { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public IdentityException()
            => Errors = Array.Empty<string>();

        /// <summary>
        /// Create a new exception with a specific message
        /// </summary>
        /// <param name="message">Error message</param>
        public IdentityException(string message)
            : base(message)
            => Errors = Array.Empty<string>();

        /// <summary>
        /// Create a new exception from an existing one
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Exception to wrap</param>
        public IdentityException(string message, Exception innerException)
            : base(message, innerException)
            => Errors = Array.Empty<string>();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="message"></param>
        /// <param name="identityResult"></param>
        public IdentityException(string message, IdentityResult identityResult)
            : base(message)
            => Errors = identityResult.Errors;
    }
}
