using AutoMapper;

namespace ReflectionAutoMapperDemo.Mapping;

/// <summary>
/// Implement on a DTO to declare where it maps from.
/// The default method creates a map from T to the implementing type, plus the reverse map.
/// Declare your own public Mapping method on the DTO to customise it.
/// </summary>
public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
}
