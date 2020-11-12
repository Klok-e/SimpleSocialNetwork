using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class SubscriptionModel
    {
        public int Id { get; set; }
        public string? SubscriberId { get; set; }

        public string? TargetId { get; set; }

        public bool IsActive { get; set; }

        private sealed class SubscriptionModelEqualityComparer : IEqualityComparer<SubscriptionModel>
        {
            public bool Equals(SubscriptionModel x, SubscriptionModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id && x.SubscriberId == y.SubscriberId && x.TargetId == y.TargetId &&
                       x.IsActive == y.IsActive;
            }

            public int GetHashCode(SubscriptionModel obj)
            {
                return HashCode.Combine(obj.Id, obj.SubscriberId, obj.TargetId, obj.IsActive);
            }
        }

        public static IEqualityComparer<SubscriptionModel> SubscriptionModelComparer { get; } =
            new SubscriptionModelEqualityComparer();
    }
}