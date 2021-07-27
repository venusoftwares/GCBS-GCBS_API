namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CalenderDetails
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string cssClass { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }    
    }
}
