using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Subscription
    {
        [Key] public int Id { get; set; }

        public string? SubscriberId { get; set; }
        public string? TargetId { get; set; }

        public bool IsActive { get; set; }

        public virtual ApplicationUser? Subscriber { get; set; }

        public virtual ApplicationUser? Target { get; set; }
    }
}