namespace GCBS_INTERNAL.Models.DurartionAndServiceType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DurationAndBasePrice")]
    public partial class DurationAndBasePrice
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DurationOrTime { get; set; }
        [Required]
        public int Hour { get; set; }
        [Required]
        public int Minutes { get; set; }
        [Required]
        public decimal BasePrice { get; set; }
        [Required]
        public decimal Margin { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
