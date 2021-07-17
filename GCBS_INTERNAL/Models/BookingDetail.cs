namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BookingDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(10)]
        public string BookingType { get; set; }

        public bool Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
    public class BookingDetailVisible
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool Visible { get; set; }

    }
}
