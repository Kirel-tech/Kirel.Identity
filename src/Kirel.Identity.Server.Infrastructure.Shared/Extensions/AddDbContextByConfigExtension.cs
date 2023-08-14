using Kirel.Identity.Server.Infrastructure.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kirel.Identity.Server.Infrastructure.Shared.Extensions;

/// <summary>
/// Extension for IServiceCollection with different databases contexts support by config
/// </summary>
public static class AddDbContextByConfigExtension
{
    /// <summary>
    /// Adds application context based on configuration file
    /// </summary>
    /// <param name="services"> Specifies the contract for a collection of service descriptors. </param>
    /// <param name="dbConfig"> Class that represents data base configuration. </param>
    /// <exception cref="Exception"> If chosen database driver is unsupported. </exception>
    /// <typeparam name="TBaseContext"> Type of the base db context. </typeparam>
    /// <typeparam name="TMssqlContext"> Type of the mssql db context, must be based on TBaseContext. </typeparam>
    /// <typeparam name="TPostgresqlContext"> Type of the postgresql db context, must be based on TBaseContext. </typeparam>
    /// <typeparam name="TMysqlContext"> Type of the mysql db context, must be based on TBaseContext. </typeparam>
    /// <typeparam name="TSqliteContext"> Type of the sqlite db context, must be based on TBaseContext. </typeparam>
    public static void AddDbContextByConfig<TBaseContext, TMssqlContext, TPostgresqlContext, TMysqlContext,
        TSqliteContext>(this IServiceCollection services, DbConfig dbConfig)
        where TMssqlContext : DbContext, TBaseContext
        where TPostgresqlContext : DbContext, TBaseContext
        where TMysqlContext : DbContext, TBaseContext
        where TSqliteContext : DbContext, TBaseContext
        where TBaseContext : DbContext
    {
        Action<DbContextOptionsBuilder> contextOptions = dbConfig.Driver switch
        {
            "sqlite" => builder => builder.UseSqlite(GetSqliteConnectionString(dbConfig)),
            "mssql" => builder => builder.UseSqlServer(GetMssqlConnectionString(dbConfig)),
            "postgresql" => builder => builder.UseNpgsql(GetPostgresqlConnectionString(dbConfig)),
            "mysql" => builder =>
            {
                var connectionString = GetMysqlConnectionString(dbConfig);
                var version = ServerVersion.AutoDetect(connectionString);
                builder.UseMySql(connectionString, version);
            },
            _ => throw new Exception("Unsupported database driver. Use one of this mssql/postgresql/mysql")
        };

        switch (dbConfig.Driver)
        {
            case "sqlite":
                services.AddDbContext<TBaseContext, TSqliteContext>(contextOptions);
                break;
            case "mysql":
                services.AddDbContext<TBaseContext, TMysqlContext>(contextOptions);
                break;
            case "postgresql":
                services.AddDbContext<TBaseContext, TPostgresqlContext>(contextOptions);
                break;
            case "mssql":
                services.AddDbContext<TBaseContext, TMssqlContext>(contextOptions);
                break;
        }
    }

    private static string GetMysqlConnectionString(DbConfig dbConfig)
    {
        return $"Server={dbConfig.Params.Address};" +
               $"Port={dbConfig.Params.Port};" +
               $"Database={dbConfig.Params.DatabaseName};" +
               $"Uid={dbConfig.Params.User};" +
               $"Pwd={dbConfig.Params.Password};";
    }

    private static string GetSqliteConnectionString(DbConfig dbConfig)
    {
        return $"Filename={dbConfig.Params.DatabaseName}.db";
    }

    private static string GetMssqlConnectionString(DbConfig dbConfig)
    {
        return $"Server={dbConfig.Params.Address},{dbConfig.Params.Port};" +
               $"Database={dbConfig.Params.DatabaseName};" +
               $"User Id={dbConfig.Params.User};" +
               $"Password={dbConfig.Params.Password};";
    }

    private static string GetPostgresqlConnectionString(DbConfig dbConfig)
    {
        return $"Server={dbConfig.Params.Address};" +
               $"Port={dbConfig.Params.Port};" +
               $"Database={dbConfig.Params.DatabaseName};" +
               $"User Id={dbConfig.Params.User};" +
               $"Password={dbConfig.Params.Password};";
    }
}