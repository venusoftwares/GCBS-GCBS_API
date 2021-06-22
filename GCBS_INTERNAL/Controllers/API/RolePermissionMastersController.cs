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
    public class RolePermissionMastersController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/RolePermissionMasters
        public IQueryable<RolePermissionMaster> GetRolePermissionMaster()
        {
            return db.RolePermissionMaster;
        }

        // GET: api/RolePermissionMasters/5
        [ResponseType(typeof(RolePermissionMaster))]
        public async Task<IHttpActionResult> GetRolePermissionMaster(int id)
        {
            RolePermissionMaster rolePermissionMaster = await db.RolePermissionMaster.FindAsync(id);
            if (rolePermissionMaster == null)
            {
                return NotFound();
            }

            return Ok(rolePermissionMaster);
        }

        // PUT: api/RolePermissionMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRolePermissionMaster(int id, RolePermissionMaster rolePermissionMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rolePermissionMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(rolePermissionMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolePermissionMasterExists(id))
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

        // POST: api/RolePermissionMasters
        [ResponseType(typeof(RolePermissionMaster))]
        public async Task<IHttpActionResult> PostRolePermissionMaster(RolePermissionMaster rolePermissionMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RolePermissionMaster.Add(rolePermissionMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = rolePermissionMaster.Id }, rolePermissionMaster);
        }

        // DELETE: api/RolePermissionMasters/5
        [ResponseType(typeof(RolePermissionMaster))]
        public async Task<IHttpActionResult> DeleteRolePermissionMaster(int id)
        {
            RolePermissionMaster rolePermissionMaster = await db.RolePermissionMaster.FindAsync(id);
            if (rolePermissionMaster == null)
            {
                return NotFound();
            }

            db.RolePermissionMaster.Remove(rolePermissionMaster);
            await db.SaveChangesAsync();

            return Ok(rolePermissionMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RolePermissionMasterExists(int id)
        {
            return db.RolePermissionMaster.Count(e => e.Id == id) > 0;
        }
    }
}