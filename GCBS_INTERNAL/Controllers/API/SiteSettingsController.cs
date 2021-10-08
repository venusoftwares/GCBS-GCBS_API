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
using log4net;

namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class SiteSettingsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: api/SiteSettings
        public IQueryable<SiteSettings> GetSiteSettings()
        {
            return db.SiteSettings;
        }

        // GET: api/SiteSettings/5
        [ResponseType(typeof(SiteSettings))]
        public async Task<IHttpActionResult> GetSiteSettings(int id)
        {
            SiteSettings siteSettings = await db.SiteSettings.FindAsync(id);
            if (siteSettings == null)
            {
                return NotFound();
            }

            return Ok(siteSettings);
        }

        // PUT: api/SiteSettings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSiteSettings(int id, SiteSettings siteSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != siteSettings.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.SiteSettings.FindAsync(id);
                siteSettings.CreatedBy = re.CreatedBy;
                siteSettings.CreatedOn = re.CreatedOn;   
                d.Dispose();
            }
            siteSettings.UpdatedBy = userDetails.Id;
            siteSettings.UpdatedOn = DateTime.Now;
            db.Entry(siteSettings).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex.Message);
                if (!SiteSettingsExists(id))
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

        // POST: api/SiteSettings
        [ResponseType(typeof(SiteSettings))]
        public async Task<IHttpActionResult> PostSiteSettings(SiteSettings siteSettings)
        {
            if (siteSettings != null)
            {
                siteSettings.CreatedBy = userDetails.Id;
                siteSettings.CreatedOn = DateTime.Now;      
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SiteSettings.Add(siteSettings);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = siteSettings.Id }, siteSettings);
        }

        // DELETE: api/SiteSettings/5
        [ResponseType(typeof(SiteSettings))]
        public async Task<IHttpActionResult> DeleteSiteSettings(int id)
        {
            SiteSettings siteSettings = await db.SiteSettings.FindAsync(id);
            if (siteSettings == null)
            {
                return NotFound();
            }

            db.SiteSettings.Remove(siteSettings);
            await db.SaveChangesAsync();

            return Ok(siteSettings);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SiteSettingsExists(int id)
        {
            return db.SiteSettings.Count(e => e.Id == id) > 0;
        }
    }
}