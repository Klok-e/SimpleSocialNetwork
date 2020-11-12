using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData] [Required] public string About { get; set; } = "";

        public virtual ICollection<Subscription>? Subscriptions { get; set; }
        
        public virtual ICollection<Subscription>? Subscribers { get; set; }
    }
}