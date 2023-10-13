namespace ChatApp.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMessagesRepository MessageSet { get; }
        IRoomsRepository RoomSet { get; }
        IUserRepository UserSet { get; }
        Task<bool> SaveAsync(CancellationToken cancellationToken);
        void Dispose();
    }
}
