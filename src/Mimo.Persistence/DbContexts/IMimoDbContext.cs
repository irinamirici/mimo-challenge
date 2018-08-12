using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mimo.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mimo.Persistence.DbContexts
{
    public interface IMimoDbContext
    {
        DbSet<Course> Courses { get; }

        DbSet<Chapter> Chapters { get; }

        DbSet<Lesson> Lessons { get; }

        DbSet<User> Users { get; }

        DbSet<Achievement> Achievements { get; }

         Task<int> SaveChangesAsync();

        int SaveChanges();

        IDbContextTransaction BeginTransaction();
    }
}
