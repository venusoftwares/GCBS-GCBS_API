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
    public class OrientationsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Orientations
        public IQueryable<Orientation> GetOrientation()
        {
            return db.Orientation;
        }

        // GET: api/Orientations/5
        [ResponseType(typeof(Orientation))]
        public async Task<IHttpActionResult> GetOrientation(int id)
        {
            Orientation orientation = await db.Orientation.FindAsync(id);
            if (orientation == null)
            {
                return NotFound();
            }

            return Ok(orientation);
        }

        // PUT: api/Orientations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrientation(int id, Orientation orientation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orientation.Id)
            {
                return BadRequest();
            }
            using (var db2 = new DatabaseContext())
            {
                Orientation orientation1 = await db2.Orientation.FindAsync(id);
                orientation.CreatedBy = orientation1.CreatedBy;
                orientation.CreatedOn = orientation1.CreatedOn;
                orientation.Status = orientation1.Status;
                db2.Dispose();
            }
            orientation.UpdatedBy = userDetails.Id;
            orientation.UpdatedOn = DateTime.Now;
            db.Entry(orientation).State = EntityState.Modified;      
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrientationExists(id))
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
        // PUT: api/Heights/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrientation(OrientationVisible orientationVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (orientationVisible == null)
            {
                return BadRequest();
            }
            Orientation orientation = await db.Orientation.FindAsync(orientationVisible.Id);
            orientation.Status = orientationVisible.Status;
            orientation.UpdatedBy = userDetails.Id;
            orientation.UpdatedOn = DateTime.Now;
            db.Entry(orientation).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/Orientations
        [ResponseType(typeof(Orientation))]
        public async Task<IHttpActionResult> PostOrientation(Orientation orientation)
        {
            if (orientation != null)
            {
                orientation.CreatedBy = userDetails.Id;
                orientation.CreatedOn = DateTime.Now;
                orientation.Status = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orientation.Add(orientation);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = orientation.Id }, orientation);
        }

        // DELETE: api/Orientations/5
        [ResponseType(typeof(Orientation))]
        public async Task<IHttpActionResult> DeleteOrientation(int id)
        {
            Orientation orientation = await db.Orientation.FindAsync(id);
            if (orientation == null)
            {
                return NotFound();
            }

            db.Orientation.Remove(orientation);
            await db.SaveChangesAsync();

            return Ok(orientation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrientationExists(int id)
        {
            return db.Orientation.Count(e => e.Id == id) > 0;
        }
    }
}