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
    public class AboutUsContentsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();   
        // GET: api/AboutUsContents/5
        [ResponseType(typeof(AboutUsContent))]
        public async Task<IHttpActionResult> GetAboutUsContent()
        {
            AboutUsContent aboutUsContent = await db.AboutUsContent.FindAsync(1);
            if (aboutUsContent == null)
            {
                return NotFound();
            }      
            return Ok(aboutUsContent);
        }

        // PUT: api/AboutUsContents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAboutUsContent(AboutUsContent aboutUsContent)
        {
            using (var d = new DatabaseContext())
            {
                var re = await d.AboutUsContent.FindAsync(1);
                aboutUsContent.CreatedBy = re.CreatedBy;
                aboutUsContent.CreatedOn = re.CreatedOn; 
                d.Dispose();
            }
            aboutUsContent.UpdatedBy = userDetails.Id;
            aboutUsContent.UpdatedOn = DateTime.Now;
            db.Entry(aboutUsContent).State = EntityState.Modified;
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