using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class ApplicationUser : ISoftDelete
    {
        [Key] public string Login { get; set; } = null!;

        public string? About { get; set; }

        public DateTime? DateBirth { get; set; }

        public bool IsAdmin { get; set; }

        public virtual SecurePassword? Password { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = null!;

        public virtual ICollection<Subscription> Subscribers { get; set; } = null!;

        public virtual ICollection<Message> Messages { get; set; } = null!;

        public virtual ICollection<OpMessage> Posts { get; set; } = null!;

        public virtual ICollection<TagBan> BansReceived { get; set; } = null!;

        public virtual ICollection<TagBan> BansIssued { get; set; } = null!;

        public virtual ICollection<TagModerator> ModeratorOfTags { get; set; } = null!;
        
        public virtual ICollection<PostVote> PostVotes { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}