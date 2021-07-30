using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class DropDownController : BaseApiController
    {
        private const char Separator = '|';
        private readonly DatabaseContext db = new DatabaseContext();
        // GET: api/ContactEnquiryViews
        
         
        [HttpGet]
        [Route("api/Country")]
        public IQueryable<Country> GetCountry()
        {
            return db.CountryMaster.Where(x=>x.Status).Select(x=>new Country { Id= x.Id,CountryName= x.CountryName, CountryCode = x.CountryCode.ToString() });
        }
     
        
        // GET: api/PriceMasters/5
        [HttpGet]  
        [Route("api/States/{CountryId}")]
        public IQueryable<States> GetStates(int CountryId)
        {
            var result = db.StateMaster.Include(x=>x.CountryMaster)
                .Where(x => x.CountryMaster.Status && x.Status && x.CountryId == CountryId)
                .Select(x => new States { Id = x.Id, StateName = x.StateName });    
            return result;
        }

        [HttpGet]
        [Route("api/Cities/{CountryId}/{StateId}")]
        public IQueryable<Cities> GetCities(int CountryId,int StateId)
        {
            var result = db.CityMaster
                .Include(x => x.CountryMaster)
                .Include(x=>x.StateMaster)
                .Where(x => x.CountryMaster.Status 
                && x.StateMaster.Status
                && x.Status 
                && x.CountryId == CountryId
                && x.StateId == StateId)
                .Select(x => new Cities { Id = x.Id, CityName = x.CityName });
            return result;
        }
        [HttpGet]
        [Route("api/Locations/{CountryId}/{StateId}/{CityId}")]
        public IQueryable<Locations> GetLocations(int CountryId, int StateId,int CityId)
        {
            var result = db.LocationMasters
                .Include(x => x.CountryMaster)
                .Include(x => x.StateMaster)
                .Include(x => x.CityMaster)
                .Where(x => x.CountryMaster.Status
                && x.StateMaster.Status
                && x.Status
                && x.CountryId == CountryId
                && x.StateId == StateId 
                && x.CityId==CityId)
                .Select(x => new Locations { Id = x.Id, LocationName = x.Location + "_"+x.PinCode });
            return result;
        }
        [HttpGet]
        [Route("api/DropDownCustomers")]
        public IQueryable<DropDownCommon> GetCustomerDetails()
        {
            var result = db.UserManagement
                .Include(x => x.RoleMasters)
                .Where(x => x.RoleMasters.RoleName == Constant.CUSTOMER && x.Status)
                .Select(x => new DropDownCommon { Key = x.Id, Value = x.Id + "-" + x.Username });
            return result;
        }

        [HttpGet]
        [Route("api/DropDownServicePartner")]
        public IQueryable<DropDownCommon> GetServicePartner()
        {
            var result = db.UserManagement
                .Include(x=>x.RoleMasters)
                .Where(x=>x.RoleMasters.RoleName== Constant.SERVICE_PROVIDER && x.Status)
                .Select(x=>new DropDownCommon { Key = x.Id,Value=x.Id +"-"+ x.Username});
            return result;
        }
        [HttpGet]
        [Route("api/DropDownServiceType")]
        public IQueryable<DropDownCommon> GetServiceType()
        {
            var result = db.ServiceTypes   
                .Where( x=> x.Visible)
                .Select(x => new DropDownCommon { Key = x.Id, Value =x.ServiceType });
            return result;
        }
        [HttpGet]
        [Route("api/DropDownServices")]
        public IQueryable<DropDownCommon> GetServices()
        {
            var result = db.ServicesMasters
                .Where(x => x.Visible)
                .Select(x => new DropDownCommon { Key = x.Id, Value = x.Service });
            return result;
        }
        [HttpGet]
        [Route("api/DropDownServiceStatus")]
        public List<DropDownCommon2> GetServiceStatus()
        {
            List<DropDownCommon2> dropDownCommons = new List<DropDownCommon2>();
            dropDownCommons.Add(new DropDownCommon2 { Key = "0", Value = "--Select Status--" });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.PENDING, Value = Constant.PENDING });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.ACCEPTED, Value = Constant.ACCEPTED });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.PROCESSING, Value = Constant.PROCESSING });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.COMPLETED, Value = Constant.COMPLETED });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.DECLINE, Value = Constant.DECLINE });     
            return dropDownCommons;
        }
        [HttpGet]
        [Route("api/DropDownUserStatus")]
        public List<DropDownCommon2> GetUserStatus()
        {
            List<DropDownCommon2> dropDownCommons = new List<DropDownCommon2>();
            dropDownCommons.Add(new DropDownCommon2 { Key = "0", Value = "--Status--" });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.PENDING, Value = Constant.PENDING });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.ACCEPTED, Value = Constant.ACCEPTED });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.CANCEL, Value = Constant.CANCEL });  
            return dropDownCommons;
        }
        [HttpGet]
        [Route("api/DropDownPaymentType")]
        public List<DropDownCommon2> GetPaymentType()
        {
            List<DropDownCommon2> dropDownCommons = new List<DropDownCommon2>();
            dropDownCommons.Add(new DropDownCommon2 { Key = "0", Value = "Select Payment Type" });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.ONLINE, Value = Constant.ONLINE });
            dropDownCommons.Add(new DropDownCommon2 { Key = Constant.OFFLINE, Value = Constant.OFFLINE });  
            return dropDownCommons;
        }
        [HttpGet]
        [Route("api/DropDownSexOrientation")]
        public List<DropDownCommon> GetSexOrientation()
        {
            var result = db.Orientation   
                .Where(x=>x.Status)
                .Select(x => new DropDownCommon { Key = x.Id, Value = x.Orientation1 }).ToList();
            return result;
        }
        [HttpGet]
        [Route("api/DropDownNationality")]
        public List<DropDownCommon> GetDropNationality()
        {
            var result = db.NationalityMaster
                .Where(x => x.Status)
                .Select(x => new DropDownCommon { Key = x.Id, Value = x.Nationality }).ToList();
            return result;
        }
        [HttpGet]
        [Route("api/DropDownLanguages")]
        public List<Languages> GetDropLanguages()
        {
            var result = db.LanguageMaster
                .Where(x => x.Status)
                .Select(x => new Languages { ItemId = x.Id, ItemLanguage = x.Language }).ToList();
            return result;
        }
        [HttpGet]
        [Route("api/DropDownAgencies")]
        public List<Agencies> GetDropDownAgencies()
        {
            var result = db.AgenciesMaster
                .Where(x => x.Status)
                .Select(x => new Agencies { ItemId = x.Id, ItemAgencies = x.HotelName }).ToList();
            return result;
        }
        [HttpGet]
        [Route("api/DropDowntDickSize")]
        public List<Languages> GettDickSize()
        {
            var result = db.DickSize
                .Where(x => x.Status)
                .Select(x => new Languages { ItemId = x.Id, ItemLanguage = x.DickSize1 }).ToList();
            return result;
        }
            /// <summary>
            /// Bio information detaills drop down Get method
            /// </summary>
            /// <returns></returns>
        [HttpGet]
        [Route("api/DropDowntBioInformation")]
        public async Task<DropDownBioInformation> GetBioInformation()
        {
            try
            {
                List<Languages> meetings = new List<Languages>();
                var userinfo = await db.UserManagement.Where(x => x.Id == userDetails.Id).FirstOrDefaultAsync();
                DropDownBioInformation dropDownBioInformation = new DropDownBioInformation();
                dropDownBioInformation.DickSize = await db.DickSize.Where(x => x.Status).Select(x => new DropDownCommon { Key = x.Id, Value = x.DickSize1 }).ToListAsync();
                dropDownBioInformation.Hair = await db.Hair.Where(x => x.Status).Select(x => new DropDownCommon { Key = x.Id, Value = x.Hair1 }).ToListAsync();
                dropDownBioInformation.Eyes = await db.Eye.Where(x => x.Status).Select(x => new DropDownCommon { Key = x.Id, Value = x.Eye1 }).ToListAsync();
                dropDownBioInformation.Height = await db.Height.Where(x => x.Status).Select(x => new DropDownCommon { Key = x.Id, Value = x.Height1 }).ToListAsync();
                dropDownBioInformation.Weight = await db.Weight.Where(x => x.Status).Select(x => new DropDownCommon { Key = x.Id, Value = x.Weight1 }).ToListAsync();
                dropDownBioInformation.Tits = await db.Tit.Where(x => x.Status).Select(x => new DropDownCommon { Key = x.Id, Value = x.Tit1 }).ToListAsync();
                dropDownBioInformation.TitType = await db.TitType.Where(x => x.Status).Select(x => new DropDownCommon { Key = x.Id, Value = x.TitType1 }).ToListAsync();
                dropDownBioInformation.Meeting = await db.Meeting.Where(x => x.Status).Select(x => new Languages {  ItemId = x.Id,  ItemLanguage = x.Meeting1 }).ToListAsync();
                dropDownBioInformation.SelectedDickSize = userinfo.DickSize;
                dropDownBioInformation.SelectedHair = userinfo.Hair;
                dropDownBioInformation.SelectedEyes = userinfo.Eyes;
                dropDownBioInformation.SelectedHeight = userinfo.Height;
                dropDownBioInformation.SelectedWeight = userinfo.Weight;
                dropDownBioInformation.SelectedTits = userinfo.Tits;
                dropDownBioInformation.SelectedTitType = userinfo.TitType;
                if (userinfo.Meeting != null)
                {
                    foreach (var meeting in userinfo.Meeting.Split(Separator))
                    {
                        var lan = await db.Meeting.FindAsync(Convert.ToInt32(meeting));
                        if (lan != null)
                        {
                            if (lan.Status)
                            {
                                meetings.Add(new Languages { ItemId = lan.Id, ItemLanguage = lan.Meeting1 });
                            }
                        }

                    }
                }
                dropDownBioInformation.SelectedMeeting  = meetings;
                dropDownBioInformation.SelectedSmoking = userinfo.Smoking;
                dropDownBioInformation.SelectedDrinking = userinfo.Drinking;
                dropDownBioInformation.ServiceTypeInCall = userinfo.ServiceTypeInCall;
                dropDownBioInformation.ServiceTypeOutCall = userinfo.ServiceTypeOutCall;

                //Gender
                dropDownBioInformation.Gender = userinfo.Gender;
                                                                   
                return dropDownBioInformation;  
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public class DropDownBioInformation
        {
            public List<DropDownCommon> DickSize { get; set; }
            public List<DropDownCommon> Hair { get; set; }
            public List<DropDownCommon> Eyes { get; set; }
            public List<DropDownCommon> Height { get; set; }
            public List<DropDownCommon> Weight { get; set; }
            public List<DropDownCommon> Tits { get; set; }   
            public List<DropDownCommon> TitType { get; set; }    
            public List<Languages> Meeting { get; set; }
            public int? SelectedDickSize { get; set; }
            public int? SelectedHair { get; set; }
            public int? SelectedEyes { get; set; }
            public int? SelectedHeight { get; set; }
            public int? SelectedWeight { get; set; }
            public int? SelectedTits { get; set; }
            public int? SelectedTitType { get; set; }  
            public bool? SelectedSmoking { get; set; }
            public bool? SelectedDrinking { get; set; }
            public bool? ServiceTypeInCall { get; set; }
            public bool? ServiceTypeOutCall { get; set; }
            public List<Languages> SelectedMeeting { get; set; }
            public string Gender { get; set; }
       
        }
        [HttpGet]
        [Route("api/DropDownContactEnquiryType")]
        public IQueryable<DropDownCommon> DropDownContactEnquiryType()
        {
            var result = db.ContactEnquiryTypes
                .Where(x => x.Status)
                .Select(x => new DropDownCommon { Key = x.Id, Value = x.ContactEnquiryType1 });
            return result;
        }

        [HttpGet]
        [Route("api/DropDownSupportEnquiryType")]
        public IQueryable<DropDownCommon> DropDownSupportEnquiryType()
        {
            var result = db.SupportEnquiryTypes
                .Where(x => x.Status)
                .Select(x => new DropDownCommon { Key = x.Id, Value = x.SupportEnquiryType1 });
            return result;
        }

    }
}
