namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChatNotification")]
    public partial class ChatNotification
    {
        public long Id { get; set; }

        public int FromUser { get; set; }

        public int ToUser { get; set; }

        [Required]
        [StringLength(250)]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        public bool IsDeleted { get; set; }
    }
}
