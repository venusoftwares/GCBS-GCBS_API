namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reports")]
    public partial class Reports
    {
        public int Id { get; set; } 

        public DateTime Date { get; set; }

        public int ReportFrom { get; set; }

        public int ReportTo { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; } 
        
    }
}
