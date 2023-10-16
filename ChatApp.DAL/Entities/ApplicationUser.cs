using ChatApp.DAL.Entities.Common;

namespace ChatApp.DAL.Entities
{
    public class ApplicationUser : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Avatar { get; set; }

        /* EF Relation */
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
