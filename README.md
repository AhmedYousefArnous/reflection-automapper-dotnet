# Reflection + AutoMapper in .NET

Companion code for **.NET Tips and Tricks #3** on the Ahmed Arnous YouTube channel, and the article
[Unlocking Dynamic Object Mapping: How Reflection Powers AutoMapper](https://medium.com/@ahmedyousefarnous/unlocking-dynamic-object-mapping-how-reflection-powers-automapper-e12c46884874).

Stop writing a `CreateMap` line for every DTO. Each DTO declares its own mapping with `IMapFrom<T>`,
and reflection finds and registers them all at startup.

## Run it

```bash
dotnet run --project ReflectionAutoMapperDemo
```

Output:

```
Maps registered by reflection:
  Order -> OrderDto
  User -> UserDto
  UserDto -> User

User -> UserDto:  1, Ahmed, ahmed@example.com
UserDto -> User:  1, Ahmed, ahmed@example.com, PasswordHash=''
Order -> OrderDto: 42, Ahmed, 199.50, "Order #42 for Ahmed: 199.50"
```

Requires the .NET 10 SDK.

## How it works

| Step | File | What it does |
| --- | --- | --- |
| 1 | `Mapping/IMapFrom.cs` | Generic interface whose default `Mapping` method creates `T -> DTO` plus the reverse map. |
| 2 | `Mapping/MappingProfileHelper.cs` | `GetExportedTypes()` finds every public type that implements `IMapFrom<>`. |
| 3 | `Mapping/MappingProfileHelper.cs` | `Activator.CreateInstance` creates each one; `Mapping` is taken from the type, or from the interface default; `Invoke` registers it. |
| 4 | `Mapping/MappingProfile.cs`, `Program.cs` | The profile runs the helper on the executing assembly; `AddAutoMapper(typeof(MappingProfile))` registers it. |

Two DTOs show both paths:

- `UserDto : IMapFrom<User>` uses the interface default (map + reverse map).
- `OrderDto : IMapFrom<Order>` declares its own `public void Mapping(Profile profile)` with a `ForMember`, so the helper calls that one instead.

Add a new DTO, implement `IMapFrom<YourEntity>`, and it is mapped. No profile edits.

## The catch

Everything is matched by name at runtime. Rename a property or change a type and the code still compiles;
a map can be skipped silently. `Program.cs` calls
`mapper.ConfigurationProvider.AssertConfigurationIsValid()` at startup, so an unmapped member throws straight
away instead of showing up later as missing data.

Also keep in mind:

- Small, one-off mappings are often easier to read as explicit code.
- Library updates can bring breaking changes.
- `Activator.CreateInstance` needs a public parameterless constructor on each DTO.

## AutoMapper version

The project pins **AutoMapper 14.0.0**, the last MIT-licensed release. From version 15, AutoMapper is
commercially licensed and its `AddAutoMapper` registration changed; check the current licence terms before
upgrading.
