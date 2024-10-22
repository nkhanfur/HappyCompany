namespace WarehouseManagement.API.Application.Extensions;

public static class MapperExtensions
{
    public static IServiceCollection AddSmartMapper(this IServiceCollection services, Type profileAssemblyType)
    {
        var assembly = profileAssemblyType.Assembly;
        var types = assembly.GetTypes().Where(t => string.Equals(t.Namespace, typeof(Mappers.WarehouseMapper).Namespace, StringComparison.Ordinal)).ToArray();
        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            if (interfaces.Length == 0)
            {
                continue;
            }

            services.AddSingleton(interfaces[0], type);
        }

        return services;
    }
}
