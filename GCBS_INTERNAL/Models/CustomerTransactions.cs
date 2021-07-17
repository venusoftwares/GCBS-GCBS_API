namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerTransactions
    {
        public int Id { get; set; }

        public int ServiceId { get; set; }

        public string BookingDate { get; set; }

        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(7)]
        public string PaymentType { get; set; }

        public int UserId { get; set; }

        public int PartnerId { get; set; }   
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMode { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("PartnerId")]
        public UserManagement PartnerManagements { get; set; }
        [ForeignKey("UserId")]
        public UserManagement UserManagements { get; set; }
        [ForeignKey("ServiceId")]
        public ServicesMaster servicesMasters { get; set; }
    }
    public class CustomerTransactionsViewModel
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public int BookingId { get; set; }
        public string Username { get; set; }
        public string UserMobile { get; set; }
        public string BookingDate { get; set; }
        public int ServiceId { get; set; }
        public string ServiceCategory { get; set; }
        public string PartnerName { get; set; }
        public int PartnerId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public DateTime? PaymentDateFrom { get; set; }
        public DateTime? PaymentDateTo { get; set; }

    }
}
