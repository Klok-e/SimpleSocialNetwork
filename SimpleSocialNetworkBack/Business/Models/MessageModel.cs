using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class MessageModel
    {
        public int OpId { get; set; }

        public int MessageId { get; set; }

        public string PosterId { get; set; } = null!;

        public string Content { get; set; } = null!;

        public int Points { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime SendDate { get; set; }

        private sealed class MessageModelEqualityComparer : IEqualityComparer<MessageModel>
        {
            public bool Equals(MessageModel x, MessageModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.OpId == y.OpId && x.MessageId == y.MessageId && x.PosterId == y.PosterId &&
                       x.Content == y.Content && x.Points == y.Points && x.IsDeleted == y.IsDeleted &&
                       x.SendDate.Equals(y.SendDate);
            }

            public int GetHashCode(MessageModel obj)
            {
                return HashCode.Combine(obj.OpId, obj.MessageId, obj.PosterId, obj.Content, obj.Points, obj.IsDeleted,
                    obj.SendDate);
            }
        }

        public static IEqualityComparer<MessageModel> MessageModelComparer { get; } =
            new MessageModelEqualityComparer();
    }
}