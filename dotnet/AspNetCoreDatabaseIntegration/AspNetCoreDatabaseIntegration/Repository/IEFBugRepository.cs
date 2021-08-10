using AspNetCoreDatabaseIntegration.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreDatabaseIntegration.Repository
{
    public interface IEFBugRepository
    {
        IList<Bug> GetAll();
        IList<Bug> GetAllRawSQL();
        Task<IList<Bug>> GetAllParallel(int ammount);
        int GetTotal();
    }
}
