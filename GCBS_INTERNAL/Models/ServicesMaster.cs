namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial; 
    [Table("ServicesMaster")]
    public partial class ServicesMaster
    {          
        public int Id { get; set; }
        public int ServiceTypeId { get; set; }
        [StringLength(200)]
        public string Service { get; set; }   
        public bool Visible { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
        [ForeignKey("ServiceTypeId")]
        public ServiceTypes ServiceTypes { get; set; }
    }
    public class ServiceMasterVisible
    {
        public int Id { get; set; }
        public bool Visible { get; set; }
    }
}
