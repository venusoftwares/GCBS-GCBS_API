namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SupportEnquiryType")]
    public partial class SupportEnquiryType
    {
        public int Id { get; set; }

        [Column("SupportEnquiryType")]
        [Required]
        [StringLength(50)]
        public string SupportEnquiryType1 { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }

    public class SupportEnquiryTypeVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
