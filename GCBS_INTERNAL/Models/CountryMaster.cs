namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CountryMaster")]
    public partial class CountryMaster
    {
             
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CountryName { get; set; }

        [Required]
        [StringLength(50)]
        public string ShortName { get; set; }

        public int CountryCode { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
 
    }
    public class CountryMasterVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }

    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
    }
}
