using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities;

namespace Business.Models
{
    public class SecurePasswordModel
    {
        public string UserId { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string Hashed { get; set; } = null!;

        //public virtual User? User { get; set; }
        private sealed class UserIdSaltHashedEqualityComparer : IEqualityComparer<SecurePasswordModel>
        {
            public bool Equals(SecurePasswordModel x, SecurePasswordModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.UserId == y.UserId && x.Salt == y.Salt && x.Hashed == y.Hashed;
            }

            public int GetHashCode(SecurePasswordModel obj)
            {
                return HashCode.Combine(obj.UserId, obj.Salt, obj.Hashed);
            }
        }

        public static IEqualityComparer<SecurePasswordModel> UserIdSaltHashedComparer { get; } =
            new UserIdSaltHashedEqualityComparer();
    }
}