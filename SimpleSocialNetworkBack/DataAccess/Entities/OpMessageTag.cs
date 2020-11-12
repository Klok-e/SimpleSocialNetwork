using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class OpMessageTag
    {
        /// <summary>
        /// Key
        /// </summary>
        [ForeignKey(nameof(Tag))]
        public string TagId { get; set; } = null!;

        /// <summary>
        /// Key
        /// </summary>
        [ForeignKey(nameof(OpMessage))]
        public int OpId { get; set; }

        public virtual Tag? Tag { get; set; }

        public virtual OpMessage? OpMessage { get; set; }
    }
}