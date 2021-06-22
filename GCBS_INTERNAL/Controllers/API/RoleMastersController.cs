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

        // GET: api/RoleMasters/5
        [ResponseType(typeof(RoleMaster))]
        public async Task<IHttpActionResult> GetRoleMaster(int id)
        {
            RoleMaster roleMaster = await db.RoleMaster.FindAsync(id);
            if (roleMaster == null)
            {
                return NotFound();
            }

            return Ok(roleMaster);
        }

        // PUT: api/RoleMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRoleMaster(int id, RoleMaster roleMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roleMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(roleMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RoleMasters
        [ResponseType(typeof(RoleMaster))]
        public async Task<IHttpActionResult> PostRoleMaster(RoleMaster roleMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RoleMaster.Add(roleMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = roleMaster.Id }, roleMaster);
        }

        // DELETE: api/RoleMasters/5
        [ResponseType(typeof(RoleMaster))]
        public async Task<IHttpActionResult> DeleteRoleMaster(int id)
        {
            RoleMaster roleMaster = await db.RoleMaster.FindAsync(id);
            if (roleMaster == null)
            {
                return NotFound();
            }

            db.RoleMaster.Remove(roleMaster);
            await db.SaveChangesAsync();

            return Ok(roleMaster);
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