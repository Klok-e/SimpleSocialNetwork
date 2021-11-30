using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Subscription : ISoftDelete
    {
        [Key]
        public int Id { get; set; }

        public string? SubscriberId { get; set; }

        public string? TargetId { get; set; }

        public bool IsNotActive { get; set; }

        public virtual ApplicationUser? Subscriber { get; set; }

        public virtual ApplicationUser? Target { get; set; }

        #region ISoftDelete Members

        [NotMapped]
        public bool IsDeleted
        {
            get => IsNotActive;
            set => IsNotActive = value;
        }

        #endregion
    }
}
