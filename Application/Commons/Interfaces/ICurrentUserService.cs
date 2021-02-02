using System;

namespace Application.Commons.Interfaces
{
    /// <summary>
    /// Accessor for the current user's id
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Id of the current use
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// Flag to indicate whether or not a user is currently authenticated
        /// </summary>
        bool IsUserAuthenticated
            => UserId != Guid.Empty;
    }
}
