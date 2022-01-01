namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EnquiryDetails
    {
        public int Id { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionId { get; set; }
        public int UserId { get; set; }     
        public int PartnerId { get; set; }    
        public int ServiceId { get; set; }      
        public DateTime ServiceDate { get; set; }    
        public int ServiceStatus { get; set; }  
        public int PaymentStatus { get; set; }

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

    public class EnquiryViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime?  FromDate {get;set;}
        public DateTime? ToDate { get; set; }
        public DateTime ServiceDate { get; set; }

        public DateTime BookingDate { get; set; }
        public string ServicePartner { get; set; }
        public int ServicePartnerId { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public int ServiceStatus { get; set; }
        public string ServiceStatusToString { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusToString { get; set; }

        public int PartnerStatus { get; set; }

        public string TimeSlot { get; set; }
    }
    public class OpenTransactionsViewModel
    {
        public int BookingId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionId { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ServicePartner { get; set; }
        public int ServicePartnerId { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public int ServiceStatus { get; set; }
        public string ServiceStatusToString { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusToString { get; set; }
        public string TimeSlot { get; set; }
    }
    public class PastTransactionsViewModel
    {
        public int BookingId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionId { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ServicePartner { get; set; }
        public int ServicePartnerId { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public int ServiceStatus { get; set; }
        public string ServiceStatusToString { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusToString { get; set; }
        public string TimeSlot { get; set; }
    }
}
