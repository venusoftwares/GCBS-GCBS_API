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
    public class MarginMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/MarginMasters
        public IQueryable<MarginMaster> GetMarginMaster()
        {
            return db.MarginMaster;
        }

        // GET: api/MarginMasters/5
        [ResponseType(typeof(MarginMaster))]
        public async Task<IHttpActionResult> GetMarginMaster(int id)
        {
            MarginMaster marginMaster = await db.MarginMaster.FindAsync(id);
            if (marginMaster == null)
            {
                return NotFound();
            }

            return Ok(marginMaster);
        }

        // PUT: api/MarginMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMarginMaster(int id, MarginMaster marginMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != marginMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(marginMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarginMasterExists(id))
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
        public async Task<IHttpActionResult> PutMarginMaster(MarginMasterVisible marginMasterVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (marginMasterVisible == null)
                {
                    return BadRequest();
                }
                //MarginMaster marginMaster = await db.MarginMaster.FindAsync(marginMasterVisible.Id);
                //marginMaster.Status = marginMasterVisible.Status;
                //marginMaster.UpdatedBy = userDetails.Id;
                //marginMaster.UpdatedOn = DateTime.Now;
                //db.Entry(marginMaster).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                using (var db2 = new DatabaseContext())
                {
                    var checklist = await db2.MarginMaster.ToArrayAsync();
                    foreach (var i in checklist)
                    {
                        if (marginMasterVisible.Id == i.Id)
                        {
                            i.Status = true;
                        }
                        else
                        {
                            i.Status = false;
                        }
                        i.UpdatedBy = userDetails.Id;
                        i.UpdatedOn = DateTime.Now;
                        db2.Entry(i).State = EntityState.Modified;
                        await db2.SaveChangesAsync();
                    }

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/MarginMasters
        [ResponseType(typeof(MarginMaster))]
        public async Task<IHttpActionResult> PostMarginMaster(MarginMaster marginMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MarginMaster.Add(marginMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = marginMaster.Id }, marginMaster);
        }

        // DELETE: api/MarginMasters/5
        [ResponseType(typeof(MarginMaster))]
        public async Task<IHttpActionResult> DeleteMarginMaster(int id)
        {
            MarginMaster marginMaster = await db.MarginMaster.FindAsync(id);
            if (marginMaster == null)
            {
                return NotFound();
            }

            db.MarginMaster.Remove(marginMaster);
            await db.SaveChangesAsync();

            return Ok(marginMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MarginMasterExists(int id)
        {
            return db.MarginMaster.Count(e => e.Id == id) > 0;
        }
    }
}