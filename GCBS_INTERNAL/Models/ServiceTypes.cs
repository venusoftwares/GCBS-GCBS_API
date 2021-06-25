namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ServiceTypes
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string ServiceType { get; set; }

        public bool Visible { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }

    public class ServiceTypesVisible
    {
        public int Id { get; set; }
        public bool Visible { get; set; }
    }
}
