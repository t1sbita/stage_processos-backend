using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StageProcessos.Application.Services;
using StageProcessos.Domain.Interfaces;
using StageProcessos.Infrastructure.Context;
using StageProcessos.Infrastructure.Data.Repositories;
using StageProcessos.Infrastructure.Data.Repositories.Base;

namespace StageProcessos.IoC.IoC;

public static class DependencyInjections
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPersistence(configuration)
            .AddRepositories()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection.AddDbContext<StageProcessosContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IProcessRepository, ProcessRepository>();

        return services;
    }
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IProcessService, ProcessService>();

        return services;
    }
}
