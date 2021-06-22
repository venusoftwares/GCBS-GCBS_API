﻿using System;
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
    public class EmailSettingsController : ApiController
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

        // DELETE: api/EmailSettings/5
        [ResponseType(typeof(EmailSettings))]
        public async Task<IHttpActionResult> DeleteEmailSettings(int id)
        {
            EmailSettings emailSettings = await db.EmailSettings.FindAsync(id);
            if (emailSettings == null)
            {
                return NotFound();
            }

            db.EmailSettings.Remove(emailSettings);
            await db.SaveChangesAsync();

            return Ok(emailSettings);
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