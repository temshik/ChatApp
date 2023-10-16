namespace ChatApp.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMessagesRepository MessageSet { get; }
        IRoomsRepository RoomSet { get; }
        IUserRepository UserSet { get; }
        Task<int> SaveAsync(CancellationToken cancellationToken);
        void Dispose();
    }
}
