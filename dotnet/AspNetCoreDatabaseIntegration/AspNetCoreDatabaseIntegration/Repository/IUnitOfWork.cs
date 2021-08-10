using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDatabaseIntegration.Repository
{
    public interface IUnitOfWork
    {
        public IDapperExceptionTypeRepository DapperExceptionTypeRepository { get; }
        public IEFExceptionTypeRepository EFExceptionTypeRepository { get; }
    }
}
