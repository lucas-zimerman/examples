using AspNetCoreDatabaseIntegration.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreDatabaseIntegration.Repository
{
    public interface IDapperBugRepository
    {
        Task<IList<Bug>> GetAll();
        Task<IList<Bug>> GetAllParallel(int ammount);
        Task<int> GetTotal();
    }
}
