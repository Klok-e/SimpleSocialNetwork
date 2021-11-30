using System.ComponentModel.DataAnnotations;

namespace Business.Models.Responses
{
    public class LimitedUserModel
    {
        [Required]
        public string Login { get; set; } = null!;

        [Required]
        public string About { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }
    }
}
