namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("AgenciesMaster")]
    public partial class AgenciesMaster
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string HotelName { get; set; }    

        public int Location { get; set; }    

        [StringLength(500)]
        public string WebsiteUrl { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        public DateTime ValidStartDate { get; set; }

        public DateTime ValidEndDate { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
        [ForeignKey("Location")]
        public LocationMasters LocationMasters { get; set; }
    }

    public class AgenciesVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }

    public class AgenciesMasterViewModel
    {
        public LocationMasters LocationMasters { get; set; }
        public AgenciesMaster AgenciesMaster { get; set; }
        public List<string> imageBase64 { get; set; }
    }
    public class AgenciesMasterView
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
        [ForeignKey("Location")]
        public virtual LocationMasters LocationMasters { get; set; }
    }
     
}
