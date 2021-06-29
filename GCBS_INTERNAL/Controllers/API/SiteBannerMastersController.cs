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
    public class SiteBannerMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/SiteBannerMasters
        public IQueryable<SiteBannerMaster> GetSiteBannerMasters()
        {
            return db.SiteBannerMasters;
        }

        // GET: api/SiteBannerMasters/5
        [ResponseType(typeof(SiteBannerMaster))]
        public async Task<IHttpActionResult> GetSiteBannerMaster(int id)
        {
            SiteBannerMaster siteBannerMaster = await db.SiteBannerMasters.FindAsync(id);
            if (siteBannerMaster == null)
            {
                return NotFound();
            }

            return Ok(siteBannerMaster);
        }

        // PUT: api/SiteBannerMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSiteBannerMaster(int id, SiteBannerMaster siteBannerMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != siteBannerMaster.Id)
            {
                return BadRequest();
            }
            using(var db2 = new DatabaseContext())
            {
                SiteBannerMaster siteBanner = await db2.SiteBannerMasters.FindAsync(id);
                siteBannerMaster.CreatedOn = DateTime.Now;
                siteBannerMaster.CreatedBy = userDetails.Id;
                db2.Dispose();
            }
            siteBannerMaster.UpdatedOn = DateTime.Now;
            siteBannerMaster.UpdatedBy = userDetails.Id;
            db.Entry(siteBannerMaster).State = EntityState.Modified;    
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteBannerMasterExists(id))
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
        public async Task<IHttpActionResult> PutSiteBannerMaster(SiteBannerVisible siteBannerVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (siteBannerVisible == null)
            {
                return BadRequest();
            }
            SiteBannerMaster siteBannerMaster = await db.SiteBannerMasters.FindAsync(siteBannerVisible.Id);
            siteBannerMaster.Status = siteBannerMaster.Status;
            siteBannerMaster.UpdatedOn = DateTime.Now;
            siteBannerMaster.UpdatedBy = userDetails.Id;
            db.Entry(siteBannerMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SiteBannerMasters
        [ResponseType(typeof(SiteBannerMaster))]
        public async Task<IHttpActionResult> PostSiteBannerMaster(SiteBannerMaster siteBannerMaster)
        {
            if(siteBannerMaster!=null)
            {
                siteBannerMaster.CreatedBy = userDetails.Id;
                siteBannerMaster.CreatedOn = DateTime.Now;
            }     
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SiteBannerMasters.Add(siteBannerMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = siteBannerMaster.Id }, siteBannerMaster);
        }

        // DELETE: api/SiteBannerMasters/5
        [ResponseType(typeof(SiteBannerMaster))]
        public async Task<IHttpActionResult> DeleteSiteBannerMaster(int id)
        {
            SiteBannerMaster siteBannerMaster = await db.SiteBannerMasters.FindAsync(id);
            if (siteBannerMaster == null)
            {
                return NotFound();
            }

            db.SiteBannerMasters.Remove(siteBannerMaster);
            await db.SaveChangesAsync();

            return Ok(siteBannerMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SiteBannerMasterExists(int id)
        {
            return db.SiteBannerMasters.Count(e => e.Id == id) > 0;
        }
    }
}