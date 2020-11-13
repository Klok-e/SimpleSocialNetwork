using System;
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
    }
}