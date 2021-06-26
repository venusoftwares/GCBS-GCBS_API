namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CityMaster")]
    public partial class CityMaster
    {
               
        public int Id { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        [Required]
        [StringLength(50)]
        public string CityName { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("CountryId")]
        public CountryMaster CountryMaster { get; set; }

        [ForeignKey("StateId")]
        public StateMaster StateMaster { get; set; }            
       
    }
}
