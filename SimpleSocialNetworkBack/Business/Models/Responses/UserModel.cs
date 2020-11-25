using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Responses
{
    public class UserModel : IEquatable<UserModel>
    {
        [Required] public string Login { get; set; } = null!;
        public string? About { get; set; }

        public DateTime? DateBirth { get; set; }
        [Required] public bool IsDeleted { get; set; }
        [Required] public bool IsAdmin { get; set; }

        public bool Equals(UserModel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Login == other.Login && About == other.About && Nullable.Equals(DateBirth, other.DateBirth) &&
                   IsDeleted == other.IsDeleted && IsAdmin == other.IsAdmin;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UserModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Login, About, DateBirth, IsDeleted, IsAdmin);
        }
    }
}