namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ServiceDurartionPrice")]
    public partial class ServiceDurartionPrice
    {
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public int UserId { get; set; }
        [Required]
        public int DurationId { get; set; }
        [Required]
        [Column(TypeName = "numeric")]
        public decimal Price { get; set; }
        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("ServiceId")]
        public ServicesMaster ServicesMaster { get; set; }
        [ForeignKey("DurationId")]
        public DurationMaster DurationMaster { get; set; }       
    }

    public class ServiceDurationPriceVisible
    {
        public int Id { get; set; }
        [Column(TypeName = "numeric")]
        public decimal Price { get; set; }
        public bool Status { get; set; }
    }  
}
