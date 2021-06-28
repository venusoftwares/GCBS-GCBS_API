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
    public class ServicesMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/ServicesMasters
        public IQueryable<ServicesMaster> GetServicesMasters()
        {
            return db.ServicesMasters;
        }

        // GET: api/ServicesMasters/5
        [ResponseType(typeof(ServicesMaster))]
        public async Task<IHttpActionResult> GetServicesMaster(int id)
        {
            ServicesMaster servicesMaster = await db.ServicesMasters.FindAsync(id);
            if (servicesMaster == null)
            {
                return NotFound();
            }

            return Ok(servicesMaster);
        }

        // PUT: api/ServicesMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutServicesMaster(int id, ServicesMaster servicesMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != servicesMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(servicesMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicesMasterExists(id))
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
        public async Task<IHttpActionResult> PutServicesMaster(ServiceMasterVisible serviceMasterVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (serviceMasterVisible == null)
            {
                return BadRequest();
            }
            ServicesMaster servicesMaster = await db.ServicesMasters.FindAsync(serviceMasterVisible.Id);
            servicesMaster.Visible = serviceMasterVisible.Visible;
            servicesMaster.UpdatedBy = userDetails.Id;
            servicesMaster.UpdatedOn = DateTime.Now;
            db.Entry(servicesMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/ServicesMasters
        [ResponseType(typeof(ServicesMaster))]
        public async Task<IHttpActionResult> PostServicesMaster(ServicesMaster servicesMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ServicesMasters.Add(servicesMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = servicesMaster.Id }, servicesMaster);
        }

        // DELETE: api/ServicesMasters/5
        [ResponseType(typeof(ServicesMaster))]
        public async Task<IHttpActionResult> DeleteServicesMaster(int id)
        {
            ServicesMaster servicesMaster = await db.ServicesMasters.FindAsync(id);
            if (servicesMaster == null)
            {
                return NotFound();
            }

            db.ServicesMasters.Remove(servicesMaster);
            await db.SaveChangesAsync();

            return Ok(servicesMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServicesMasterExists(int id)
        {
            return db.ServicesMasters.Count(e => e.Id == id) > 0;
        }
    }
}