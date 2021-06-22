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
    public class UserManagementsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/UserManagements
        public IQueryable<UserManagement> GetUserManagement()
        {
            return db.UserManagement;
        }

        // GET: api/UserManagements/5
        [ResponseType(typeof(UserManagement))]
        public async Task<IHttpActionResult> GetUserManagement(int id)
        {
            UserManagement userManagement = await db.UserManagement.FindAsync(id);
            if (userManagement == null)
            {
                return NotFound();
            }

            return Ok(userManagement);
        }

        // PUT: api/UserManagements/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserManagement(int id, UserManagement userManagement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userManagement.Id)
            {
                return BadRequest();
            }

            db.Entry(userManagement).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserManagementExists(id))
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

        // POST: api/UserManagements
        [ResponseType(typeof(UserManagement))]
        public async Task<IHttpActionResult> PostUserManagement(UserManagement userManagement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserManagement.Add(userManagement);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = userManagement.Id }, userManagement);
        }

        // DELETE: api/UserManagements/5
        [ResponseType(typeof(UserManagement))]
        public async Task<IHttpActionResult> DeleteUserManagement(int id)
        {
            UserManagement userManagement = await db.UserManagement.FindAsync(id);
            if (userManagement == null)
            {
                return NotFound();
            }

            db.UserManagement.Remove(userManagement);
            await db.SaveChangesAsync();

            return Ok(userManagement);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserManagementExists(int id)
        {
            return db.UserManagement.Count(e => e.Id == id) > 0;
        }
    }
}