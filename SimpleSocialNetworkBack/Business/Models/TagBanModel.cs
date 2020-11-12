using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class TagBanModel
    {
        public int Id { get; set; }

        public string TagId { get; set; } = null!;
        public string UserId { get; set; } = null!;


        public string ModeratorId { get; set; } = null!;

        public DateTime ExpirationDate { get; set; }
        public DateTime BanIssuedDate { get; set; }

        public bool Cancelled { get; set; }
    }
}