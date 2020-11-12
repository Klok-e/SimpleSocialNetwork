using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class User
    {
        [Key] public string Login { get; set; } = null!;

        public string? Name { get; set; }

        [Required] public string About { get; set; } = "";

        public DateTime? DateBirth { get; set; }

        public bool IsDeleted { get; set; }

        public virtual SecurePassword? Password { get; set; }
    }
}