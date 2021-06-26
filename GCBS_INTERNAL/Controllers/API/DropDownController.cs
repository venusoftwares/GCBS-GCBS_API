using GCBS_INTERNAL.Models;
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
    }
}
