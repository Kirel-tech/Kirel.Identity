namespace Example.Contexts;

/// <summary>
/// Static class for database initialization
/// </summary>
public static class SeedDb
{
    /// <summary>
    /// Database initialization
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ExKirelIdentityContext>();
        context.Database.EnsureCreated();
    }
}