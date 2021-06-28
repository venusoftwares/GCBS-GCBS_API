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
    public class StateMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/StateMasters
        [Route("api/StateMasters/{CountryId}/{Status}")]
        public IQueryable<StateMasterViewModel> GetStateMaster(int CountryId,int Status)
        {
            IQueryable<StateMasterViewModel> filter = db.StateMaster
                .Include(x=>x.CountryMaster)
                .Select(x=>new StateMasterViewModel 
                {
                    Id = x.Id,
                    State = x.StateName,
                    Country = x.CountryMaster.CountryName,
                    Status = x.Status,
                    CountryId = x.CountryId  
                });         
            if (CountryId > 0)
            {
                filter = filter.Where(x => x.CountryId == CountryId);
            }
            if(Status == 0 || Status == 1 )
            {
                var status = Status == 1 ? true : false;
                filter = filter.Where(x => x.Status == status);
            }            
            return filter;
        }

        // GET: api/StateMasters/5
        [ResponseType(typeof(StateMaster))]
        public async Task<IHttpActionResult> GetStateMaster(int id)
        {
            StateMaster stateMaster = await db.StateMaster.FindAsync(id);
            if (stateMaster == null)
            {
                return NotFound();
            }

            return Ok(stateMaster);
        }

        // PUT: api/StateMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStateMaster(int id, StateMaster stateMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stateMaster.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.StateMaster.FindAsync(id);
                stateMaster.CreatedBy = re.CreatedBy;
                stateMaster.CreatedOn = re.CreatedOn;
                d.Dispose();
            }
            stateMaster.UpdatedBy = userDetails.Id;
            stateMaster.UpdatedOn = DateTime.Now;
            db.Entry(stateMaster).State = EntityState.Modified;
       
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateMasterExists(id))
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
        public async Task<IHttpActionResult> PutStateMaster(StateMasterVisible stateMasterVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (stateMasterVisible == null)
            {
                return BadRequest();
            }
            StateMaster stateMaster = await db.StateMaster.FindAsync(stateMasterVisible.Id);
            stateMaster.Status = stateMasterVisible.Status;
            stateMaster.UpdatedOn = DateTime.Now;
            stateMaster.UpdatedBy = userDetails.Id;
            db.Entry(stateMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        
        // POST: api/StateMasters
       [ResponseType(typeof(StateMaster))]
        public async Task<IHttpActionResult> PostStateMaster(StateMaster stateMaster)
        {
            stateMaster.CreatedBy = userDetails.Id;
            stateMaster.CreatedOn = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StateMaster.Add(stateMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = stateMaster.Id }, stateMaster);
        }

        // DELETE: api/StateMasters/5
        [ResponseType(typeof(StateMaster))]
        public async Task<IHttpActionResult> DeleteStateMaster(int id)
        {
            StateMaster stateMaster = await db.StateMaster.FindAsync(id);
            if (stateMaster == null)
            {
                return NotFound();
            }

            db.StateMaster.Remove(stateMaster);
            await db.SaveChangesAsync();

            return Ok(stateMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StateMasterExists(int id)
        {
            return db.StateMaster.Count(e => e.Id == id) > 0;
        }
    }
}