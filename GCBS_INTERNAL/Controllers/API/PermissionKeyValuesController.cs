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
    public class PermissionKeyValuesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PermissionKeyValues
        public IQueryable<PermissionKeyValue> GetPermissionKeyValue()
        {
            return db.PermissionKeyValue;
        }

        // GET: api/PermissionKeyValues/5
        [ResponseType(typeof(PermissionKeyValue))]
        public async Task<IHttpActionResult> GetPermissionKeyValue(int id)
        {
            PermissionKeyValue permissionKeyValue = await db.PermissionKeyValue.FindAsync(id);
            if (permissionKeyValue == null)
            {
                return NotFound();
            }

            return Ok(permissionKeyValue);
        }

        // PUT: api/PermissionKeyValues/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPermissionKeyValue(int id, PermissionKeyValue permissionKeyValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != permissionKeyValue.Id)
            {
                return BadRequest();
            }

            db.Entry(permissionKeyValue).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissionKeyValueExists(id))
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
        public async Task<IHttpActionResult> PutPermissionKeyValue(PermissionKeyValueVisible permissionKeyValueVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (permissionKeyValueVisible == null)
            {
                return BadRequest();
            }
            PermissionKeyValue permissionKeyValue = await db.PermissionKeyValue.FindAsync(permissionKeyValueVisible.Id);
            permissionKeyValue.Status = permissionKeyValueVisible.Status;
            db.Entry(permissionKeyValue).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        

 // POST: api/PermissionKeyValues
 [ResponseType(typeof(PermissionKeyValue))]
        public async Task<IHttpActionResult> PostPermissionKeyValue(PermissionKeyValue permissionKeyValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PermissionKeyValue.Add(permissionKeyValue);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PermissionKeyValueExists(permissionKeyValue.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = permissionKeyValue.Id }, permissionKeyValue);
        }

        // DELETE: api/PermissionKeyValues/5
        [ResponseType(typeof(PermissionKeyValue))]
        public async Task<IHttpActionResult> DeletePermissionKeyValue(int id)
        {
            PermissionKeyValue permissionKeyValue = await db.PermissionKeyValue.FindAsync(id);
            if (permissionKeyValue == null)
            {
                return NotFound();
            }

            db.PermissionKeyValue.Remove(permissionKeyValue);
            await db.SaveChangesAsync();

            return Ok(permissionKeyValue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PermissionKeyValueExists(int id)
        {
            return db.PermissionKeyValue.Count(e => e.Id == id) > 0;
        }
    }
}