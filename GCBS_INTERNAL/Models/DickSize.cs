namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DickSize")]
    public partial class DickSize
    {
        public int Id { get; set; }

        [Column("DickSize")]
        [Required]
        [StringLength(50)]
        public string DickSize1 { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }

    public class DickSizeVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
