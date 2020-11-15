using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class CreateOpMessageModel
    {
        [Required]
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public List<string> Tags { get; set; } = null!;
    }
}