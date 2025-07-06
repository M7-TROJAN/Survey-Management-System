using MapsterMapper;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;

namespace SurveyBasket.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddDependecies(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddOpenApi();

        services.AddMapsterConfig() // Register Mapster configuration
            .AddFluentValidationConfig(); // Register FluentValidation configuration

        services.AddScoped<IPollService, PollService>();

        return services;
    }

    public static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        // scans the assembly and gets the IRegister, adding the registration to the TypeAdapterConfig
        config.Scan(Assembly.GetExecutingAssembly());
        // register the mapper as Singleton service for my application
        var mapper = new Mapper(config);
        services.AddSingleton<IMapper>(mapper);

        return services;
    }

    public static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        // Add FluentValidation configuration
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}