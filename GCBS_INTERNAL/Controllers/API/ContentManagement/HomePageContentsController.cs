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

namespace GCBS_INTERNAL.Controllers.API
{
    [Authorize]
    public class HomePageContentsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

      
        // GET: api/HomePageContents/5
        [ResponseType(typeof(HomePageContent))]
        public async Task<IHttpActionResult> GetHomePageContent()
        {
            HomePageContent homePageContent = await db.HomePageContent.FindAsync(1);
            if (homePageContent == null)
            {
                return NotFound();
            }

            return Ok(homePageContent);
        }

        // PUT: api/HomePageContents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHomePageContent(HomePageContent homePageContent)
        {
            using (var d = new DatabaseContext())
            {
                var re = await d.HomePageContent.FindAsync(1);
                homePageContent.CreatedBy = re.CreatedBy;
                homePageContent.CreatedOn = re.CreatedOn;   
                d.Dispose();
            }
            homePageContent.UpdatedBy = userDetails.Id;
            homePageContent.UpdatedOn = DateTime.Now;     
            db.Entry(homePageContent).State = EntityState.Modified;
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