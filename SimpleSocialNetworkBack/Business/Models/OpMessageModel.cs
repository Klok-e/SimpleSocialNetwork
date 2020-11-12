using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities;

namespace Business.Models
{
    public class OpMessageModel
    {
        public int Id { get; set; }

        public string? PosterId { get; set; }

        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public int Points { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime SendDate { get; set; }

        private sealed class OpMessageModelEqualityComparer : IEqualityComparer<OpMessageModel>
        {
            public bool Equals(OpMessageModel x, OpMessageModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id && x.PosterId == y.PosterId && x.Title == y.Title && x.Content == y.Content &&
                       x.Points == y.Points && x.IsDeleted == y.IsDeleted && x.SendDate.Equals(y.SendDate);
            }

            public int GetHashCode(OpMessageModel obj)
            {
                return HashCode.Combine(obj.Id, obj.PosterId, obj.Title, obj.Content, obj.Points, obj.IsDeleted,
                    obj.SendDate);
            }
        }

        public static IEqualityComparer<OpMessageModel> OpMessageModelComparer { get; } =
            new OpMessageModelEqualityComparer();
    }
}