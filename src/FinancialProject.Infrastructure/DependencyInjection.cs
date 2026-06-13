using FinancialProject.Application.Abstractions;
using FinancialProject.Infrastructure.Persistence;
using FinancialProject.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialProject.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentTenantAccessor, HttpCurrentTenantAccessor>();
        services.AddScoped<ICurrentUserAccessor, HttpCurrentUserAccessor>();

        var connectionString = ConnectionStringFactory.BuildPostgresConnectionString(configuration);

        services.AddDbContext<FinancialProjectDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(FinancialProjectDbContext).Assembly.FullName);
                npgsql.EnableRetryOnFailure(5);
            });
            options.UseSnakeCaseNamingConvention();
        });

        services.AddHealthChecks().AddDbContextCheck<FinancialProjectDbContext>("postgres");

        return services;
    }
}
