namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Hair
    {
        public int Id { get; set; }

        [Column("Hair")]
        [Required]
        [StringLength(50)]
        public string Hair1 { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
    public class HairVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
