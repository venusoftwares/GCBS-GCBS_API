namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TermsAndConditions
    {
        
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }


        public string BookingTitle { get; set; }
        public string BookingBody { get; set; }
        public string CustomerSignUpTitle { get; set; }
        public string CustomerSignUpBody { get; set; }
        public string PartnerSignUpTitle { get; set; }
        public string PartnerSignUpBody { get; set; }
        public string CancelTitle { get; set; }
        public string CancelBody { get; set; }
        public string RejectTitle { get; set; }
        public string RejectBody { get; set; }
    }
}
