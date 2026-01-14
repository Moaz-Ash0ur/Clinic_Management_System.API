namespace ClinicManagement.Application.Common.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {   
        IRepository<T> GetRepository<T>() where T : class;
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();

    }








}