using System;

namespace Domain.Commons
{
    /// <summary>
    /// Base class representing an auditable class with a creation date and the last time it has been updated
    /// </summary>
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Date on which this entity has been created
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Date on which this entity has been updated for th last time
        /// </summary>
        public DateTime? LastModifiedOn { get; set; }
    }
}
