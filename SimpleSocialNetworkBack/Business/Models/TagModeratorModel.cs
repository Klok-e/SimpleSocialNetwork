using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class TagModeratorModel
    {
        public string TagId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public bool IsRevoked { get; set; }
    }
}