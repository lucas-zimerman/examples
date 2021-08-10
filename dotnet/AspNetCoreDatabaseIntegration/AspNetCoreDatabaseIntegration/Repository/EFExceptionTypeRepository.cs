using AspNetCoreDatabaseIntegration.Model;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreDatabaseIntegration.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreDatabaseIntegration.Repository
{
    public class EFExceptionTypeRepository : IEFExceptionTypeRepository
    {
        private readonly ExceptionTypeDbContext context;
        public EFExceptionTypeRepository(ExceptionTypeDbContext context)
        {
            this.context = context;
        }

        public IList<ExceptionType> GetAll()
            => context.ExceptionTypes.ToList();

        public IList<ExceptionType> GetAll(int max)
            => context.ExceptionTypes.Take(max).ToList();

        public IList<ExceptionType> GetAllRawSQL()
            => context.ExceptionTypes.FromSqlRaw("SELECT * FROM Bug").ToList();

        public int GetTotal()
            => context.ExceptionTypes.Count();
    }
}
