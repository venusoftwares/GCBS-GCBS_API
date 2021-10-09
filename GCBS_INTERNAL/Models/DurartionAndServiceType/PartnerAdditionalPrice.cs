namespace GCBS_INTERNAL.Models.DurartionAndServiceType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartnerAdditionalPrice")]
    public partial class PartnerAdditionalPrice
    {
        public int id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceType { get; set; }

        public decimal AdditionalPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int? UpdateBy { get; set; }
    }
}
