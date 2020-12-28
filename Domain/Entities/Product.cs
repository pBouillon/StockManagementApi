using Domain.Commons;

namespace Domain.Entities
{
    /// <summary>
    /// Managed product
    /// </summary>
    public class Product : AuditableEntity
    {
        /// <summary>
        /// Maximum length for the product name
        /// </summary>
        public const int NameMaxLength = 200;

        /// <summary>
        /// If of the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string ToString()
            => $"Product {{ Id = { Id }, Name = { Name } }}";
    }
}
