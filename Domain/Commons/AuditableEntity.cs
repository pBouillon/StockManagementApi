using System;

namespace Domain.Commons
{
    public class AuditableEntity
    {
        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
