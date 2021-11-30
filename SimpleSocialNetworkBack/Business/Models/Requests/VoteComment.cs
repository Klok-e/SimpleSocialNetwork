using System.ComponentModel.DataAnnotations;

namespace Business.Models.Requests
{
    public class VoteComment
    {
        [Required]
        public CommentKeyModel CommentId { get; set; } = null!;

        [Required]
        public VoteType VoteType { get; set; }
    }
}
