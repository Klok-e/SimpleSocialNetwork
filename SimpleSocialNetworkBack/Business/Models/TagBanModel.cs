using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class TagBanModel
    {
        public int Id { get; set; }

        public string TagId { get; set; } = null!;
        public string UserId { get; set; } = null!;


        public string ModeratorId { get; set; } = null!;

        public DateTime ExpirationDate { get; set; }
        public DateTime BanIssuedDate { get; set; }

        public bool Cancelled { get; set; }

        private sealed class TagBanModelEqualityComparer : IEqualityComparer<TagBanModel>
        {
            public bool Equals(TagBanModel x, TagBanModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id && x.TagId == y.TagId && x.UserId == y.UserId && x.ModeratorId == y.ModeratorId &&
                       x.ExpirationDate.Equals(y.ExpirationDate) && x.BanIssuedDate.Equals(y.BanIssuedDate) &&
                       x.Cancelled == y.Cancelled;
            }

            public int GetHashCode(TagBanModel obj)
            {
                return HashCode.Combine(obj.Id, obj.TagId, obj.UserId, obj.ModeratorId, obj.ExpirationDate,
                    obj.BanIssuedDate, obj.Cancelled);
            }
        }

        public static IEqualityComparer<TagBanModel> TagBanModelComparer { get; } = new TagBanModelEqualityComparer();
    }
}