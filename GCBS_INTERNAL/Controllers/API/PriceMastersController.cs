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
    public class PriceMastersController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PriceMasters
        public IQueryable<PriceMaster> GetPriceMaster()
        {
            return db.PriceMaster;
        }

        // GET: api/PriceMasters/5
        [ResponseType(typeof(PriceMaster))]
        public async Task<IHttpActionResult> GetPriceMaster(int id)
        {
            PriceMaster priceMaster = await db.PriceMaster.FindAsync(id);
            if (priceMaster == null)
            {
                return NotFound();
            }

            return Ok(priceMaster);
        }

        // PUT: api/PriceMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPriceMaster(int id, PriceMaster priceMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(priceMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceMasterExists(id))
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
        public async Task<IHttpActionResult> PutPriceMaster(PriceMasterVisible priceMasterVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (priceMasterVisible == null)
            {
                return BadRequest();
            }
            PriceMaster priceMaster = await db.PriceMaster.FindAsync(priceMasterVisible.Id);
            priceMaster.Visible = priceMasterVisible.Visible;
            db.Entry(priceMaster).State = EntityState.Modified; 
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PriceMasters
        [ResponseType(typeof(PriceMaster))]
        public async Task<IHttpActionResult> PostPriceMaster(PriceMaster priceMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PriceMaster.Add(priceMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = priceMaster.Id }, priceMaster);
        }

        // DELETE: api/PriceMasters/5
        [ResponseType(typeof(PriceMaster))]
        public async Task<IHttpActionResult> DeletePriceMaster(int id)
        {
            PriceMaster priceMaster = await db.PriceMaster.FindAsync(id);
            if (priceMaster == null)
            {
                return NotFound();
            }

            db.PriceMaster.Remove(priceMaster);
            await db.SaveChangesAsync();

            return Ok(priceMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceMasterExists(int id)
        {
            return db.PriceMaster.Count(e => e.Id == id) > 0;
        }
    }
}