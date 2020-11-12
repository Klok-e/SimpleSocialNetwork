using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities;

namespace Business.Models
{
    public class SecurePasswordModel
    {
        public string UserId { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string Hashed { get; set; } = null!;

        //public virtual User? User { get; set; }
    }
}