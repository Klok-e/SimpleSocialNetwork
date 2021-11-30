using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class SecurePassword
    {
        [Key]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        [Required]
        public string Salt { get; set; } = null!;

        [Required]
        public string Hashed { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
