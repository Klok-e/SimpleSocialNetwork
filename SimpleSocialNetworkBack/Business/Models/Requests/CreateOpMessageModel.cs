using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Requests
{
    public class CreateOpMessageModel
    {
        [Required] [MinLength(10)] public string Title { get; set; } = null!;
        [Required] public string Content { get; set; } = null!;
        [Required] public List<string> Tags { get; set; } = null!;
    }
}