namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SiteBannerMaster")]
    public partial class SiteBannerMaster
    {
        public int Id { get; set; }

        [Required]
        public string Images { get; set; }

        [Required]
        public string MainHeading { get; set; }

        [Required]
        public string MainTitle { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }

    public class SiteBannerVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
