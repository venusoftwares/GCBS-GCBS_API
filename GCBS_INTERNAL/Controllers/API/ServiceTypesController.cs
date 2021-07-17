using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.ViewModels;
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
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Models
{
    [CustomAuthorize]
    public class ServiceTypesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/ServiceTypes
        public IQueryable<ServiceTypes> GetServiceTypes()
        {
            return db.ServiceTypes;
        }

        
        // GET: api/ServiceTypes/5
        [ResponseType(typeof(ServiceTypes))]
        public async Task<IHttpActionResult> GetServiceTypes(int id)
        {
            ServiceTypes serviceTypes = await db.ServiceTypes.FindAsync(id);
            if (serviceTypes == null)
            {
                return NotFound();
            }

            return Ok(serviceTypes);
        }

        // PUT: api/ServiceTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutServiceTypes(int id, ServiceTypes serviceTypes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != serviceTypes.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.ServiceTypes.FindAsync(id);
                serviceTypes.CreatedBy = re.CreatedBy;
                serviceTypes.CreatedOn = re.CreatedOn;
                serviceTypes.Visible = re.Visible;
                d.Dispose();
            }
            serviceTypes.UpdatedBy = userDetails.Id;
            serviceTypes.UpdatedOn = DateTime.Now;
            db.Entry(serviceTypes).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceTypesExists(id))
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
        public async Task<IHttpActionResult> PutServiceTypes(ServiceTypesVisible serviceTypesVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (serviceTypesVisible == null)
            {
                return BadRequest();
            }
            ServiceTypes serviceTypes = await db.ServiceTypes.FindAsync(serviceTypesVisible.Id);
            serviceTypes.Visible = serviceTypesVisible.Visible;
            db.Entry(serviceTypes).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ServiceTypes
        [ResponseType(typeof(ServiceTypes))]
        public async Task<IHttpActionResult> PostServiceTypes(ServiceTypes serviceTypes)
        {
            if (serviceTypes != null)
            {
                serviceTypes.CreatedBy = userDetails.Id;
                serviceTypes.CreatedOn = DateTime.Now;
                serviceTypes.Visible = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ServiceTypes.Add(serviceTypes);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = serviceTypes.Id }, serviceTypes);
        }

        // DELETE: api/ServiceTypes/5
        [ResponseType(typeof(ServiceTypes))]
        public async Task<IHttpActionResult> DeleteServiceTypes(int id)
        {
            ServiceTypes serviceTypes = await db.ServiceTypes.FindAsync(id);
            if (serviceTypes == null)
            {
                return NotFound();
            }

            db.ServiceTypes.Remove(serviceTypes);
            await db.SaveChangesAsync();

            return Ok(serviceTypes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiceTypesExists(int id)
        {
            return db.ServiceTypes.Count(e => e.Id == id) > 0;
        }
    }
}