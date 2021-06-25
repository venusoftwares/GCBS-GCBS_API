namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleMaster")]
    public partial class RoleMaster
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Dashboard { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public bool Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
    public class RoleMasterVisible
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool Status { get; set; }
    }

}
