using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class Warning18ContentController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

     

        // GET: api/Warning18Content/5
        [ResponseType(typeof(Warning18Content))]
        public async Task<IHttpActionResult> GetWarning18Content()
        {
            Warning18Content warning18Content = await db.Warning18Content.FindAsync(1);
            if (warning18Content == null)
            {
                return NotFound();
            }  
            return Ok(warning18Content);
        }

        // PUT: api/Warning18Content/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWarning18Content(Warning18Content warning18Content)
        {
            using (var d = new DatabaseContext())
            {
                var re = await d.Warning18Content.FindAsync(1);
                warning18Content.CreatedBy = re.CreatedBy;
                warning18Content.CreatedOn = re.CreatedOn;
                d.Dispose();
            }
            warning18Content.UpdatedBy = userDetails.Id;
            warning18Content.UpdatedOn = DateTime.Now;
            db.Entry(warning18Content).State = EntityState.Modified;
            await db.SaveChangesAsync();   
            return StatusCode(HttpStatusCode.NoContent);
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