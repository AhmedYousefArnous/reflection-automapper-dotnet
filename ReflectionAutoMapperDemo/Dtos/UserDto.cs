using ReflectionAutoMapperDemo.Entities;
using ReflectionAutoMapperDemo.Mapping;

namespace ReflectionAutoMapperDemo.Dtos;

// Uses the default Mapping from the interface: User <-> UserDto, matched by name and type.
public class UserDto : IMapFrom<User>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}
