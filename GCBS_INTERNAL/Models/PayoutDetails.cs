namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayoutDetails
    {
        public int Id { get; set; }

        public DateTime PayoutDate { get; set; }
        [StringLength(50)]
        public string PartnerId { get; set; }

        [StringLength(50)]
        public string PartnerName { get; set; }    
        public double Payment { get; set; }
        public string Status { get; set; }
        
        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("PartnerId")]
        public UserManagement UserManagement { get; set; }
    }
    public class PayoutViewModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string Status { get; set; }
        public double Payment { get; set; }
    }
}
