namespace DataAccess.Entities
{
    public class PostVote
    {
        public string VoterLogin { get; set; } = null!;
        public int PostId { get; set; }

        public bool IsUpvote { get; set; }

        public virtual ApplicationUser Voter { get; set; } = null!;
        public virtual OpMessage OpMessage { get; set; } = null!;
    }
}