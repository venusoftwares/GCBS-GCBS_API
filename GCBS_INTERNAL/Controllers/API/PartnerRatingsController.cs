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
    public class PartnerRatingsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PartnerRatings
        public IQueryable<PartnerRating> GetPartnerRating()
        {
            return db.PartnerRating;
        }

        // GET: api/PartnerRatings/5
        [ResponseType(typeof(PartnerRating))]
        public async Task<IHttpActionResult> GetPartnerRating(int id)
        {
            PartnerRating partnerRating = await db.PartnerRating.FindAsync(id);
            if (partnerRating == null)
            {
                return NotFound();
            }

            return Ok(partnerRating);
        }

        // PUT: api/PartnerRatings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPartnerRating(int id, PartnerRating partnerRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != partnerRating.Id)
            {
                return BadRequest();
            }

            db.Entry(partnerRating).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerRatingExists(id))
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

        // POST: api/PartnerRatings
        [ResponseType(typeof(PartnerRating))]
        public async Task<IHttpActionResult> PostPartnerRating(PartnerRating partnerRating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PartnerRating.Add(partnerRating);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = partnerRating.Id }, partnerRating);
        }

        // DELETE: api/PartnerRatings/5
        [ResponseType(typeof(PartnerRating))]
        public async Task<IHttpActionResult> DeletePartnerRating(int id)
        {
            PartnerRating partnerRating = await db.PartnerRating.FindAsync(id);
            if (partnerRating == null)
            {
                return NotFound();
            }

            db.PartnerRating.Remove(partnerRating);
            await db.SaveChangesAsync();

            return Ok(partnerRating);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PartnerRatingExists(int id)
        {
            return db.PartnerRating.Count(e => e.Id == id) > 0;
        }
    }
}