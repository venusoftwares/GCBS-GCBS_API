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
    public class EyesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Eyes
        public IQueryable<Eye> GetEye()
        {
            return db.Eye;
        }

        // GET: api/Eyes/5
        [ResponseType(typeof(Eye))]
        public async Task<IHttpActionResult> GetEye(int id)
        {
            Eye eye = await db.Eye.FindAsync(id);
            if (eye == null)
            {
                return NotFound();
            }

            return Ok(eye);
        }

        // PUT: api/Eyes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEye(int id, Eye eye)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eye.Id)
            {
                return BadRequest();
            }
            using (var db2 = new DatabaseContext())
            {
                Eye eye1 = await db2.Eye.FindAsync(id);
                eye.CreatedBy = eye1.CreatedBy;
                eye.CreatedOn = eye1.CreatedOn;
                eye.Status = eye1.Status;
                db2.Dispose();
            }
            eye.UpdatedBy = userDetails.Id;
            eye.UpdatedOn = DateTime.Now;
            db.Entry(eye).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EyeExists(id))
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
        public async Task<IHttpActionResult> PutEye(EyeVisible eyeVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (eyeVisible == null)
            {
                return BadRequest();
            }
            Eye eye = await db.Eye.FindAsync(eyeVisible.Id);
            eye.Status = eyeVisible.Status;
            eye.UpdatedBy = userDetails.Id;
            eye.UpdatedOn = DateTime.Now;
            db.Entry(eye).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/Eyes
        [ResponseType(typeof(Eye))]
        public async Task<IHttpActionResult> PostEye(Eye eye)
        {
            if (eye != null)
            {
                eye.CreatedBy = userDetails.Id;
                eye.CreatedOn = DateTime.Now;
                eye.Status = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Eye.Add(eye);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = eye.Id }, eye);
        }

        // DELETE: api/Eyes/5
        [ResponseType(typeof(Eye))]
        public async Task<IHttpActionResult> DeleteEye(int id)
        {
            Eye eye = await db.Eye.FindAsync(id);
            if (eye == null)
            {
                return NotFound();
            }

            db.Eye.Remove(eye);
            await db.SaveChangesAsync();

            return Ok(eye);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EyeExists(int id)
        {
            return db.Eye.Count(e => e.Id == id) > 0;
        }
    }
}