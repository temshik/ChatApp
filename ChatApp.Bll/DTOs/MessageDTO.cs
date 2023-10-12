using System.ComponentModel.DataAnnotations;

namespace ChatApp.Bll.DTOs
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string FromUserName { get; set; }
        public string FromFullName { get; set; }
        [Required]
        public string Room { get; set; }
        public string Avatar { get; set; }
    }
}
