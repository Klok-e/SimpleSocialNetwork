using System.ComponentModel.DataAnnotations;

namespace Business.Models.Requests
{
    public class CreateCommentModel
    {
        [Required]
        public int OpId { get; set; }

        [Required]
        public string Content { get; set; } = null!;
    }
}
