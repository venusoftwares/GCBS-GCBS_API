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
    public class SmsSettingsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/SmsSettings
        public IQueryable<SmsSettings> GetSmsSettings()
        {
            return db.SmsSettings;
        }

        // GET: api/SmsSettings/5
        [ResponseType(typeof(SmsSettings))]
        public async Task<IHttpActionResult> GetSmsSettings(int id)
        {
            SmsSettings smsSettings = await db.SmsSettings.FindAsync(id);
            if (smsSettings == null)
            {
                return NotFound();
            }

            return Ok(smsSettings);
        }

        // PUT: api/SmsSettings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSmsSettings(int id, SmsSettings smsSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != smsSettings.Id)
            {
                return BadRequest();
            }

            db.Entry(smsSettings).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmsSettingsExists(id))
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

        // POST: api/SmsSettings
        [ResponseType(typeof(SmsSettings))]
        public async Task<IHttpActionResult> PostSmsSettings(SmsSettings smsSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SmsSettings.Add(smsSettings);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = smsSettings.Id }, smsSettings);
        }

        // DELETE: api/SmsSettings/5
        [ResponseType(typeof(SmsSettings))]
        public async Task<IHttpActionResult> DeleteSmsSettings(int id)
        {
            SmsSettings smsSettings = await db.SmsSettings.FindAsync(id);
            if (smsSettings == null)
            {
                return NotFound();
            }

            db.SmsSettings.Remove(smsSettings);
            await db.SaveChangesAsync();

            return Ok(smsSettings);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SmsSettingsExists(int id)
        {
            return db.SmsSettings.Count(e => e.Id == id) > 0;
        }
    }
}