using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Subscription
    {
        /// <summary>
        /// Key
        /// </summary>
        [ForeignKey(nameof(Subscriber))]
        public string SubscriberId { get; set; } = null!;

        /// <summary>
        /// Key
        /// </summary>
        [ForeignKey(nameof(Target))]
        public string TargetId { get; set; } = null!;

        public bool IsActive { get; set; }

        public virtual User? Subscriber { get; set; }

        public virtual User? Target { get; set; }
    }
}