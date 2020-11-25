using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Requests
{
    public class BanUserModel
    {
        [Required] public string Login { get; set; } = null!;
        [Required] public DateTime ExpirationDate { get; set; }
    }
}