namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PayoutDetails
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string PartnerId { get; set; }

        [StringLength(50)]
        public string PartnerName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BalanceAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Payout { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
