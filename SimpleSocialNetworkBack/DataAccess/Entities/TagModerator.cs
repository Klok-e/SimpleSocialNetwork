using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class TagModerator
    {
        public string TagId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public bool IsRevoked { get; set; }

        public virtual Tag Tag { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}