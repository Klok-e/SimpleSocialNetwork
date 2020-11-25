using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Tag
    {
        [Key] public string Name { get; set; } = null!;

        public virtual ICollection<TagBan> Bans { get; set; } = null!;

        public virtual ICollection<TagModerator> Moderators { get; set; } = null!;

        public virtual ICollection<OpMessageTag> OpMessages { get; set; } = null!;
    }
}