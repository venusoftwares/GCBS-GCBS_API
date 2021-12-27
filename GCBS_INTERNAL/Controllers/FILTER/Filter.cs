using GCBS_INTERNAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.Controllers.FILTER
{
   
        public class DropDownCom
        {
            [JsonProperty("item_id")]
            public int ItemId { get; set; }

            [JsonProperty("item_text")]
            public string ItemText { get; set; }
    }
    public class FilterResponse
    {
        public List<UserManagementViewModel> userManagementViewModels { get; set; }
        public List<DropDownCom> Location { get; set; }
        public List<DropDownCom> Gender { get; set; }
        public List<DropDownCom> Age { get; set; }
        public List<DropDownCom> HairColor { get; set; }
        public List<DropDownCom> Tits { get; set; }
        public List<DropDownCom> TitTypes { get; set; }
        public List<DropDownCom> Weight { get; set; }
        public List<DropDownCom> Height { get; set; }
        public List<DropDownCom> Eyes { get; set; }
        public List<DropDownCom> Orientation { get; set; }
        public List<DropDownCom> ServiceTypes { get; set; }


        public List<DropDownCom> SelectedLocation { get; set; }
        public List<DropDownCom> SeletedGender { get; set; }
        public List<DropDownCom> SeletedAge { get; set; }
        public List<DropDownCom> SeletedHairColor { get; set; }
        public List<DropDownCom> SeletedTits { get; set; }
        public List<DropDownCom> SeletedTitTypes { get; set; }
        public List<DropDownCom> SeletedWeight { get; set; }
        public List<DropDownCom> SeletedHeight { get; set; }
        public List<DropDownCom> SeletedEyes { get; set; }
        public List<DropDownCom> SeletedOrientation { get; set; }
        public List<DropDownCom> SeletedServiceTypes { get; set; }

    }
    public class FilterRequest
    {
        public List<DropDownCom> SelectedLocation { get; set; }
        public List<DropDownCom> SeletedGender { get; set; }
        public List<DropDownCom> SeletedAge { get; set; }
        public List<DropDownCom> SeletedHairColor { get; set; }
        public List<DropDownCom> SeletedTits { get; set; }
        public List<DropDownCom> SeletedTitTypes { get; set; }
        public List<DropDownCom> SeletedWeight { get; set; }
        public List<DropDownCom> SeletedHeight { get; set; }
        public List<DropDownCom> SeletedEyes { get; set; }
        public List<DropDownCom> SeletedOrientation { get; set; }
        public List<DropDownCom> SeletedServiceTypes { get; set; }

    }
    public class LocationFilter
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
    }

}