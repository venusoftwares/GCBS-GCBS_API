namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelMaster")]
    public partial class HotelMaster
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string HotelName { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(200)]
        public string Images { get; set; }

        [StringLength(500)]
        public string WebsiteUrl { get; set; }

        public DateTime? ValidStartDate { get; set; }

        public DateTime? ValidEndDate { get; set; }   
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
    public class HotelMasterVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
