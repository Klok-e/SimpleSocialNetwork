using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Message
    {
        /// <summary>
        /// Key
        /// </summary>
        public int OpId { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public int MessageId { get; set; }

        [ForeignKey(nameof(Poster))] public string PosterId { get; set; } = null!;

        [Required] public string Content { get; set; } = null!;

        public int Points { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime SendDate { get; set; }

        public virtual User? Poster { get; set; }
    }
}