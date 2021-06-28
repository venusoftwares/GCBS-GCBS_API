namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserManagement")]
    public partial class UserManagement
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Username { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(200)]
        public string EmailId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("RoleId")]
        public RoleMaster RoleMasters { get; set; }
    }

    public class UserMasterVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
