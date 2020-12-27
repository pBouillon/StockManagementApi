using System;

namespace Domain.Commons
{
    public class AuditableEntity
    {
        public DateTime CreatedOn { get; set; }

        public DateTime? LastModifiedOn { get; set; }
    }
}
