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
    public class DisclaimerContentsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();     

        // GET: api/DisclaimerContents/5
        [ResponseType(typeof(DisclaimerContent))]
        public async Task<IHttpActionResult> GetDisclaimerContent()
        {
            DisclaimerContent disclaimerContent = await db.DisclaimerContent.FindAsync(1);
            if (disclaimerContent == null)
            {
                return NotFound();
            }            
            return Ok(disclaimerContent);
        }        
        // PUT: api/DisclaimerContents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDisclaimerContent(DisclaimerContent disclaimerContent)
        {
            using (var d = new DatabaseContext())
            {
                var re = await d.DisclaimerContent.FindAsync(1);
                disclaimerContent.CreatedBy = re.CreatedBy;
                disclaimerContent.CreatedOn = re.CreatedOn;
                d.Dispose();
            }
            disclaimerContent.UpdatedBy = userDetails.Id;
            disclaimerContent.UpdatedOn = DateTime.Now;    
            db.Entry(disclaimerContent).State = EntityState.Modified;
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