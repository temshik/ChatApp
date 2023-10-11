namespace ChatApp.DAL.Entities.Common
{
    public class AuditableEntity
    {
        public DateTimeOffset CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }
}
