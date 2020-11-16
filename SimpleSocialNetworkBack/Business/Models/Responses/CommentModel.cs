using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities;

namespace Business.Models.Responses
{
    public class CommentModel
    {
        [Required] public int OpId { get; set; }

        [Required] public int MessageId { get; set; }

        [Required] public string PosterId { get; set; } = null!;

        [Required] public string Content { get; set; } = null!;

        [Required] public int Points { get; set; }

        [Required] public bool IsDeleted { get; set; }

        [Required] public DateTime SendDate { get; set; }
    }
}