using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend;

public class Context : DbContext
{
    private const string DATABASE_PATH = "database.db";

    public DbSet<User> Users { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
# if DEBUG
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        optionsBuilder.UseSqlite($"DataSource={baseDir}{DATABASE_PATH}");
#else

        string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
#endif
    }
}
