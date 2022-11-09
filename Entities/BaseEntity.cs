class BaseEntity
{
    public Guid Id {get;set;}
    public DateTime CreatedAt = DateTime.UtcNow;
}