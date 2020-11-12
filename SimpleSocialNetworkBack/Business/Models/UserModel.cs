using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserModel
    {
        public string Login { get; set; } = null!;

        public string? Name { get; set; }

        public string About { get; set; } = null!;

        public DateTime? DateBirth { get; set; }

        public bool IsDeleted { get; set; }

        private sealed class UserModelEqualityComparer : IEqualityComparer<UserModel>
        {
            public bool Equals(UserModel x, UserModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Login == y.Login && x.Name == y.Name && x.About == y.About &&
                       Nullable.Equals(x.DateBirth, y.DateBirth) && x.IsDeleted == y.IsDeleted;
            }

            public int GetHashCode(UserModel obj)
            {
                return HashCode.Combine(obj.Login, obj.Name, obj.About, obj.DateBirth, obj.IsDeleted);
            }
        }

        public static IEqualityComparer<UserModel> UserModelComparer { get; } = new UserModelEqualityComparer();
    }
}