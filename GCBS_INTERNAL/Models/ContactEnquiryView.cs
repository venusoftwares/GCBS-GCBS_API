namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactEnquiryView")]
    public partial class ContactEnquiryView
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }

        public string PhoneNumber { get; set; }    
        public int ContactType { get; set; }   
        public DateTime? Date { get; set; }

        [StringLength(250)]
        public string Description { get; set; }     
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }    
    }
}
