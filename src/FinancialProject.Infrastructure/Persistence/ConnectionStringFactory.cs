using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FinancialProject.Infrastructure.Persistence;

internal static class ConnectionStringFactory
{
    public static string BuildPostgresConnectionString(IConfiguration configuration)
    {
        var explicitConnection = configuration.GetConnectionString("Postgres")
            ?? configuration["Database:ConnectionString"];

        if (!string.IsNullOrWhiteSpace(explicitConnection))
        {
            return explicitConnection;
        }

        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL")
            ?? Environment.GetEnvironmentVariable("DATABASE_PRIVATE_URL")
            ?? Environment.GetEnvironmentVariable("DATABASE_PUBLIC_URL");

        if (!string.IsNullOrWhiteSpace(databaseUrl))
        {
            return FromDatabaseUrl(databaseUrl);
        }

        var host = Environment.GetEnvironmentVariable("PGHOST");
        var port = Environment.GetEnvironmentVariable("PGPORT");
        var username = Environment.GetEnvironmentVariable("PGUSER");
        var password = Environment.GetEnvironmentVariable("PGPASSWORD");
        var database = Environment.GetEnvironmentVariable("PGDATABASE");

        if (!string.IsNullOrWhiteSpace(host)
            && !string.IsNullOrWhiteSpace(port)
            && !string.IsNullOrWhiteSpace(username)
            && !string.IsNullOrWhiteSpace(password)
            && !string.IsNullOrWhiteSpace(database))
        {
            return new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = int.Parse(port),
                Username = username,
                Password = password,
                Database = database,
                SslMode = SslMode.Prefer
            }.ConnectionString;
        }

        return "Host=localhost;Port=5432;Database=financial_project;Username=postgres;Password=postgres;Ssl Mode=Disable";
    }

    private static string FromDatabaseUrl(string databaseUrl)
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2, StringSplitOptions.TrimEntries);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Username = userInfo.ElementAtOrDefault(0) ?? string.Empty,
            Password = userInfo.ElementAtOrDefault(1) ?? string.Empty,
            Database = uri.AbsolutePath.Trim('/'),
            SslMode = SslMode.Prefer
        };

        if (!string.IsNullOrWhiteSpace(uri.Query))
        {
            var query = uri.Query.TrimStart('?')
                .Split('&', StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split('=', 2))
                .ToDictionary(
                    parts => Uri.UnescapeDataString(parts[0]),
                    parts => parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : string.Empty,
                    StringComparer.OrdinalIgnoreCase);

            if (query.TryGetValue("sslmode", out var sslMode) && Enum.TryParse<SslMode>(sslMode, true, out var parsedMode))
            {
                builder.SslMode = parsedMode;
            }
        }

        return builder.ConnectionString;
    }
}
