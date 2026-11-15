using System.Reflection;
using AutoMapper;

namespace ReflectionAutoMapperDemo.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        MappingProfileHelper.ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly(), this);
    }
}
