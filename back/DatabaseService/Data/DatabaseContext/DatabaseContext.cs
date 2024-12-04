using Microsoft.EntityFrameworkCore;
using DatabaseService.Data.Entities;

namespace DatabaseService.Data.DatabaseContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //наследуем базовую логику 

            modelBuilder.Entity<User>()
                .ToTable("users");

            modelBuilder.Entity<User>()
                .HasKey(u => u.id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserProjects)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<Project>()
                .ToTable("projects");

            // Определение ключей и связей для Project
            modelBuilder.Entity<Project>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.UserProjects)
                .WithOne(up => up.Project)
                .HasForeignKey(up => up.ProjectId);

            modelBuilder.Entity<UserProject>()
                .ToTable("user_project");

            modelBuilder.Entity<UserProject>()
                .HasKey(up => up.Id);

            // modelBuilder.Entity<User>(entity =>
            // {
            //     entity.HasKey(u => u.Id);
            //     entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
            //     entity.HasMany(u => u.Orders)
            //         .WithOne(o => o.User)
            //         .HasForeignKey(o => o.UserId);
            // });

        }   

    }
}
