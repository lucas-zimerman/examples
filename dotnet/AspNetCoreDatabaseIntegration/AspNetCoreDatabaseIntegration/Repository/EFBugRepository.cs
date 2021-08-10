using AspNetCoreDatabaseIntegration.Model;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public async Task<IList<Bug>> GetAllParallel(int ammount)
        {
            var sql1 = $"SELECT TOP {ammount / 4} * FROM Bug";
            var sql2 = $"SELECT TOP {ammount / 4} * FROM Bug WHERE Id > {ammount / 4}";
            var sql3 = $"SELECT TOP {ammount / 4} * FROM Bug WHERE Id > {ammount - ammount / 4}";
            var sql4 = $"SELECT TOP {ammount / 4} * FROM Bug WHERE Id > {ammount / 2}";
            var results = new Task<List<Bug>>[]
            {
                context.Error.Select(s => s).Take(ammount / 4).ToListAsync(),
                context.Error.Select(s => s).Where((bug) => bug.Id > ammount / 4).Take(ammount / 4).ToListAsync(),
                context.Error.Select(s => s).Where((bug) => bug.Id > ammount - ammount / 4).Take(ammount / 4).ToListAsync(),
                context.Error.Select(s => s).Where((bug) => bug.Id > ammount / 2).Take(ammount / 4).ToListAsync()
            };
            await Task.WhenAll(results);
            return results.SelectMany(p => p.Result).ToList();
            }

        public int GetTotal()
        {
                return context.Error.Count();
        }
    }
}
