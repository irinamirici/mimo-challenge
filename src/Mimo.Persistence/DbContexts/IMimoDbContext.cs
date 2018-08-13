using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mimo.Persistence.Entities;

namespace Mimo.Persistence.DbContexts
{
    public interface IMimoDbContext
    {
        DbSet<Course> Courses { get; }

        DbSet<Chapter> Chapters { get; }

        DbSet<Lesson> Lessons { get; }

        DbSet<User> Users { get; }

        DbSet<Achievement> Achievements { get; }

        int SaveChanges();

        IDbContextTransaction BeginTransaction();
    }
}
