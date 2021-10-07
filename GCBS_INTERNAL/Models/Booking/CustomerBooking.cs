namespace GCBS_INTERNAL.Models.Booking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerBooking")]
    public partial class CustomerBooking
    {
        public int Id { get; set; }

        public string ServiceLocation { get; set; }

        public string ServiceArea { get; set; }

        public string HouseOrHotel { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public string Durarion { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        public decimal BasePrice { get; set; }

        [Required]
        public string ServiceType { get; set; }

        public decimal ServicePrice { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal PartnerPrice { get; set; }
        public decimal AdminPrice { get; set; }
        
        public string Notes { get; set; }

        public int CustomerId { get; set; } 

        public int ProviderId { get; set; }

        public int Status { get; set; }
        public int PartnerStatus { get; set; }
        public int CustomerStatus { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("ProviderId")]
        public UserManagement UserManagement { get; set; }
        [ForeignKey("CustomerId")]
        public UserManagement CustomerManagement { get; set; }
    }
}
