using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.FILTER
{
    public class FilterDefaultLocationController : ApiController
    {
        public DatabaseContext db = new DatabaseContext();
        [HttpGet]
        [Route("api/GetGcbsFilter")]
        public async Task<IHttpActionResult> GetGcbsFilter()
        {
            return Ok(await GetFilterResponse());

        }


        private async Task<FilterResponse> GetFilterResponse()
        {
            //var response = await GetLastFilterResponseAsync();

            FilterResponse filterResponse = new FilterResponse()
            {
                Location = GetLocation(),
                Age = GetAge(),
                Eyes = await db.Eye.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Eye1 }).ToListAsync(),
                Gender = GetGender(),
                HairColor = await db.Hair.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Hair1 }).ToListAsync(),
                Height = await db.Height.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Height1 }).ToListAsync(),
                Weight = await db.Weight.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Weight1 }).ToListAsync(),
                Orientation = await db.Orientation.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Orientation1 }).ToListAsync(),
                ServiceTypes = await db.PartnerServiceType.Select(x => new DropDownCom { ItemId = x.id, ItemText = x.ServiceType }).ToListAsync(),
                Tits = await db.Tit.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Tit1 }).ToListAsync(),
                TitTypes = await db.TitType.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.TitType1 }).ToListAsync(),
                //SeletedAge = response.SeletedAge,
                //SeletedEyes = response.SeletedEyes,
                //SeletedGender = response.SeletedGender,
                //SeletedHairColor = response.SeletedHairColor,
                //SeletedHeight = response.SeletedHeight,
                //SeletedOrientation = response.SeletedOrientation,
                //SeletedServiceTypes = response.SeletedServiceTypes,
                //SeletedTits = response.SeletedTits,
                //SeletedTitTypes = response.SeletedTitTypes,
                //SeletedWeight = response.SeletedWeight,
                //SelectedLocation = response.SelectedLocation
            };

            return filterResponse;
        }

        public List<DropDownCom> GetAge()
        {
            List<DropDownCom> DropDownComs = new List<DropDownCom>
            {
                new DropDownCom{ ItemId = 18, ItemText = "18 To 25"},
                new DropDownCom{ ItemId = 26, ItemText = "26 To 30"},
                new DropDownCom{ ItemId = 31, ItemText = "31 To 35"},
                new DropDownCom{ ItemId = 36, ItemText = "36 To 40"},
                new DropDownCom{ ItemId = 41, ItemText = "41 To 45"},
                new DropDownCom{ ItemId = 45, ItemText = "45 To 50"},
                new DropDownCom{ ItemId = 51, ItemText = "51 To 55"},
                new DropDownCom{ ItemId = 56, ItemText = "56 To 60"},
                new DropDownCom{ ItemId = 61, ItemText = "61 To 65"},
                new DropDownCom{ ItemId = 66, ItemText = "66 To 70"}
            };
            return DropDownComs;
        }
        public List<DropDownCom> GetGender()
        {
            List<DropDownCom> DropDownComs = new List<DropDownCom>
            {
                new DropDownCom{ ItemId = 1, ItemText = "Male"},
                new DropDownCom{ ItemId = 2, ItemText = "Female"},
                new DropDownCom{ ItemId = 3, ItemText = "Trangender"}
            };
            return DropDownComs;
        }
        public string FildEmpty(string value)
        {
            return !string.IsNullOrEmpty(value) ? value + ", " : "";
        }
        public string AFildEmpty(string value)
        {
            return !string.IsNullOrEmpty(value) ? value : "";
        }
        public List<DropDownCom> GetLocation()
        {
            SqlParameter startDate = new SqlParameter("@startDate", "Value");
            SqlParameter endDate = new SqlParameter("@endDate", "Value");
            //var list = db.Database.SqlQuery<LocationFilter>("exec yourStoreProcedureName @startDate, @endDate", startDate, endDate).ToList();
            var list = db.Database.SqlQuery<LocationFilter>("exec getLocation", startDate, endDate).ToList();
            return list.Select(x => new DropDownCom { ItemId = x.Id, ItemText = $"{FildEmpty(x.CountryName)}{FildEmpty(x.StateName)}{FildEmpty(x.CityName)}{AFildEmpty(x.Address) }" }).ToList();

        }
    }
}
