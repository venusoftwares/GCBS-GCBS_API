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
    public class NationalityMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/NationalityMasters
        public IQueryable<NationalityMaster> GetNationalityMaster()
        {
            return db.NationalityMaster;
        }

        // GET: api/NationalityMasters/5
        [ResponseType(typeof(NationalityMaster))]
        public async Task<IHttpActionResult> GetNationalityMaster(int id)
        {
            NationalityMaster nationalityMaster = await db.NationalityMaster.FindAsync(id);
            if (nationalityMaster == null)
            {
                return NotFound();
            }

            return Ok(nationalityMaster);
        }

        // PUT: api/NationalityMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNationalityMaster(int id, NationalityMaster nationalityMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nationalityMaster.Id)
            {
                return BadRequest();
            }
            using(var d = new DatabaseContext())
            {
                var re = await d.NationalityMaster.FindAsync(id);
                nationalityMaster.CreatedBy = re.CreatedBy;
                nationalityMaster.CreatedOn = re.CreatedOn;
                d.Dispose();
            }

            nationalityMaster.UpdatedBy = userDetails.Id;
            nationalityMaster.UpdatedOn = DateTime.Now;
            db.Entry(nationalityMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NationalityMasterExists(id))
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
        public async Task<IHttpActionResult> PutNationalityMaster(NationalityVisible nationality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (nationality == null)
            {
                return BadRequest();
            }
            NationalityMaster nationalityMaster = await db.NationalityMaster.FindAsync(nationality.Id);
            nationalityMaster.Status = nationality.Status;
            nationalityMaster.UpdatedBy = userDetails.Id;
            nationalityMaster.UpdatedOn = DateTime.Now;
            db.Entry(nationalityMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        
        // POST: api/NationalityMasters
        [ResponseType(typeof(NationalityMaster))]
        public async Task<IHttpActionResult> PostNationalityMaster(NationalityMaster nationalityMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            nationalityMaster.CreatedBy = userDetails.Id;
            nationalityMaster.CreatedOn = DateTime.Now;
            db.NationalityMaster.Add(nationalityMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = nationalityMaster.Id }, nationalityMaster);
        }

        // DELETE: api/NationalityMasters/5
        [ResponseType(typeof(NationalityMaster))]
        public async Task<IHttpActionResult> DeleteNationalityMaster(int id)
        {
            NationalityMaster nationalityMaster = await db.NationalityMaster.FindAsync(id);
            if (nationalityMaster == null)
            {
                return NotFound();
            }

            db.NationalityMaster.Remove(nationalityMaster);
            await db.SaveChangesAsync();

            return Ok(nationalityMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NationalityMasterExists(int id)
        {
            return db.NationalityMaster.Count(e => e.Id == id) > 0;
        }
    }
}