namespace GCBS_INTERNAL.Models.Booking
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using static GCBS_INTERNAL.Controllers.CustomerBookingCtl.CustomerToPartnerBookingController;

    [Table("CustomerBooking")]
    public partial class CustomerBooking
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public string Durarion { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        [Required]
        public string Json { get; set; }

        public JsonReponse JsonReponse
        {
            get { return JsonConvert.DeserializeObject<JsonReponse>(Json); }
        }
        public decimal BasePrice { get; set; }

        public decimal AdditionalPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal PartnerPrice { get; set; } 

        public int CustomerId { get; set; }

        public int ProviderId { get; set; }

        public int Status { get; set; }

        public int PartnerStatus { get; set; }

        public int CustomerStatus { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("ProviderId")]
        public UserManagement UserManagement { get; set; }

        [ForeignKey("CustomerId")]
        public UserManagement CustomerManagement { get; set; }

        
    }
}
