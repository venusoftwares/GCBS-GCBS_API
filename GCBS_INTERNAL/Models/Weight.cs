namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Weight")]
    public partial class Weight
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Height { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
    public class WeightVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
