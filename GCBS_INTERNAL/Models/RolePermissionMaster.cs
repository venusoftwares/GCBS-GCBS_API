namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RolePermissionMaster")]
    public partial class RolePermissionMaster
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int MenuId { get; set; }

        [StringLength(250)]
        public string Privilege { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
    public class RolePermissionRequest
    {     
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public bool[] Permission { get; set; }
    }
}
