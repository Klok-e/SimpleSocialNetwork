using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class TagModel
    {
        public string Name { get; set; } = null!;

        private sealed class NameEqualityComparer : IEqualityComparer<TagModel>
        {
            public bool Equals(TagModel x, TagModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Name == y.Name;
            }

            public int GetHashCode(TagModel obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public static IEqualityComparer<TagModel> NameComparer { get; } = new NameEqualityComparer();
    }
}