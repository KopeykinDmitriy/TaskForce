using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.Entities;

namespace SCT.Common.Data.DatabaseContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users{ get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskTags> TaskTags { get; set; }
        public DbSet<TaskRelation> TaskRelations { get; set; }
        public DbSet<UserTags> UserTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder); //наследуем базовую логику 

            // Определение ключей и связей для Users
            modelBuilder.Entity<User>()
                .HasKey(u => u.id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserProjects)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.UserId);
 

            // Определение ключей и связей для Project
            modelBuilder.Entity<Project>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.UserProjects)
                .WithOne(up => up.Project)
                .HasForeignKey(up => up.ProjectId);


            // UserProject
            //modelBuilder.Entity<UserProject>()
            //    .HasKey(up => up.Id);

            modelBuilder.Entity<UserProject>() // составной ключ
                .HasKey(up => new {up.UserId, up.ProjectId});


            // Tasks
            //modelBuilder.Entity<Tasks>()
            //    .HasKey(t => t.Id);

            //modelBuilder.Entity<Tasks>()
            //    .HasOne(t => t.Project)        // У задачи есть один проект
            //    .WithMany(p => p.Tasks)        // У проекта может быть много задач
            //.HasForeignKey(t => t.ProjectId); // Внешний ключ - ProjectId

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity
                    .HasOne(t => t.Project)        // У задачи есть один проект
                    .WithMany(p => p.Tasks)        // У проекта может быть много задач
                    .HasForeignKey(t => t.ProjectId); // Внешний ключ - ProjectId
                entity
                    .HasOne(t => t.UserCreate)
                    .WithMany(u => u.UserCreate)
                    .HasForeignKey(t => t.UserCreateId);
                entity
                    .HasOne(t => t.UserDo)
                    .WithMany(u => u.UserDo)
                    .HasForeignKey(t => t.UserDoId);
            });

                //TasksRelotion
            modelBuilder.Entity<TaskRelation>()
                .HasKey(tr => tr.Id);

            // Связь TaskRelation с Tasks через IdTask1
            modelBuilder.Entity<TaskRelation>()
                .HasOne(tr => tr.Task1)
                .WithMany(t => t.TaskRelations1)
                .HasForeignKey(tr => tr.IdTask1);
            //.OnDelete(DeleteBehavior.Restrict); // Запрещаем каскадное удаление подумать об добавлении

            // Связь TaskRelation с Tasks через IdTask2
            modelBuilder.Entity<TaskRelation>()
                .HasOne(tr => tr.Task2)
                .WithMany(t => t.TaskRelations2)
                .HasForeignKey(tr => tr.IdTask2);
            //.OnDelete(DeleteBehavior.Restrict); 


            // Tag
            modelBuilder.Entity<Tag>()
                .HasKey(tt => tt.Id);


            //TaskTags
            //modelBuilder.Entity<TaskTags>()
            //    .HasKey(t => t.Id);

            modelBuilder.Entity<TaskTags>() // составной ключ
                .HasKey(tt => new { tt.TaskId, tt.TagId });

            modelBuilder.Entity<TaskTags>()
                .HasOne(tt => tt.Task)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskId);

            modelBuilder.Entity<TaskTags>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TagId);


            //UserTags
            modelBuilder.Entity<UserTags>()
                .HasKey(ut => ut.Id);

            modelBuilder.Entity<UserTags>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTags)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<UserTags>()
                .HasOne(ut => ut.Tag)
                .WithMany(t => t.UserTags)
                .HasForeignKey(ut => ut.TagId);

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
