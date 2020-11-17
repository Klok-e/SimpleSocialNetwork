using System.ComponentModel.DataAnnotations;

namespace Business.Models.Requests
{
    public class VoteComment
    {
        [Required] public int OpId { get; set; }

        [Required] public int MessageId { get; set; }

        [Required] public VoteType VoteType { get; set; }
    }
}