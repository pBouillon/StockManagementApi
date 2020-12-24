using System;
using Application.Commons.Interfaces;

namespace Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now
            => DateTime.Now;
    }
}
