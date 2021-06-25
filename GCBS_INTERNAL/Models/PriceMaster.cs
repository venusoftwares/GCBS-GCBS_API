namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PriceMaster")]
    public partial class PriceMaster
    {
        public int Id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DurationHour { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DurationMinutes { get; set; }
        [Column(TypeName = "numeric")]
        public decimal? Price { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PremiumPrice { get; set; }

        public int? Shot { get; set; }

        public bool? Visible { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }

    public class PriceMasterVisible
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool Visible { get; set; }

    }
}
