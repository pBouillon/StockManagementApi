using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Commons.Models
{
    /// <summary>
    /// TODO
    /// </summary>
    public class IdentityResult
    {
        public bool Succeeded { get; }

        public string[] Errors { get; }

        internal IdentityResult(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }
        public static IdentityResult Failure(IEnumerable<string> errors)
            => new IdentityResult(false, errors);

        public static IdentityResult Success()
            => new IdentityResult(true, Array.Empty<string>());

    }

    /// <summary>
    /// TODO
    /// </summary>
    public class IdentityResult<TPayload>
    {
        public bool Succeeded { get; }

        public string[] Errors { get; }

        public TPayload Payload { get; }

        internal IdentityResult(bool succeeded, IEnumerable<string> errors, TPayload payload)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Payload = payload;
        }

        public IdentityResult AsIdentityResult()
            => new IdentityResult(Succeeded, Errors);

        public static IdentityResult<TPayload> Failure(IEnumerable<string> errors)
            => new IdentityResult<TPayload>(false, errors, default!);

        public static IdentityResult<TPayload> Success(TPayload payload)
            => new IdentityResult<TPayload>(true, Array.Empty<string>(), payload);
    }
}
