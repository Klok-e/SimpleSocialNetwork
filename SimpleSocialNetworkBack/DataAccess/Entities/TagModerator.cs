using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class TagModerator : ISoftDelete
    {
        public string TagId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public bool IsRevoked { get; set; }

        public virtual Tag Tag { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        #region ISoftDelete Members

        [NotMapped]
        public bool IsDeleted
        {
            get => IsRevoked;
            set => IsRevoked = value;
        }

        #endregion
    }
}
