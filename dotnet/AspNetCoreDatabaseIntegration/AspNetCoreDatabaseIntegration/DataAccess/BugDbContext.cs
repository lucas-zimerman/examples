using AspNetCoreDatabaseIntegration.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

namespace AspNetCoreDatabaseIntegration.DataAccess
{
    public class BugEfDbContext : DbContext
    {
        public BugEfDbContext(DbContextOptions<BugEfDbContext> options)
    : base(options)
        { 
        }

        public DbSet<Bug> Error { get; set; }
        public DbSet<Bug> Error2 { get; set; }
        public DbSet<Bug> Error3 { get; set; }
        public DbSet<Bug> Error4 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bug>( entity =>
            {
                entity.HasKey(k => k.Id);
            });
        }

        public static void Seed(BugEfDbContext db)
        {
            //Clear the previous data
            db.Database.ExecuteSqlRaw("TRUNCATE TABLE Bug");
            for (int i = 0; i < 50_000; i++)
            {
                db.Set<Bug>().Add(new Bug()
                {
                    Name = DateTime.Now.Ticks.ToString()
                });
            }
            db.SaveChanges();
        }
    }
}
