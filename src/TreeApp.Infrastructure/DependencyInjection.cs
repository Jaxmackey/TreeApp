using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TreeApp.Domain.Interfaces;
using TreeApp.Infrastructure.Data;
using TreeApp.Infrastructure.Repositories;

namespace TreeApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<INodeRepository, NodeRepository>();
        services.AddScoped<IJournalRepository, JournalRepository>();

        return services;
    }
}