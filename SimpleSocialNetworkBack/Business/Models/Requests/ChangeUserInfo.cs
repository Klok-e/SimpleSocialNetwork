using System;

namespace Business.Models.Requests
{
    public class ChangeUserInfo
    {
        public string? About { get; set; }

        public DateTime? DateBirth { get; set; }
    }
}
