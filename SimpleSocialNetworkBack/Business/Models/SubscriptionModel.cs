using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class SubscriptionModel
    {
        public string SubscriberId { get; set; } = null!;

        public string TargetId { get; set; } = null!;

        public bool IsActive { get; set; }

        private sealed class SubscriberIdTargetIdIsActiveEqualityComparer : IEqualityComparer<SubscriptionModel>
        {
            public bool Equals(SubscriptionModel x, SubscriptionModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.SubscriberId == y.SubscriberId && x.TargetId == y.TargetId && x.IsActive == y.IsActive;
            }

            public int GetHashCode(SubscriptionModel obj)
            {
                return HashCode.Combine(obj.SubscriberId, obj.TargetId, obj.IsActive);
            }
        }

        public static IEqualityComparer<SubscriptionModel> SubscriberIdTargetIdIsActiveComparer { get; } =
            new SubscriberIdTargetIdIsActiveEqualityComparer();
    }
}