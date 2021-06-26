namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StateMaster")]
    public partial class StateMaster
    {       
        public int Id { get; set; }

        public int CountryId { get; set; }

        [Required]
        [StringLength(50)]
        public string StateName { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("CountryId")]
        public virtual CountryMaster CountryMaster { get; set; }
                                                                    
    }

    public class StateMasterVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }

    public class States
    {
        public int Id { get; set; }
        public string StateName { get; set; }
    }
}
