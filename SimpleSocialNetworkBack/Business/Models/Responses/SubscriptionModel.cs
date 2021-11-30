using System.ComponentModel.DataAnnotations;

namespace Business.Models.Responses
{
    public class SubscriptionModel
    {
        [Required]
        public int Id { get; set; }

        public string? SubscriberId { get; set; }

        public string? TargetId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
