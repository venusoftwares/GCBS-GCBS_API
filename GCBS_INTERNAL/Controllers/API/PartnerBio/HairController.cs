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
namespace GCBS_INTERNAL.Controllers.API.PartnerBio
{
     [CustomAuthorize]
    public class HairController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Hair
        public IQueryable<Hair> GetHair()
        {
            return db.Hair;
        }

        // GET: api/Hair/5
        [ResponseType(typeof(Hair))]
        public async Task<IHttpActionResult> GetHair(int id)
        {
            Hair hair = await db.Hair.FindAsync(id);
            if (hair == null)
            {
                return NotFound();
            }

            return Ok(hair);
        }

        // PUT: api/Hair/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHair(int id, Hair hair)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hair.Id)
            {
                return BadRequest();
            }
            using (var db2 = new DatabaseContext())
            {
                Hair hair1 = await db2.Hair.FindAsync(id);
                hair.CreatedBy = hair1.CreatedBy;
                hair.CreatedOn = hair1.CreatedOn;
                hair.Status = hair1.Status;
                db2.Dispose();
            }
            hair.UpdatedBy = userDetails.Id;
            hair.UpdatedOn = DateTime.Now;
            db.Entry(hair).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HairExists(id))
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
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHair(HairVisible hairVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (hairVisible == null)
            {
                return BadRequest();
            }
            Hair hair  = await db.Hair.FindAsync(hairVisible.Id);
            hair.Status = hairVisible.Status;
            hair.UpdatedBy = userDetails.Id;
            hair.UpdatedOn = DateTime.Now;
            db.Entry(hair).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/Hair
        [ResponseType(typeof(Hair))]
        public async Task<IHttpActionResult> PostHair(Hair hair)
        {
            if (hair != null)
            {
                hair.CreatedBy = userDetails.Id;
                hair.CreatedOn = DateTime.Now;
                hair.Status = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Hair.Add(hair);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = hair.Id }, hair);
        }

        // DELETE: api/Hair/5
        [ResponseType(typeof(Hair))]
        public async Task<IHttpActionResult> DeleteHair(int id)
        {
            Hair hair = await db.Hair.FindAsync(id);
            if (hair == null)
            {
                return NotFound();
            }

            db.Hair.Remove(hair);
            await db.SaveChangesAsync();

            return Ok(hair);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HairExists(int id)
        {
            return db.Hair.Count(e => e.Id == id) > 0;
        }
    }
}