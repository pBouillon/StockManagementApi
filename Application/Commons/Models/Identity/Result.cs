using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Commons.Models.Identity
{
    /// <summary>
    /// Wrapper for the result of the IdentityServer's operations
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Flag indicating whether this operation succeeded or not
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// A collection of the errors that might have occurred
        /// </summary>
        public string[] Errors { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="succeeded">Flag indicating whether this operation succeeded or not</param>
        /// <param name="errors">A collection of the errors that might have occurred</param>
        internal Result(bool succeeded, IEnumerable<string> errors)
            => (Succeeded, Errors) = (succeeded, errors.ToArray());

        /// <summary>
        /// Create a new <see cref="Result"/> indicating that the operation failed
        /// </summary>
        /// <param name="errors">Causes of the failure</param>
        /// <returns>The <see cref="Result"/> indicating that the operation failed</returns>
        public static Result Failure(IEnumerable<string> errors)
            => new Result(false, errors);

        /// <summary>
        /// Create a new <see cref="Result"/> indicating that the operation succeeded
        /// </summary>
        /// <returns>The <see cref="Result"/> indicating that the operation succeeded</returns>
        public static Result Success()
            => new Result(true, Array.Empty<string>());
    }

    /// <summary>
    /// Wrapper for the result of the IdentityServer's operations, holds a payload
    /// </summary>
    /// <typeparam name="TPayload">Type of the response's payload</typeparam>
    public class Result<TPayload>
        where TPayload : class
    {
        /// <summary>
        /// Flag indicating whether this operation succeeded or not
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// A collection of the errors that might have occurred
        /// </summary>
        public string[] Errors { get; }

        /// <summary>
        /// The operation's response, might be null
        /// </summary>
        public TPayload? Payload { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="succeeded">Flag indicating whether this operation succeeded or not</param>
        /// <param name="errors">A collection of the errors that might have occurred</param>
        /// <param name="payload">The operation's response</param>
        internal Result(bool succeeded, IEnumerable<string> errors, TPayload? payload)
            => (Succeeded, Errors, Payload) = (succeeded, errors.ToArray(), payload);

        /// <summary>
        /// Create a new <see cref="Result"/> indicating that the operation failed
        /// </summary>
        /// <param name="errors">Causes of the failure</param>
        /// <returns>The <see cref="Result"/> indicating that the operation failed</returns>
        public static Result<TPayload> Failure(IEnumerable<string> errors)
            => new Result<TPayload>(false, errors, default);

        /// <summary>
        /// Create a new <see cref="Result"/> indicating that the operation succeeded
        /// </summary>
        /// <typeparam name="TPayload">Type of the response's payload</typeparam>
        /// <param name="payload">The operation's response</param>
        /// <returns>The <see cref="Result"/> indicating that the operation succeeded</returns>
        public static Result<TPayload> Success(TPayload payload)
            => new Result<TPayload>(true, Array.Empty<string>(), payload);
    }
}
