namespace MyRecipeBook.Domain.Entities
{
    public class EntityBase
    {
        public long Id { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }

    public class ReferenceEntityBase<T> where T : struct, Enum
    {
        public T Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class JunctionEntityBase
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
