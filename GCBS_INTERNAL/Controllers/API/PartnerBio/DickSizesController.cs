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
    public class DickSizesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/DickSizes
        public IQueryable<DickSize> GetDickSize()
        {
            return db.DickSize;
        }

        // GET: api/DickSizes/5
        [ResponseType(typeof(DickSize))]
        public async Task<IHttpActionResult> GetDickSize(int id)
        {
            DickSize dickSize = await db.DickSize.FindAsync(id);
            if (dickSize == null)
            {
                return NotFound();
            }

            return Ok(dickSize);
        }

        // PUT: api/DickSizes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDickSize(int id, DickSize dickSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dickSize.Id)
            {
                return BadRequest();
            }

            using(var db2 = new DatabaseContext())
            {
                DickSize dick = await db2.DickSize.FindAsync(id);
                dickSize.CreatedBy = dick.CreatedBy;
                dickSize.CreatedOn = dick.CreatedOn;
                dickSize.Status = dick.Status;
                db2.Dispose();
            }
            dickSize.UpdatedBy = userDetails.Id;
            dickSize.UpdatedOn = DateTime.Now;
            db.Entry(dickSize).State = EntityState.Modified;         
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DickSizeExists(id))
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
        public async Task<IHttpActionResult> PutDickSize(DickSizeVisible dickSizeVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dickSizeVisible == null)
            {
                return BadRequest();
            }
            DickSize dickSize = await db.DickSize.FindAsync(dickSizeVisible.Id);
            dickSize.Status= dickSizeVisible.Status;
            dickSize.UpdatedBy = userDetails.Id;
            dickSize.UpdatedOn = DateTime.Now;
            db.Entry(dickSize).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/DickSizes
        [ResponseType(typeof(DickSize))]
        public async Task<IHttpActionResult> PostDickSize(DickSize dickSize)
        {
            if(dickSize!=null)
            {
                dickSize.CreatedBy = userDetails.Id;
                dickSize.CreatedOn = DateTime.Now;
                dickSize.Status = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }     
            db.DickSize.Add(dickSize);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = dickSize.Id }, dickSize);
        }

        // DELETE: api/DickSizes/5
        [ResponseType(typeof(DickSize))]
        public async Task<IHttpActionResult> DeleteDickSize(int id)
        {
            DickSize dickSize = await db.DickSize.FindAsync(id);
            if (dickSize == null)
            {
                return NotFound();
            }

            db.DickSize.Remove(dickSize);
            await db.SaveChangesAsync();

            return Ok(dickSize);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DickSizeExists(int id)
        {
            return db.DickSize.Count(e => e.Id == id) > 0;
        }
    }
}