using Idata.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Factory class for creating instances of the <see cref="IdataContext"/>.
/// </summary>
public class DbContextFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="IdataContext"/> using the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <returns>An instance of <see cref="IdataContext"/>.</returns>
    public static IdataContext CreateDbContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdataContext>();
        optionsBuilder.UseSqlServer(connectionString);  // Change to your database provider

        return new IdataContext(optionsBuilder.Options);
    }
}