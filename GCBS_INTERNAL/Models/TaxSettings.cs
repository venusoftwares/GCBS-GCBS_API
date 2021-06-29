namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TaxSettings
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GSTIN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Percentage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal CGST { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SGST { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
