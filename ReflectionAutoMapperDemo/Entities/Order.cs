namespace ReflectionAutoMapperDemo.Entities;

public class Order
{
    public int Id { get; set; }
    public User Customer { get; set; } = new();
    public decimal Total { get; set; }
    public DateTime PlacedAt { get; set; }
}
