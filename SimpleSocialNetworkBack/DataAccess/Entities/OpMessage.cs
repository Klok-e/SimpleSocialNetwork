using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class OpMessage
    {
        [Key] public int Id { get; set; }

        [ForeignKey(nameof(Poster))] public string? PosterId { get; set; }

        [Required] public string Title { get; set; } = null!;
        [Required] public string Content { get; set; } = null!;

        public int Points { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime SendDate { get; set; }

        public virtual ApplicationUser? Poster { get; set; }
    }
}