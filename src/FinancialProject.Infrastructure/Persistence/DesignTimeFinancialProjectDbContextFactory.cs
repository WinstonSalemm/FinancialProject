using FinancialProject.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FinancialProject.Infrastructure.Persistence;

public sealed class DesignTimeFinancialProjectDbContextFactory : IDesignTimeDbContextFactory<FinancialProjectDbContext>
{
    public FinancialProjectDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<FinancialProjectDbContext>();
        optionsBuilder.UseNpgsql(ConnectionStringFactory.BuildPostgresConnectionString(configuration), npgsql =>
        {
            npgsql.MigrationsAssembly(typeof(FinancialProjectDbContext).Assembly.FullName);
        });
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new FinancialProjectDbContext(optionsBuilder.Options, new NullCurrentTenantAccessor(), new NullCurrentUserAccessor());
    }

    private sealed class NullCurrentTenantAccessor : ICurrentTenantAccessor
    {
        public Guid? OrganizationId => null;
    }

    private sealed class NullCurrentUserAccessor : ICurrentUserAccessor
    {
        public Guid? UserId => Guid.Empty;
    }
}
