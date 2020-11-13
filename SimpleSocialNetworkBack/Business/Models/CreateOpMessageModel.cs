using System.Collections.Generic;

namespace Business.Models
{
    public class CreateOpMessageModel
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public List<string> Tags { get; set; } = null!;
    }
}