namespace GCBS_INTERNAL.Models
{
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
        public string MobileNo { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int LocationId { get; set; }
        public bool Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        [ForeignKey("RoleId")]
        public RoleMaster RoleMasters { get; set; }

        [ForeignKey("LocationId")]
        public LocationMasters  LocationMasters { get; set; }
    }

    public class UserManagementViewModel   
    {  
        public UserManagement UserManagements { get; set; }
        public List<string> imageBase64 { get; set; } 
        public int Age { get; set; }
        public CityMaster CityMaster { get; set; }
        public StateMaster StateMaster { get; set; }
        public CountryMaster CountryMaster { get; set; }
    }

    public class UserMasterVisible
    {
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}
