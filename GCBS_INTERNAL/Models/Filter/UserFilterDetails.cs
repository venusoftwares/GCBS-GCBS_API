namespace GCBS_INTERNAL.Models.Filter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserFilterDetails
    {
        public int id { get; set; }

        [Required]
        public string FilterJson { get; set; }

        public int UserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
