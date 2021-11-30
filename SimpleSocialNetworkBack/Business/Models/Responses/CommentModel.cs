using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Responses
{
    public class CommentModel
    {
        [Required]
        public int OpId { get; set; }

        [Required]
        public int MessageId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public int Points { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public DateTime SendDate { get; set; }

        public string? PosterId { get; set; }
    }
}
