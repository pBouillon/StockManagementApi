using System;

namespace Application.Commons.Interfaces
{
    /// <summary>
    /// DateTime service, providing methods to interact with the time
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Current <see cref="DateTime"/> accessor
        /// </summary>
        DateTime Now { get; }
    }
}
