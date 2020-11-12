using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class TagModerator
    {
        /// <summary>
        /// Key
        /// </summary>
        [ForeignKey(nameof(Tag))]
        public string TagId { get; set; } = null!;

        /// <summary>
        /// Key
        /// </summary>
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public bool IsRevoked { get; set; }

        public virtual Tag? Tag { get; set; }
        public virtual User? User { get; set; }
    }
}