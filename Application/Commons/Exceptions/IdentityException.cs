using Application.Commons.Models;
using System;
using System.Collections.Generic;
using Application.Authentication.Dtos;

namespace Application.Commons.Exceptions
{
    /// <summary>
    /// Exception thrown when an error related to Identity occurs
    /// </summary>
    public class IdentityException : ApplicationException
    {
        /// <summary>
        /// List of all the errors causing this exception
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
        /// Create a new exception with a specific message from the response of a failing Identity operation
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="errors">Errors causing this exception</param>
        public IdentityException(string message, string[] errors)
            : base(message)
            => Errors = errors;
    }
}
