using Microsoft.EntityFrameworkCore;
using DatabaseService.Data.Entities;

namespace DatabaseService.Data.DatabaseContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
