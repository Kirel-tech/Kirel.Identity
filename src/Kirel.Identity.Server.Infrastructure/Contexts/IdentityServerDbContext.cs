using Kirel.Identity.Core.Context;
using Kirel.Identity.Server.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kirel.Identity.Server.Infrastructure.Contexts;

/// <inheritdoc />
public class
    IdentityServerDbContext : KirelIdentityContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim,
        UserToken>
{
    /// <inheritdoc />
    public IdentityServerDbContext(DbContextOptions options) : base(options)
    {
    }
}

/// <summary>
/// Identity MySQL context class
/// </summary>
public class MysqlIdentityServerDbContext : IdentityServerDbContext
{
    /// <summary>
    /// Returns instance of identity mysql db context
    /// </summary>
    /// <param name="options"> The options to be used by a DbContext. </param>
    public MysqlIdentityServerDbContext(DbContextOptions options) : base(options)
    {
    }
}

/// <summary>
/// Identity Sqlite context class
/// </summary>
public class SqliteIdentityServerDbContext : IdentityServerDbContext
{
    /// <summary>
    /// Returns instance of identity Sqlite db context
    /// </summary>
    /// <param name="options"> The options to be used by a DbContext. </param>
    public SqliteIdentityServerDbContext(DbContextOptions options) : base(options)
    {
    }
}

/// <summary>
/// Identity MSSQL context class
/// </summary>
public class MssqlIdentityServerDbContext : IdentityServerDbContext
{
    /// <summary>
    /// Returns instance of identity mssql db context
    /// </summary>
    /// <param name="options"> The options to be used by a DbContext. </param>
    public MssqlIdentityServerDbContext(DbContextOptions options) : base(options)
    {
    }
}

/// <summary>
/// Identity PostgresSQL context class
/// </summary>
public class PostgresqlIdentityServerDbContext : IdentityServerDbContext
{
    /// <summary>
    /// Returns instance of identity postgres sql db context
    /// </summary>
    /// <param name="options"> The options to be used by a DbContext. </param>
    public PostgresqlIdentityServerDbContext(DbContextOptions options) : base(options)
    {
    }
}