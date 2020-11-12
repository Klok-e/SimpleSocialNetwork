using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities;

namespace Business.Models
{
    public class OpMessageTagModel
    {
        public string TagId { get; set; } = null!;

        public int OpId { get; set; }
    }
}