namespace GCBS_INTERNAL.Models.DurartionAndServiceType
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartnerBasePrice")]
    public partial class PartnerBasePrice
    {
        public int id { get; set; }

        public int UserId { get; set; }

        public int DurationId { get; set; }

        public decimal BasePrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int? UpdateBy { get; set; }

        [ForeignKey("DurationId")]
        public DurationAndBasePrice DurationAndBasePrice { get; set; }
    }
}
