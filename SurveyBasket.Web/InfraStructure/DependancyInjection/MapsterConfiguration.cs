using MapsterMapper;
using System.Reflection;

namespace SurveyBasket.Web.InfraStructure.DependancyInjection;

public static class MapsterConfiguration
{
    public static void AddMapster(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        // scans the assembly and gets the IRegister, adding the registration to the TypeAdapterConfig
        config.Scan(Assembly.GetExecutingAssembly());
        // register the mapper as Singleton service for my application
        var mapper = new Mapper(config);
        services.AddSingleton<IMapper>(mapper);
    }
}
