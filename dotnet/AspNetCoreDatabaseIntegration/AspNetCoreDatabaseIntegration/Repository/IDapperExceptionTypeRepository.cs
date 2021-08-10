using AspNetCoreDatabaseIntegration.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreDatabaseIntegration.Repository
{
    public interface IDapperExceptionTypeRepository
    {
        Task<IList<ExceptionType>> GetAll();
        Task<IList<ExceptionType>> GetAllParallel(int ammount);
        Task<int> GetTotal();
    }
}
