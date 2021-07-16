namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Availability")]
    public partial class Availability
    {
        public int Id { get; set; }

        public int UserId { get; set; }     
        public TimeSpan Sunday { get; set; }

        public TimeSpan Monday { get; set; }

        public TimeSpan Tuesday { get; set; }

        public TimeSpan Wednesday { get; set; }

        public TimeSpan Thursday { get; set; }

        public TimeSpan Friday { get; set; }

        public TimeSpan Saturday { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
