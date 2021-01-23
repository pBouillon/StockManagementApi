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

        public static IdentityResult Success()
            => new IdentityResult(true, Array.Empty<string>());

        public static IdentityResult Failure(IEnumerable<string> errors)
            => new IdentityResult(false, errors);
    }
}
