namespace DataAccess.Entities
{
    public class OpMessageTag
    {
        public string TagId { get; set; } = null!;

        public int OpId { get; set; }

        public virtual Tag Tag { get; set; } = null!;

        public virtual OpMessage OpMessage { get; set; } = null!;
    }
}
