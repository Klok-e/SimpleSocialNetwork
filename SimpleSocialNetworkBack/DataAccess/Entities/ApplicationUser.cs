using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class ApplicationUser
    {
        [Key] public string Login { get; set; } = null!;

        [Required] public string About { get; set; } = "";
        
        public DateTime? DateBirth { get; set; }

        public bool IsDeleted { get; set; }
        
        public virtual SecurePassword? Password { get; set; }
        
        public virtual ICollection<Subscription>? Subscriptions { get; set; }

        public virtual ICollection<Subscription>? Subscribers { get; set; }
    }
}