namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BankAccountDetails
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string BankName { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountHolderName { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountNumber { get; set; }

        [StringLength(50)]
        public string RoutingNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string IFSCCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
