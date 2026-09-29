using FoodBook.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodBook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddDbContext<FoodBookDbContext>(options =>
            options.UseInMemoryDatabase("FoodBookDb"));

        return services;
    }
}
