using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mimo.Persistence.Entities;

namespace Mimo.Persistence.DbContexts
{
    public class MimoDbContext : DbContext, IMimoDbContext
    {

        public DbSet<Course> Courses { get; set; }

        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Achievement> Achievements { get; set; }

        public MimoDbContext(DbContextOptions<MimoDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(x => x.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(x => x.Username).IsUnique();
                entity.HasMany(x => x.Achievements)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(x => x.CompletedLessons)
                    .WithOne(x => x.User)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(x => x.Name).IsUnique();
                entity.Property(x => x.Description).IsRequired().HasMaxLength(200);
                entity.HasMany(x => x.Chapters)
                    .WithOne(x => x.Course)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Description).IsRequired().HasMaxLength(200);
                entity.HasMany(x => x.Lessons)
                    .WithOne(x => x.Chapter)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Description).IsRequired().HasMaxLength(200);
                entity.Property(x => x.Content).IsRequired().HasMaxLength(1000);
            });
            modelBuilder.Entity<Achievement>(entity =>
            {
                entity.Property(x => x.Name).IsRequired().HasMaxLength(30);
                entity.HasIndex(x => x.Name).IsUnique();
            });
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return base.Database.BeginTransaction();
        }
    }
}
