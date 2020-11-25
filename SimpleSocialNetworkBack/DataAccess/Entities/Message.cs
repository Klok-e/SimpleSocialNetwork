using System;

namespace DataAccess.Entities
{
    public class Message : ISoftDelete
    {
        public int OpId { get; set; }
        public int MessageId { get; set; }

        public string Content { get; set; } = null!;

        public int Points { get; set; }

        public DateTime SendDate { get; set; }


        public virtual ApplicationUser Poster { get; set; } = null!;

        public virtual OpMessage OpMessage { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}