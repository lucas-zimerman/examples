using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreDatabaseIntegration.Repository
{
    public interface IUnitOfWork
    {
        public IDapperBugRepository BugsRepository { get; }
        public IEFBugRepository EFBugRepository { get; }
    }
}
