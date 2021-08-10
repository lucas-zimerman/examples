using AspNetCoreDatabaseIntegration.Model;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDatabaseIntegration.Repository
{
    public class DapperBugRepository : IDapperBugRepository
    {
        private readonly IConfiguration configuration;
        public DapperBugRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IList<Bug>> GetAll()
        {
            var sql = "SELECT * FROM Bug";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Bug>(sql);
                return result.ToList();
            }
        }

        public async Task<IList<Bug>> GetAllParallel(int ammount)
        {
            var sql1 = $"SELECT TOP {ammount / 4} * FROM Bug";
            var sql2 = $"SELECT TOP {ammount / 4} * FROM Bug WHERE Id > {ammount / 4}";
            var sql3 = $"SELECT TOP {ammount / 4} * FROM Bug WHERE Id > {ammount - ammount / 4}";
            var sql4 = $"SELECT TOP {ammount / 4} * FROM Bug WHERE Id > {ammount / 2}";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var results = new Task<IEnumerable<Bug>>[]
                {
                    connection.QueryAsync<Bug>(sql1),
                    connection.QueryAsync<Bug>(sql2),
                    connection.QueryAsync<Bug>(sql3),
                    connection.QueryAsync<Bug>(sql4)
                };
                await Task.WhenAll(results);
                return results.SelectMany(p => p.Result).ToList();
            }
        }

        public Task<int> GetTotal()
        {
            var sql = "SELECT count(*) FROM Bug";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                return Task.FromResult(connection.ExecuteScalar<int>(sql));
            }
        }
    }
}
