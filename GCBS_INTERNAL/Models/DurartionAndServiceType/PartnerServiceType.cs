namespace GCBS_INTERNAL.Models.DurartionAndServiceType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartnerServiceType")]
    public partial class PartnerServiceType
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceType { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }
    }
}
