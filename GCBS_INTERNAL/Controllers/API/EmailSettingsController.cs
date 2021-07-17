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
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class EmailSettingsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/EmailSettings
        public IQueryable<EmailSettings> GetEmailSettings()
        {
            return db.EmailSettings;
        }

        // GET: api/EmailSettings/5
        [ResponseType(typeof(EmailSettings))]
        public async Task<IHttpActionResult> GetEmailSettings(int id)
        {
            EmailSettings emailSettings = await db.EmailSettings.FindAsync(id);
            if (emailSettings == null)
            {
                return NotFound();
            }

            return Ok(emailSettings);
        }

        // PUT: api/EmailSettings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmailSettings(int id, EmailSettings emailSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emailSettings.Id)
            {
                return BadRequest();
            }

            db.Entry(emailSettings).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailSettingsExists(id))
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

        // POST: api/EmailSettings
        [ResponseType(typeof(EmailSettings))]
        public async Task<IHttpActionResult> PostEmailSettings(EmailSettings emailSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmailSettings.Add(emailSettings);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = emailSettings.Id }, emailSettings);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmailSettingsExists(int id)
        {
            return db.EmailSettings.Count(e => e.Id == id) > 0;
        }
    }
}