using Domain.Commons;

namespace Domain.Entities
{
    public class Product : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
