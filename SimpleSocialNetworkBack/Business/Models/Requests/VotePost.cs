using System.ComponentModel.DataAnnotations;

namespace Business.Models.Requests
{
    public class VotePost
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public VoteType VoteType { get; set; }
    }
}
