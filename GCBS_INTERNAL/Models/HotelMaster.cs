namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelMaster")]
    public partial class HotelMaster
    {
        public int Id { get; set; }

        [Required]
        public string HotelName { get; set; }
        [Required]
        public int Location { get; set; }  
        public string Images { get; set; }

        [Required]
        public string WebsiteUrl { get; set; }
        [Required]
        [StringLength(200)]
        public string Email { get; set; }
        [Required]
        public DateTime ValidStartDate { get; set; }
        [Required]
        public DateTime ValidEndDate { get; set; }   
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("Location")]
        public LocationMasters LocationMasters { get; set; }
    }
    public class HotelMasterVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
    public class HotelMasterViewModel
    {
        public HotelMaster HotelMaster { get; set; }
        public List<string> imageBase64 { get; set; }
        public LocationMasters LocationMasters { get; set; }
    }
    public class HotelMasterView
    {
        public int Id { get; set; }
        public string HotelName { get; set; }
        public int Location { get; set; }    
        public string Image { get; set; }
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }         
        public string ValidStartDate { get; set; }     
        public string ValidEndDate { get; set; }
        public bool Status { get; set; }
        public LocationMasters LocationMasters { get; set; }
    }
}
