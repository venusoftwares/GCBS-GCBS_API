namespace GCBS_INTERNAL.Models.Payment
{
    using GCBS_INTERNAL.Models.Booking;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PartnerPayoutDetails
    {
        public int Id { get; set; }

        public int BookingNo { get; set; }

        public int PartnerId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Amount { get; set; }

        [StringLength(50)]
        public string ReferenceNo { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("BookingNo")]
        public CustomerBooking customerBooking { get; set; }
    }
}
