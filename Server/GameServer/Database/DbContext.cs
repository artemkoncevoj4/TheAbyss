using Microsoft.EntityFrameworkCore;
namespace GameServer.Database;
public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }
    public DbSet<User> User {get;set;} = default!;

}