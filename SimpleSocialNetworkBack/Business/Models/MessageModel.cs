using System;

namespace Business.Models
{
    public class MessageModel
    {
        public int OpId { get; set; }

        public int MessageId { get; set; }

        public string PosterId { get; set; }= null!;

        public string Content { get; set; }= null!;

        public int Points { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime SendDate { get; set; }
    }
}