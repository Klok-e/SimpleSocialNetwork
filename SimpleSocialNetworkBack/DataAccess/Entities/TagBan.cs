using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class TagBan
    {
        [Key] public int Id { get; set; }

        public DateTime ExpirationDate { get; set; }
        public DateTime BanIssuedDate { get; set; }

        public bool Cancelled { get; set; }

        public virtual Tag? Tag { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual ApplicationUser? Moderator { get; set; }
    }
}