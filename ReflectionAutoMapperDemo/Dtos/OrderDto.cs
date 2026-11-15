using AutoMapper;
using ReflectionAutoMapperDemo.Entities;
using ReflectionAutoMapperDemo.Mapping;

namespace ReflectionAutoMapperDemo.Dtos;

// Declares its own Mapping method, so the helper calls this one instead of the interface default.
public class OrderDto : IMapFrom<Order>
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = ""; // flattened from Customer.Name by convention
    public decimal Total { get; set; }
    public string Summary { get; set; } = "";       // custom member

    public void Mapping(Profile profile) =>
        profile.CreateMap<Order, OrderDto>()
               .ForMember(d => d.Summary,
                          o => o.MapFrom(s => $"Order #{s.Id} for {s.Customer.Name}: {s.Total:0.00}"));
}
