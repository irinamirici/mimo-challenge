using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mimo.Persistence.Entities;
using System.Threading.Tasks;

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
            });
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(x => x.Name).IsUnique();
                entity.Property(x => x.Description).IsRequired().HasMaxLength(200);
            });
            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.Property(x => x.Name).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Description).IsRequired().HasMaxLength(200);
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

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
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
