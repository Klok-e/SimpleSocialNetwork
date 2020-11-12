using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class TagModeratorModel
    {
        public string TagId { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public bool IsRevoked { get; set; }

        private sealed class TagIdUserIdIsRevokedEqualityComparer : IEqualityComparer<TagModeratorModel>
        {
            public bool Equals(TagModeratorModel x, TagModeratorModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.TagId == y.TagId && x.UserId == y.UserId && x.IsRevoked == y.IsRevoked;
            }

            public int GetHashCode(TagModeratorModel obj)
            {
                return HashCode.Combine(obj.TagId, obj.UserId, obj.IsRevoked);
            }
        }

        public static IEqualityComparer<TagModeratorModel> TagIdUserIdIsRevokedComparer { get; } =
            new TagIdUserIdIsRevokedEqualityComparer();
    }
}