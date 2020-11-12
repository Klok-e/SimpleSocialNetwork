using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Tag
    {
        [Key] public string Name { get; set; } = null!;
    }
}