using Microsoft.OpenApi.Models;

namespace FoodBook.WebApi.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddWebApiLayer(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "FoodBook API",
                Version = "v1",
                Description = "Onion architecture, EF Core in-memory."
            });
        });

        return services;
    }
}
