 
using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Filter;
using GCBS_INTERNAL.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.FILTER
{
    [CustomAuthorize]
    public class FilterLocationController : BaseApiController
    {
        public DatabaseContext db = new DatabaseContext();

        [HttpGet]
        [Route("api/GetCurrentLocationFilter")]
        public IHttpActionResult GetCurrentLocationFilter(double fromlat, double fromlon, double tolat, double tolon)
        {
            //var sCoord = new GeoCoordinate(11.1205, 77.3322);

            //var eCoord = new GeoCoordinate(11.103830, 77.391910);
            var sCoord = new GeoCoordinate(fromlat, fromlon);

            var eCoord = new GeoCoordinate(tolat, tolon);

            var result = sCoord.GetDistanceTo(eCoord) / 1000;

            return Ok(result);
        }
        [HttpGet]
        [Route("api/GetGcbsFilter")]
        public async Task<IHttpActionResult> GetGcbsFilter()
        {
            return Ok(await GetFilterResponse());

        }

        private async Task<FilterResponse> GetFilterResponse()
        {
            var response = await GetLastFilterResponseAsync();

            FilterResponse filterResponse = new FilterResponse()
            {
                Location = GetLocation(),
                Age = GetAge(),
                Eyes = await db.Eye.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Eye1 }).ToListAsync(),
                Gender = GetGender(),
                HairColor = await db.Hair.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Hair1 }).ToListAsync(),
                Height = await db.Height.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Height1 }).ToListAsync(),
                Weight = await db.Height.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Height1 }).ToListAsync(),
                Orientation = await db.Orientation.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Orientation1 }).ToListAsync(),
                ServiceTypes = await db.PartnerServiceType.Select(x => new DropDownCom { ItemId = x.id, ItemText = x.ServiceType }).ToListAsync(),
                Tits = await db.Tit.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Tit1 }).ToListAsync(),
                TitTypes = await db.TitType.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.TitType1 }).ToListAsync(),
                SeletedAge = response.SeletedAge,
                SeletedEyes = response.SeletedEyes,
                SeletedGender = response.SeletedGender,
                SeletedHairColor = response.SeletedHairColor,
                SeletedHeight = response.SeletedHeight,
                SeletedOrientation = response.SeletedOrientation,
                SeletedServiceTypes = response.SeletedServiceTypes,
                SeletedTits = response.SeletedTits,
                SeletedTitTypes = response.SeletedTitTypes,
                SeletedWeight = response.SeletedWeight,
                SelectedLocation = response.SelectedLocation
            }; 

            return filterResponse;
        }

        [HttpPost]
        [Route("api/SubmitGcbsFilter")]
        public async Task<IHttpActionResult> SubmitGcbsFilter(FilterRequest filterRequest)
        {
            try
            {
                await InsertOrUpdate(filterRequest);

                var list =await GetFilterResponse();

                FilterResponse filterResponse = new FilterResponse()
                {
                    Location = GetLocation(),
                    Age = GetAge(),
                    Eyes = await db.Eye.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Eye1 }).ToListAsync(),
                    Gender = GetGender(),
                    HairColor = await db.Hair.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Hair1 }).ToListAsync(),
                    Height = await db.Height.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Height1 }).ToListAsync(),
                    Weight = await db.Height.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Height1 }).ToListAsync(),
                    Orientation = await db.Orientation.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Orientation1 }).ToListAsync(),
                    ServiceTypes = await db.PartnerServiceType.Select(x => new DropDownCom { ItemId = x.id, ItemText = x.ServiceType }).ToListAsync(),
                    Tits = await db.Tit.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.Tit1 }).ToListAsync(),
                    TitTypes = await db.TitType.Select(x => new DropDownCom { ItemId = x.Id, ItemText = x.TitType1 }).ToListAsync(),
                    SeletedAge = list.SeletedAge,
                    SeletedEyes = list.SeletedEyes,
                    SeletedGender = list.SeletedGender,
                    SeletedHairColor = list.SeletedHairColor,
                    SeletedHeight = list.SeletedHeight,
                    SeletedOrientation = list.SeletedOrientation,
                    SeletedServiceTypes = list.SeletedServiceTypes,
                    SeletedTits = list.SeletedTits,
                    SeletedTitTypes = list.SeletedTitTypes,
                    SeletedWeight = list.SeletedWeight,
                    SelectedLocation = list.SelectedLocation,
                    userManagementViewModels = await PartnerDetails(filterRequest)
                };

                

                return Ok(filterResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<UserManagementViewModel>> PartnerDetails(FilterRequest filterRequest)
        {
            log.Info("Called");
            List<UserManagementViewModel> list = new List<UserManagementViewModel>();
            var res = await db.UserManagement
                .Include(x => x.CountryMaster)
                .Include(x => x.StateMaster)
                .Include(x => x.CityMaster)
                .Where(x => x.Status && x.RoleId == 3)
                .OrderBy(x => x.LastLogin)
                .ToListAsync();

            //Filter Location
            if (filterRequest.SelectedLocation.Count > 0)
            {
                res = res.Where(x => x.CityId != null).ToList();
                List<int> vs = filterRequest.SelectedLocation.Select(x => x.ItemId).ToList();
                res = res.Where(x => vs.Contains((int)x.CityId)).ToList();
            }

            ////Filter Age
            //if (filterRequest.SelectedLocation.Count > 0)
            //{
            //    List<int> vs = filterRequest.SeletedAge.Select(x => x.ItemId).ToList();
            //    res = res.Where(x => vs.Contains((int)x.)).ToList();
            //}

            //Filter ESeletedEyesyes
            if (filterRequest.SeletedEyes.Count > 0)
            {
                
                List<int> vs = filterRequest.SeletedEyes.Select(x => x.ItemId).ToList();
                res = res.Where(x => vs.Contains((int)x.Eyes)).ToList();
            }

            //Filter SeletedGender
            if (filterRequest.SeletedGender.Count > 0)
            {
               
                List<string> vs = filterRequest.SeletedGender.Select(x => x.ItemText).ToList();
                res = res.Where(x => vs.Contains(x.Gender)).ToList();
            }

            //Filter SeletedHairColor
            if (filterRequest.SeletedHairColor.Count > 0)
            {
                
                List<int> vs = filterRequest.SeletedHairColor.Select(x => x.ItemId).ToList();
                res = res.Where(x => vs.Contains((int)x.Hair)).ToList();
            }
            //Filter SeletedHeight
            if (filterRequest.SeletedHeight.Count > 0)
            {
                List<int> vs = filterRequest.SeletedHeight.Select(x => x.ItemId).ToList();
                res = res.Where(x => vs.Contains((int)x.Height)).ToList();
            }

            //Filter SeletedOrientation
            if (filterRequest.SeletedOrientation.Count > 0)
            {
                
                List<int> vs = filterRequest.SeletedOrientation.Select(x => x.ItemId).ToList();
                res = res.Where(x => vs.Contains((int)x.SexualOrientation)).ToList();
            }



            //Filter SeletedTits
            if (filterRequest.SeletedTits.Count > 0)
            {
                List<int> vs = filterRequest.SeletedTits.Select(x => x.ItemId).ToList();
                res = res.Where(x => vs.Contains((int)x.Tits)).ToList();
            }

            //Filter SeletedTitTypes
            if (filterRequest.SeletedTitTypes.Count > 0)
            {
                List<int> vs = filterRequest.SeletedTitTypes.Select(x => x.ItemId).ToList();
                res = res.Where(x => vs.Contains((int)x.TitType)).ToList();
            }

            //Filter SeletedWeight
            if (filterRequest.SeletedWeight.Count > 0)
            {
                List<int> vs = filterRequest.SeletedWeight.Select(x => x.ItemId).ToList();
                res = res.Where(x => vs.Contains((int)x.Weight)).ToList();
            }


            //Filter ServiceTypes
            if (filterRequest.SeletedAge.Count > 0)
            {
                List<int> vs = filterRequest.SeletedAge.Select(x => x.ItemId).ToList();
                res = userManagementsDetails(res, vs);
            }

            //filter gender 
            if (filterRequest.SeletedGender.Count > 0)
            { 
                List<string> vs = filterRequest.SeletedGender.Select(x => x.ItemText).ToList();
                res = GenderList(res, vs);
            }

            foreach (var a in res)
            {
                List<string> img = new List<string>();
                img.Add(a.Image);
                // First or default 
                //var path = imgser.GetFiles(a.Id, Constant.SERVICE_PROVIDER_FOLDER_TYPE);  
                list.Add(new UserManagementViewModel
                {
                    UserManagements = a,
                    imageBase64 = img,
                    Age = (DateTime.Now.Year - Convert.ToDateTime(a.DateOfBirth).Year)

                });
            }
            log.Info("End");
            return list;
        }

        private List<UserManagement> GenderList(List<UserManagement> user, List<string> gender)
        {
            List<UserManagement> userManagements = new List<UserManagement>();

            foreach (var gende in gender)
            {

                foreach (var list in user)
                {
                    if (list.Gender == gende)
                    {
                        if (!userManagements.Any(x => x.Id == list.Id))
                        {
                            userManagements.Add(list);
                        }
                    }

                }
            }
            return userManagements;
        }

        private List<UserManagement> userManagementsDetails(List<UserManagement> user, List<int> id)
        {
            List<UserManagement> userManagements = new List<UserManagement>();

            foreach (var age in id)
            {
                var age2 = 0;
                if (age == 18)

                {
                    age2 = 18 + 7;
                }
                else
                {
                    age2 = age + 5;
                }

                DateTime dateTime = DateTime.Now.Date.AddYears(-age2);

                foreach (var list in user)
                {
                    if (list.DateOfBirth >= dateTime)
                    {
                        if (!userManagements.Any(x => x.Id == list.Id))
                        {
                            userManagements.Add(list);
                        }
                    }

                }
            }

            return userManagements;


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

        public class DropDownCom
        {
            [JsonProperty("item_id")]
            public int ItemId { get; set; }

            [JsonProperty("item_text")]
            public string ItemText { get; set; }
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

        public List<DropDownCom> GetLocation()
        {
            SqlParameter startDate = new SqlParameter("@startDate", "Value");
            SqlParameter endDate = new SqlParameter("@endDate", "Value");
            //var list = db.Database.SqlQuery<LocationFilter>("exec yourStoreProcedureName @startDate, @endDate", startDate, endDate).ToList();
            var list = db.Database.SqlQuery<LocationFilter>("exec getLocation", startDate, endDate).ToList();
            return list.Select(x => new DropDownCom { ItemId = x.Id, ItemText = $"{FildEmpty(x.CountryName)}{FildEmpty(x.StateName)}{FildEmpty(x.CityName)}{AFildEmpty(x.Address) }" }).ToList();

        }
        public string FildEmpty(string value)
        {
            return !string.IsNullOrEmpty(value) ? value + ", " : "";
        }
        public string AFildEmpty(string value)
        {
            return !string.IsNullOrEmpty(value) ? value  : "";
        }
        public class LocationFilter
        {
            public int Id { get; set; }
            public string CountryName { get; set; }
            public string StateName { get; set; }
            public string CityName { get; set; }
            public string Address { get; set; }
        }

        private async Task<FilterRequest> GetLastFilterResponseAsync()
        {
            var response =await db.UserFilterDetails.Where(x => x.UserId == userDetails.Id).FirstOrDefaultAsync();

            if(response!=null)
            {
                FilterRequest filterResponse = JsonConvert.DeserializeObject<FilterRequest>(response.FilterJson);
                return filterResponse;
            }
            else
            {
                FilterRequest filterResponse = new FilterRequest
                {
                    SelectedLocation = new List<DropDownCom>(),
                    SeletedAge = new List<DropDownCom>(),
                    SeletedEyes = new List<DropDownCom>(),
                    SeletedGender = new List<DropDownCom>(),
                    SeletedHairColor = new List<DropDownCom>(),
                    SeletedHeight = new List<DropDownCom>(),
                    SeletedOrientation = new List<DropDownCom>(),
                    SeletedServiceTypes = new List<DropDownCom>(),
                    SeletedTits = new List<DropDownCom>(),
                    SeletedTitTypes = new List<DropDownCom>(),
                    SeletedWeight = new List<DropDownCom>(),
                };
                return filterResponse;
            }

        }
        private async Task InsertOrUpdate(FilterRequest filterRequest)
        {
            var response = await db.UserFilterDetails.Where(x => x.UserId == userDetails.Id).FirstOrDefaultAsync();
            if(response!=null)
            {
                var user =await db.UserFilterDetails.Where(x => x.UserId == userDetails.Id).FirstOrDefaultAsync();
                user.FilterJson = JsonConvert.SerializeObject(filterRequest);
                user.UpdatedDate = DateTime.Now;
                
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else
            {
                UserFilterDetails user = new UserFilterDetails();
                user.CreatedDate = DateTime.Now;
                user.FilterJson = JsonConvert.SerializeObject(filterRequest);
                user.UserId = userDetails.Id;
                db.UserFilterDetails.Add(user);
                await db.SaveChangesAsync();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
