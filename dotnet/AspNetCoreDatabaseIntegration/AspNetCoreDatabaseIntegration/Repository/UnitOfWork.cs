namespace AspNetCoreDatabaseIntegration.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IDapperExceptionTypeRepository dapperExceptionTypeRepository, IEFExceptionTypeRepository efExceptionTypeRepository)
        {
            DapperExceptionTypeRepository = dapperExceptionTypeRepository;
            EFExceptionTypeRepository = efExceptionTypeRepository;
        }
        public IDapperExceptionTypeRepository DapperExceptionTypeRepository { get; }

        public IEFExceptionTypeRepository EFExceptionTypeRepository { get; }
    }
}
