namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PermissionKeyValue")]
    public partial class PermissionKeyValue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Key { get; set; }

        [StringLength(50)]
        public string Value { get; set; }

        public bool Status { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
    public class PermissionKeyValueVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
    public class PermissionViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Visible { get; set; }
    }
}
