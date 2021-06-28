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

namespace GCBS_INTERNAL.Controllers.API
{
    [Authorize]
    public class DropDownController : BaseApiController
    {

        private readonly DatabaseContext db = new DatabaseContext();
        // GET: api/ContactEnquiryViews
        
         
        [HttpGet]
        [Route("api/Country")]
        public IQueryable<Country> GetCountry()
        {
            return db.CountryMaster.Where(x=>x.Status).Select(x=>new Country { Id= x.Id,CountryName= x.CountryName });
        }
     
        
        // GET: api/PriceMasters/5
        [HttpGet]  
        [Route("api/States")]
        public IQueryable<States> GetStates(int CountryId)
        {
            var result = db.StateMaster.Include(x=>x.CountryMaster)
                .Where(x => x.CountryMaster.Status && x.Status && x.CountryId == CountryId)
                .Select(x => new States { Id = x.Id, StateName = x.StateName });    
            return result;
        }

        [HttpGet]
        [Route("api/Cities")]
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
    }
}
