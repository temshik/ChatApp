namespace ChatApp.DAL.Entities.Common
{
    public interface IHasKey<T>
    {
        T Id { get; set; }
    }
}
