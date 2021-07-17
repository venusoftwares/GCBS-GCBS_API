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
    public class TermsAndConditionsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
     
        // GET: api/TermsAndConditions/5
        [ResponseType(typeof(TermsAndConditions))]
        public async Task<IHttpActionResult> GetTermsAndConditions()
        {
            TermsAndConditions termsAndConditions = await db.TermsAndConditions.FindAsync(1);
            if (termsAndConditions == null)
            {
                return NotFound();
            }

            return Ok(termsAndConditions);
        }

        // PUT: api/TermsAndConditions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTermsAndConditions(TermsAndConditions termsAndConditions)
        {
            using (var d = new DatabaseContext())
            {
                var re = await d.TermsAndConditions.FindAsync(1);
                termsAndConditions.CreatedBy = re.CreatedBy;
                termsAndConditions.CreatedOn = re.CreatedOn;
                d.Dispose();
            }
            termsAndConditions.UpdatedBy = userDetails.Id;
            termsAndConditions.UpdatedOn = DateTime.Now;
            db.Entry(termsAndConditions).State = EntityState.Modified;
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