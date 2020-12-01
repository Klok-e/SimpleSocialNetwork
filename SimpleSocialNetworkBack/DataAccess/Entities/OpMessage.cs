using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class OpMessage : ISoftDelete
    {
        [Key] public int Id { get; set; }

        [Required] public string Title { get; set; } = null!;
        [Required] public string Content { get; set; } = null!;

        //public int Points { get; set; }

        public DateTime SendDate { get; set; }

        public virtual ApplicationUser? Poster { get; set; }

        public virtual ICollection<OpMessageTag> Tags { get; set; } = null!;

        public virtual ICollection<Message> Messages { get; set; } = null!;
        public virtual ICollection<PostVote> Votes { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}