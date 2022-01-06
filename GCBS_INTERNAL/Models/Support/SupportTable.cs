namespace GCBS_INTERNAL.Models.Support
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Support")]
    public partial class SupportTable
    {
        public int id { get; set; }

        [StringLength(150)]
        public string SupportType { get; set; }

        public int UserId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; } 
      
        public string Description { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
