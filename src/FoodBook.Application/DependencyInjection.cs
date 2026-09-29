using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FoodBook.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
