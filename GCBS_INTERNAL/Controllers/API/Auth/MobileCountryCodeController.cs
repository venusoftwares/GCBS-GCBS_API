using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Auth
{
    public class MobileCountryCodeController : ApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();
        [HttpGet]
        [Route("api/mobileCountryCode")]
        public IQueryable<Country> GetCountry()
        {
            return db.CountryMaster.Where(x => x.Status).Select(x => new Country { Id = x.Id, CountryName = x.CountryName, CountryCode = x.CountryCode.ToString() });
        }
    }
}
