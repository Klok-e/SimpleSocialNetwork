using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class TagBan
    {
        [Key] public int Id { get; set; }

        [ForeignKey(nameof(Tag))] [Required] public string TagId { get; set; } = null!;
        [ForeignKey(nameof(User))] [Required] public string UserId { get; set; } = null!;

        [ForeignKey(nameof(Moderator))]
        [Required]
        public string ModeratorId { get; set; } = null!;

        public DateTime ExpirationDate { get; set; }
        public DateTime BanIssuedDate { get; set; }

        public bool Cancelled { get; set; }

        public virtual Tag? Tag { get; set; }
        public virtual User? User { get; set; }
        public virtual User? Moderator { get; set; }
    }
}