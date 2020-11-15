using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Answers
{
    public class LoggedInUser : IEquatable<LoggedInUser>
    {
        [Required] public string Login { get; set; } = null!;
        [Required] public string Token { get; set; } = null!;

        public bool Equals(LoggedInUser? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Login == other.Login && Token == other.Token;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LoggedInUser)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Login, Token);
        }
    }
}