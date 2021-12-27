using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API
{
    public class CommonLogoController : ApiController
    {
        public async Task<IHttpActionResult> GetCommonLogo()
        {
            DatabaseContext db = new DatabaseContext();
            return Ok(await db.SiteSettings.Select(x=>x.logo).FirstOrDefaultAsync());
        }
    }
}
