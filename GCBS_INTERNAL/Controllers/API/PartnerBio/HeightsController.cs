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

namespace GCBS_INTERNAL.Controllers.API.PartnerBio
{
    [Authorize]
    public class HeightsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Heights
        public IQueryable<Height> GetHeight()
        {
            return db.Height;
        }

        // GET: api/Heights/5
        [ResponseType(typeof(Height))]
        public async Task<IHttpActionResult> GetHeight(int id)
        {
            Height height = await db.Height.FindAsync(id);
            if (height == null)
            {
                return NotFound();
            }

            return Ok(height);
        }

        // PUT: api/Heights/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHeight(int id, Height height)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != height.Id)
            {
                return BadRequest();
            }
            using (var db2 = new DatabaseContext())
            {
                Height height1 = await db2.Height.FindAsync(id);
                height.CreatedBy = height1.CreatedBy;
                height.CreatedOn = height1.CreatedOn;
                db2.Dispose();
            }
            height.UpdatedBy = userDetails.Id;
            height.UpdatedOn = DateTime.Now;
            db.Entry(height).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeightExists(id))
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
        public async Task<IHttpActionResult> PutHeight(HeightVisible heightVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (heightVisible == null)
            {
                return BadRequest();
            }
            Height height = await db.Height.FindAsync(heightVisible.Id);
            height.Status = heightVisible.Status;
            height.UpdatedBy = userDetails.Id;
            height.UpdatedOn = DateTime.Now;
            db.Entry(height).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Heights
        [ResponseType(typeof(Height))]
        public async Task<IHttpActionResult> PostHeight(Height height)
        {
            if (height != null)
            {
                height.CreatedBy = userDetails.Id;
                height.CreatedOn = DateTime.Now;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Height.Add(height);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = height.Id }, height);
        }

        // DELETE: api/Heights/5
        [ResponseType(typeof(Height))]
        public async Task<IHttpActionResult> DeleteHeight(int id)
        {
            Height height = await db.Height.FindAsync(id);
            if (height == null)
            {
                return NotFound();
            }

            db.Height.Remove(height);
            await db.SaveChangesAsync();

            return Ok(height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HeightExists(int id)
        {
            return db.Height.Count(e => e.Id == id) > 0;
        }
    }
}