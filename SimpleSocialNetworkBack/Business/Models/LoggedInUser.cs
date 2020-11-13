using System;

namespace Business.Models
{
    public class LoggedInUser
    {
        public string Login { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}