namespace AspNetCoreDatabaseIntegration.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDapperBugRepository bugRepository, IEFBugRepository efBugRepository)
        {
            BugsRepository = bugRepository;
            EFBugRepository = efBugRepository;
        }
        public IDapperBugRepository BugsRepository { get; }

        public IEFBugRepository EFBugRepository { get; }
    }
}
