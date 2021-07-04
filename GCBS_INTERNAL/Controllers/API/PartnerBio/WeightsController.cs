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
    public class WeightsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Weights
        public IQueryable<Weight> GetWeight()
        {
            return db.Weight;
        }

        // GET: api/Weights/5
        [ResponseType(typeof(Weight))]
        public async Task<IHttpActionResult> GetWeight(int id)
        {
            Weight weight = await db.Weight.FindAsync(id);
            if (weight == null)
            {
                return NotFound();
            }

            return Ok(weight);
        }

        // PUT: api/Weights/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWeight(int id, Weight weight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weight.Id)
            {
                return BadRequest();
            }
            using (var db2 = new DatabaseContext())
            {
                Weight weight1 = await db2.Weight.FindAsync(id);
                weight.CreatedBy = weight1.CreatedBy;
                weight.CreatedOn = weight1.CreatedOn;
                weight.Status = weight1.Status;
                db2.Dispose();
            }
            weight.UpdatedBy = userDetails.Id;
            weight.UpdatedOn = DateTime.Now;
            db.Entry(weight).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightExists(id))
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
        public async Task<IHttpActionResult> PutWeight(WeightVisible weightVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (weightVisible == null)
            {
                return BadRequest();
            }
            Weight weight = await db.Weight.FindAsync(weightVisible.Id);
            weight.Status = weightVisible.Status;
            weight.UpdatedBy = userDetails.Id;
            weight.UpdatedOn = DateTime.Now;
            db.Entry(weight).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Weights
        [ResponseType(typeof(Weight))]
        public async Task<IHttpActionResult> PostWeight(Weight weight)
        {
            if (weight != null)
            {
                weight.CreatedBy = userDetails.Id;
                weight.CreatedOn = DateTime.Now;
                weight.Status = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Weight.Add(weight);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = weight.Id }, weight);
        }

        // DELETE: api/Weights/5
        [ResponseType(typeof(Weight))]
        public async Task<IHttpActionResult> DeleteWeight(int id)
        {
            Weight weight = await db.Weight.FindAsync(id);
            if (weight == null)
            {
                return NotFound();
            }

            db.Weight.Remove(weight);
            await db.SaveChangesAsync();

            return Ok(weight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WeightExists(int id)
        {
            return db.Weight.Count(e => e.Id == id) > 0;
        }
    }
}