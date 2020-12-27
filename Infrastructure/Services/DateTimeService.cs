using System;
using Application.Commons.Interfaces;

namespace Infrastructure.Services
{
    /// <inheritdoc cref="IDateTime"/>
    public class DateTimeService : IDateTime
    {
        /// <inheritdoc cref="IDateTime.Now"/>
        public DateTime Now
            => DateTime.Now;
    }
}
