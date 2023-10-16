using ChatApp.DAL.Entities.Common;

namespace ChatApp.DAL.Entities
{
    public class Message : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid ToRoomId { get; set; }
        public Room ToRoom { get; set; }
        public Guid FromUserId { get; set; }
        public ApplicationUser FromUser { get; set; }

    }
}
