using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserModel
    {
        public string Login { get; set; } = null!;

        public string? Name { get; set; }

        public string About { get; set; } = null!;

        public DateTime? DateBirth { get; set; }

        public bool IsDeleted { get; set; }
    }
}