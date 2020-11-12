using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class SubscriptionModel
    {
        public string SubscriberId { get; set; } = null!;

        public string TargetId { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}