namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PaymentGateway")]
    public partial class PaymentGateway
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string SelectAccount { get; set; }

        [StringLength(50)]
        public string GateWayName { get; set; }

        [StringLength(50)]
        public string CurrencyCode { get; set; }

        [StringLength(300)]
        public string URL { get; set; }

        [StringLength(50)]
        public string MerchantId { get; set; }

        [StringLength(50)]
        public string AccessCode { get; set; }

        [StringLength(50)]
        public string WorkingKey { get; set; }

        public bool? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
