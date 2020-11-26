using System.ComponentModel.DataAnnotations;

namespace Business.Models.Requests
{
    public class CommentKeyModel
    {
        [Required] public int OpId { get; set; }

        [Required] public int MessageId { get; set; }
    }
}