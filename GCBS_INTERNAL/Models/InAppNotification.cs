namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InAppNotification")]
    public partial class InAppNotification
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(250)]
        public string Content { get; set; }

        public bool NotificationALL { get; set; }

        public bool Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
