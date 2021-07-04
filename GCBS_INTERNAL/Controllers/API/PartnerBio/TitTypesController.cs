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
    public class TitTypesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/TitTypes
        public IQueryable<TitType> GetTitType()
        {
            return db.TitType;
        }

        // GET: api/TitTypes/5
        [ResponseType(typeof(TitType))]
        public async Task<IHttpActionResult> GetTitType(int id)
        {
            TitType titType = await db.TitType.FindAsync(id);
            if (titType == null)
            {
                return NotFound();
            }

            return Ok(titType);
        }

        // PUT: api/TitTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTitType(int id, TitType titType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != titType.Id)
            {
                return BadRequest();
            }
            using (var db2 = new DatabaseContext())
            {
                TitType titType1 = await db2.TitType.FindAsync(id);
                titType.CreatedBy = titType1.CreatedBy;
                titType.CreatedOn = titType1.CreatedOn;
                titType.Status = titType1.Status;
                db2.Dispose();
            }
            titType.UpdatedBy = userDetails.Id;
            titType.UpdatedOn = DateTime.Now;
            db.Entry(titType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TitTypeExists(id))
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
        public async Task<IHttpActionResult> PutTitType(TitTypeVisible titTypeVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (titTypeVisible == null)
            {
                return BadRequest();
            }
            TitType titType = await db.TitType.FindAsync(titTypeVisible.Id);
            titType.Status = titTypeVisible.Status;
            titType.UpdatedBy = userDetails.Id;
            titType.UpdatedOn = DateTime.Now;
            db.Entry(titType).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/TitTypes
        [ResponseType(typeof(TitType))]
        public async Task<IHttpActionResult> PostTitType(TitType titType)
        {
            if (titType != null)
            {
                titType.CreatedBy = userDetails.Id;
                titType.CreatedOn = DateTime.Now;
                titType.Status = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TitType.Add(titType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = titType.Id }, titType);
        }

        // DELETE: api/TitTypes/5
        [ResponseType(typeof(TitType))]
        public async Task<IHttpActionResult> DeleteTitType(int id)
        {
            TitType titType = await db.TitType.FindAsync(id);
            if (titType == null)
            {
                return NotFound();
            }

            db.TitType.Remove(titType);
            await db.SaveChangesAsync();

            return Ok(titType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TitTypeExists(int id)
        {
            return db.TitType.Count(e => e.Id == id) > 0;
        }
    }
}