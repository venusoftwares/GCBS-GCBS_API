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
    public class TitsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Tits
        public IQueryable<Tit> GetTit()
        {
            return db.Tit;
        }

        // GET: api/Tits/5
        [ResponseType(typeof(Tit))]
        public async Task<IHttpActionResult> GetTit(int id)
        {
            Tit tit = await db.Tit.FindAsync(id);
            if (tit == null)
            {
                return NotFound();
            }

            return Ok(tit);
        }

        // PUT: api/Tits/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTit(int id, Tit tit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tit.Id)
            {
                return BadRequest();
            }
            using (var db2 = new DatabaseContext())
            {
                Tit tit1 = await db2.Tit.FindAsync(id);
                tit.CreatedBy = tit1.CreatedBy;
                tit.CreatedOn = tit1.CreatedOn;
                db2.Dispose();
            }
            tit.UpdatedBy = userDetails.Id;
            tit.UpdatedOn = DateTime.Now;
            db.Entry(tit).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TitExists(id))
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
        public async Task<IHttpActionResult> PutTit(TitVisible titVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (titVisible == null)
            {
                return BadRequest();
            }
            Tit tit = await db.Tit.FindAsync(titVisible.Id);
            tit.Status = titVisible.Status;
            tit.UpdatedBy = userDetails.Id;
            tit.UpdatedOn = DateTime.Now;
            db.Entry(tit).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/Tits
        [ResponseType(typeof(Tit))]
        public async Task<IHttpActionResult> PostTit(Tit tit)
        {
            if (tit != null)
            {
                tit.CreatedBy = userDetails.Id;
                tit.CreatedOn = DateTime.Now;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tit.Add(tit);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tit.Id }, tit);
        }

        // DELETE: api/Tits/5
        [ResponseType(typeof(Tit))]
        public async Task<IHttpActionResult> DeleteTit(int id)
        {
            Tit tit = await db.Tit.FindAsync(id);
            if (tit == null)
            {
                return NotFound();
            }

            db.Tit.Remove(tit);
            await db.SaveChangesAsync();

            return Ok(tit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TitExists(int id)
        {
            return db.Tit.Count(e => e.Id == id) > 0;
        }
    }
}