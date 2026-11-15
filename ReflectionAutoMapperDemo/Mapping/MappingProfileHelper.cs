using System.Reflection;
using AutoMapper;

namespace ReflectionAutoMapperDemo.Mapping;

public static class MappingProfileHelper
{
    public static void ApplyMappingsFromAssembly(Assembly assembly, Profile profile)
    {
        // 1. Find every public type that implements the open generic IMapFrom<>.
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            // 2. Create an instance (the type needs a public parameterless constructor).
            var instance = Activator.CreateInstance(type);

            // 3. Use the type's own Mapping method if it declares one,
            //    otherwise fall back to the default method on the interface.
            var methodInfo = type.GetMethod("Mapping")
                ?? type.GetInterface("IMapFrom`1")!.GetMethod("Mapping");

            // 4. Invoke it with the profile, which registers the map.
            methodInfo?.Invoke(instance, new object[] { profile });
        }
    }
}
