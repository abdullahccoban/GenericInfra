using GenericInfra.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericInfra.Extensions;

public static class UnitOfWorkServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        return services;
    }
    
}