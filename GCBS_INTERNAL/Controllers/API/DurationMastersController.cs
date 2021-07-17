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
    public class DurationMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/DurationMasters
        public IQueryable<DurationMaster> GetDurationMaster()
        {
            return db.DurationMaster;
        }

        // GET: api/DurationMasters/5
        [ResponseType(typeof(DurationMaster))]
        public async Task<IHttpActionResult> GetDurationMaster(int id)
        {
            DurationMaster durationMaster = await db.DurationMaster.FindAsync(id);
            if (durationMaster == null)
            {
                return NotFound();
            }

            return Ok(durationMaster);
        }

        // PUT: api/DurationMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDurationMaster(int id, DurationMaster durationMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != durationMaster.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.DurationMaster.FindAsync(id);
                durationMaster.CreatedBy = re.CreatedBy;
                durationMaster.CreatedOn = re.CreatedOn;
                durationMaster.Status = re.Status;
                d.Dispose();
            }
            durationMaster.UpdatedBy = userDetails.Id;
            durationMaster.UpdatedOn = DateTime.Now;
            db.Entry(durationMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DurationMasterExists(id))
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
        // PUT: api/PriceMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDurationMaster(DurationVisible durationVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (durationVisible == null)
            {
                return BadRequest();
            }
            DurationMaster durationMaster = await db.DurationMaster.FindAsync(durationVisible.Id);
            durationMaster.Status = durationVisible.Status;
            durationMaster.UpdatedBy = userDetails.Id;
            durationMaster.UpdatedOn = DateTime.Now;
            db.Entry(durationMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/DurationMasters
        [ResponseType(typeof(DurationMaster))]
        public async Task<IHttpActionResult> PostDurationMaster(DurationMaster durationMaster)
        {
            if(durationMaster!=null)
            {
                durationMaster.CreatedBy = userDetails.Id;
                durationMaster.CreatedOn = DateTime.Now;
                durationMaster.Status = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }       
            db.DurationMaster.Add(durationMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = durationMaster.Id }, durationMaster);
        }

        // DELETE: api/DurationMasters/5
        [ResponseType(typeof(DurationMaster))]
        public async Task<IHttpActionResult> DeleteDurationMaster(int id)
        {
            DurationMaster durationMaster = await db.DurationMaster.FindAsync(id);
            if (durationMaster == null)
            {
                return NotFound();
            }

            db.DurationMaster.Remove(durationMaster);
            await db.SaveChangesAsync();

            return Ok(durationMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DurationMasterExists(int id)
        {
            return db.DurationMaster.Count(e => e.Id == id) > 0;
        }
    }
}