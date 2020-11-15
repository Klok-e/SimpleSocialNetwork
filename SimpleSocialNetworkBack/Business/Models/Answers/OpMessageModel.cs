using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Models.Answers
{
    public class OpMessageModel
    {
        [Required] public int Id { get; set; }
        public string? PosterId { get; set; }
        [Required] public string Title { get; set; } = null!;
        [Required] public string Content { get; set; } = null!;
        [Required] public int Points { get; set; }
        [Required] public bool IsDeleted { get; set; }
        [Required] public DateTime SendDate { get; set; }
    }
}