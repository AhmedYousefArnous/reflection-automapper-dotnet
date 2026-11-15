using AutoMapper;
using AutoMapper.Internal;
using Microsoft.Extensions.DependencyInjection;
using ReflectionAutoMapperDemo.Dtos;
using ReflectionAutoMapperDemo.Entities;
using ReflectionAutoMapperDemo.Mapping;

// Register AutoMapper with the profile that scans this assembly for IMapFrom<T>.
var services = new ServiceCollection();
services.AddAutoMapper(typeof(MappingProfile));
using var provider = services.BuildServiceProvider();
var mapper = provider.GetRequiredService<IMapper>();

// The catch from the video: mistakes only show up at runtime.
// Validating the configuration at startup turns silent failures into an exception you see straight away.
mapper.ConfigurationProvider.AssertConfigurationIsValid();

Console.WriteLine("Maps registered by reflection:");
foreach (var map in mapper.ConfigurationProvider.Internal().GetAllTypeMaps()
             .OrderBy(m => m.SourceType.Name).ThenBy(m => m.DestinationType.Name))
    Console.WriteLine($"  {map.SourceType.Name} -> {map.DestinationType.Name}");

var user = new User { Id = 1, Name = "Ahmed", Email = "ahmed@example.com", PasswordHash = "secret-hash" };

var userDto = mapper.Map<UserDto>(user);
Console.WriteLine($"\nUser -> UserDto:  {userDto.Id}, {userDto.Name}, {userDto.Email}");

var back = mapper.Map<User>(userDto); // reverse map from the interface default
Console.WriteLine($"UserDto -> User:  {back.Id}, {back.Name}, {back.Email}, PasswordHash='{back.PasswordHash}'");

var order = new Order { Id = 42, Customer = user, Total = 199.50m, PlacedAt = DateTime.UtcNow };
var orderDto = mapper.Map<OrderDto>(order); // custom Mapping declared on OrderDto
Console.WriteLine($"Order -> OrderDto: {orderDto.Id}, {orderDto.CustomerName}, {orderDto.Total}, \"{orderDto.Summary}\"");
