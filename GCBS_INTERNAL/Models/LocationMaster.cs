namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LocationMaster")]
    public partial class LocationMaster
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Country { get; set; }

        [StringLength(200)]
        public string State { get; set; }

        [StringLength(200)]
        public string City { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
