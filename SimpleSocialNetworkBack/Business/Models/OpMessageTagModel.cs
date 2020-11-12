using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities;

namespace Business.Models
{
    public class OpMessageTagModel
    {
        public string TagId { get; set; } = null!;

        public int OpId { get; set; }

        private sealed class TagIdOpIdEqualityComparer : IEqualityComparer<OpMessageTagModel>
        {
            public bool Equals(OpMessageTagModel x, OpMessageTagModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.TagId == y.TagId && x.OpId == y.OpId;
            }

            public int GetHashCode(OpMessageTagModel obj)
            {
                return HashCode.Combine(obj.TagId, obj.OpId);
            }
        }

        public static IEqualityComparer<OpMessageTagModel> TagIdOpIdComparer { get; } = new TagIdOpIdEqualityComparer();
    }
}