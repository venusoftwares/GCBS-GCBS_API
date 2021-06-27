namespace GCBS_INTERNAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LocationMasters
    {
         
        public int Id { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        public int PinCode { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
                                               
        [ForeignKey("CountryId")]
        public CountryMaster CountryMaster { get; set; }

        [ForeignKey("StateId")]
        public StateMaster StateMaster { get; set; }          
                                                            
        [ForeignKey("CityId")]
        public virtual CityMaster CityMaster { get; set; }    
    }


    public class LocationMasterVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }

    public class LocationMasterViewModel
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public int PinCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool Status { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
    }
  
}
