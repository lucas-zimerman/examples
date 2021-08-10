using AspNetCoreDatabaseIntegration.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreDatabaseIntegration.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreDatabaseIntegration.Repository
{
    public class EFBugRepository : IEFBugRepository
    {
        private readonly BugEfDbContext context;
        public EFBugRepository(BugEfDbContext context)
        {
            this.context = context;
        }

        public IList<Bug> GetAll()
        {
            return context.Error.Select(s => s).ToList();
        }
        public IList<Bug> GetAllRawSQL()
        {
            return context.Error.FromSqlRaw("SELECT * FROM Bug").ToList();
        }

        public async Task<IList<Bug>> GetAllParallel(int ammount, int take, int db)
        ///            => await context.Error.Select(s => s).Where((bug) => bug.Id > ammount).Take(take).ToListAsync(); 
        {
            if (db == 0)
            {
                return context.Error.FromSqlRaw($"SELECT TOP {take} * FROM Bug WHERE ID > {ammount}").AsNoTracking().ToList();
            }
            else if (db == 1)
            {
                return context.Error2.FromSqlRaw($"SELECT TOP {take} * FROM Bug WHERE ID > {ammount}").AsNoTracking().ToList();
            }
            else if (db == 2)
            {
                return context.Error3.FromSqlRaw($"SELECT TOP {take} * FROM Bug WHERE ID > {ammount}").AsNoTracking().ToList();
            }
            else if (db == 3)
            {
                return context.Error4.FromSqlRaw($"SELECT TOP {take} * FROM Bug WHERE ID > {ammount}").AsNoTracking().ToList();
            }
            return null;
        }
        public int GetTotal()
        {
                return context.Error.Count();
        }
    }
}
