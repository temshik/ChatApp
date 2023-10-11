using ChatApp.DAL.Entities.Common;

namespace ChatApp.DAL.Entities
{
    public class Room : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public ApplicationUser Admin { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
