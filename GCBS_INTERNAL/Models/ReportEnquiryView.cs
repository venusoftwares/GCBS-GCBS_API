namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReportEnquiryView")]
    public partial class ReportEnquiryView
    {
        public int Id { get; set; }

        public int TicketNo { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public int PartnerId { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }

        [ForeignKey("PartnerId")]
        public UserManagement PartnerManagements { get; set; }
        [ForeignKey("UserId")]
        public UserManagement UserManagements { get; set; }
    }
}
