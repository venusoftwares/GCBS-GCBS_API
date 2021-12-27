namespace GCBS_INTERNAL.Models.Support
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SupportType")]
    public partial class SupportType
    {
        public int id { get; set; }

        [Column("SupportType")]
        [StringLength(50)]
        public string SupportType1 { get; set; }
    }
}
