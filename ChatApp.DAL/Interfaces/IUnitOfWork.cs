namespace ChatApp.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMessagesRepository MessageSet { get; }
        IRoomsRepository RoomSet { get; }
        Task<bool> SaveAsync(CancellationToken cancellationToken);
        void Dispose();
    }
}
