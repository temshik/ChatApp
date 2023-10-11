namespace ChatApp.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMessagesRepository MessageSet { get; }
        Task<bool> SaveAsync(CancellationToken cancellationToken);
    }
}
