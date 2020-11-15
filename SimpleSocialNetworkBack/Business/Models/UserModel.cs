using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserModel : IEquatable<UserModel>
    {
        [Required] public string Login { get; set; } = null!;
        [Required] public string About { get; set; } = null!;

        public DateTime? DateBirth { get; set; }
        [Required] public bool IsDeleted { get; set; }

        public bool Equals(UserModel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Login == other.Login && About == other.About && Nullable.Equals(DateBirth, other.DateBirth) &&
                   IsDeleted == other.IsDeleted;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Login, About, DateBirth, IsDeleted);
        }
    }
}