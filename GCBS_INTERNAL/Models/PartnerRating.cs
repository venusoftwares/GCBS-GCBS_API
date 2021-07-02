namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PartnerRating")]
    public partial class PartnerRating
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int Partnerid { get; set; }

        [Required]
        [StringLength(200)]
        public string Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
        [ForeignKey("UserId")]
        public UserManagement UserManagement { get; set; }
        [ForeignKey("Partnerid")]
        public UserManagement partnerManagement { get; set; }
    }

    public class PartnerRatingViewModel
    {
        public string UserId { get; set; }

        public string Partnerid { get; set; }

        public string Rating { get; set; }
        public string CreatedOn { get; set; }
    }
}
