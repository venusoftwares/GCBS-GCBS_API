namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tit")]
    public partial class Tit
    {
        public int Id { get; set; }

        [Column("Tit")]
        [Required]
        [StringLength(50)]
        public string Tit1 { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
    public class TitVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
