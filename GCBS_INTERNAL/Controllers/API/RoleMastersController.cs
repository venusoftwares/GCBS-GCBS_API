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
    public class RoleMastersController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/RoleMasters
        public IQueryable<RoleMaster> GetRoleMaster()
        {
            return db.RoleMaster;
        }


        // PUT: api/PriceMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRoleMaster(RoleMasterVisible roleMasterVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (roleMasterVisible == null)
            {
                return BadRequest();
            }
            RoleMaster roleMaster = await db.RoleMaster.FindAsync(roleMasterVisible.Id);
            roleMaster.Status= roleMasterVisible.Status;
            db.Entry(roleMaster).State = EntityState.Modified;
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

        private bool RoleMasterExists(int id)
        {
            return db.RoleMaster.Count(e => e.Id == id) > 0;
        }
    }
}