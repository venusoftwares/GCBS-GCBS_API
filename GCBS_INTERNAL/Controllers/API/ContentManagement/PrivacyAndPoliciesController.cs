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
    public class PrivacyAndPoliciesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();    
        // GET: api/PrivacyAndPolicies/5
        [ResponseType(typeof(PrivacyAndPolicy))]
        public async Task<IHttpActionResult> GetPrivacyAndPolicy()
        {
            PrivacyAndPolicy privacyAndPolicy = await db.PrivacyAndPolicy.FindAsync(1);
            if (privacyAndPolicy == null)
            {
                return NotFound();
            }          
            return Ok(privacyAndPolicy);
        }

        // PUT: api/PrivacyAndPolicies/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPrivacyAndPolicy(PrivacyAndPolicy privacyAndPolicy)
        {
          
            using (var d = new DatabaseContext())
            {
                var re = await d.PrivacyAndPolicy.FindAsync(1);
                privacyAndPolicy.CreatedBy = re.CreatedBy;
                privacyAndPolicy.CreatedOn = re.CreatedOn;
                d.Dispose();
            }
            privacyAndPolicy.UpdatedBy = userDetails.Id;
            privacyAndPolicy.UpdatedOn = DateTime.Now;
            db.Entry(privacyAndPolicy).State = EntityState.Modified;
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