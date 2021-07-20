namespace GCBS_INTERNAL.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserManagement")]
    public partial class UserManagement
    {
        public int Id { get; set; }
        [StringLength(200)]  
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Username { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(200)]
        public string EmailId { get; set; }
        [Required]
        [StringLength(6)]
        public string MobileCountryCode { get; set; }
        [Required]
        public string MobileNo { get; set; }
        public DateTime? DateOfBirth { get; set; }  
        public DateTime? DateOfSignUp { get; set; }  
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }    
        public int? LocationId { get; set; }
        public int? PostalCode { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastActivateTime { get; set; }
        public bool? OnlineStatus { get; set; }
        public int? SexualOrientation { get; set; }
        public string Languages { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }        
        public string Gender { get; set; }
        public int? TitType { get; set; }
        public int? Tits { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public int? Eyes { get; set; }
        public int? Hair { get; set; }
        public int? DickSize { get; set; }  
        public bool? Smoking { get; set; }
        public bool? Drinking { get; set; }
        public string Meeting { get; set; }  
        public bool? ServiceTypeInCall { get; set; }
        public bool? ServiceTypeOutCall { get; set; }   
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("RoleId")]
        public RoleMaster RoleMasters { get; set; }

        [ForeignKey("CountryId")]
        public CountryMaster CountryMaster { get; set; }
        [ForeignKey("StateId")]
        public StateMaster StateMaster { get; set; }
        [ForeignKey("CityId")]
        public CityMaster CityMaster { get; set; }
        [ForeignKey("LocationId")]
        public LocationMasters  LocationMasters { get; set; }            
    }

    public class UserManagementViewModel   
    {  
        public UserManagement UserManagements { get; set; }
        public List<string> imageBase64 { get; set; } 
        public int Age { get; set; }   
    }

    public class UserManagementPartnerProfile
    {
        public UserManagement userManagement { get; set; }
        public int Age { get; set; }
        public List<Languages> Languages { get; set; }
   
    }
    public class Languages
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("item_text")]
        public string ItemLanguage { get; set; }
    }
    public class Meetings
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("item_text")]
        public string ItemMeeting { get; set; }
    }
    public class UserMasterVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }

    public class UserBioInformation
    {         
        public int? SelectedDickSize { get; set; }
        public int? SelectedHair { get; set; }
        public int? SelectedEyes { get; set; }
        public int? SelectedHeight { get; set; }
        public int? SelectedWeight { get; set; }
        public int? SelectedTits { get; set; }
        public int? SelectedTitType { get; set; }    
        public bool? SelectedSmoking { get; set; }
        public bool? SelectedDrinking { get; set; }
        public List<Languages> SelectedMeetings { get; set; }
        public bool? SelectedServiceTypeInCall { get; set; }
        public bool? SelectedServiceTypeOutCall { get; set; }
    }
  
}
