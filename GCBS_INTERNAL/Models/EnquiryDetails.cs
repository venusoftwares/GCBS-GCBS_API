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

        public int UserId { get; set; }

        public int PartnerId { get; set; }

        public int ServiceId { get; set; }

        public DateTime EnquiryDate { get; set; }

        [Required]
        [StringLength(50)]
        public string EnquiryStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string UserStatus { get; set; }

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
        public string ServicePartner { get; set; }
        public int ServicePartnerId { get; set; }
        public string Service { get; set; }
        public string ServiceStatus { get; set; }
        public string UserStatus { get; set; }


    }
}
