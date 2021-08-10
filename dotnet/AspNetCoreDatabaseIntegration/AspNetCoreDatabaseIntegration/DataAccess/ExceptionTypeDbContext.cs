using AspNetCoreDatabaseIntegration.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AspNetCoreDatabaseIntegration.DataAccess
{
    public class ExceptionTypeDbContext : DbContext
    {
        public ExceptionTypeDbContext(DbContextOptions<ExceptionTypeDbContext> options)
    : base(options)
        { 
        }

        public DbSet<ExceptionType> ExceptionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExceptionType>( entity =>
            {
                entity.HasKey(k => k.Id);
            });
        }

        public static void Seed(ExceptionTypeDbContext db)
        {
            //Clear the previous data
            db.Database.ExecuteSqlRaw("TRUNCATE TABLE ExceptionType");

            //Get list of exceptions types.
            var type = typeof(Exception);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .ToList();

            var rand = new Random();
            for (int i = 0; i < 50_000; i++)
            {
                db.Set<ExceptionType>().Add(new ExceptionType()
                {
                    Name = types[rand.Next(0, types.Count())].FullName,
                    Level = DateTime.Now.Ticks.ToString()

                });
            }
            db.SaveChanges();
        }
    }
}
