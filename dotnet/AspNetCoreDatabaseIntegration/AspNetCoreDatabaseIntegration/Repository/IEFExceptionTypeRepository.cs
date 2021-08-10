using AspNetCoreDatabaseIntegration.Model;
using System.Collections.Generic;

namespace AspNetCoreDatabaseIntegration.Repository
{
    public interface IEFExceptionTypeRepository
    {
        IList<ExceptionType> GetAll();
        IList<ExceptionType> GetAll(int Max);
        IList<ExceptionType> GetAllRawSQL();
        int GetTotal();
    }
}
