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
    public class InAppNotificationsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: api/InAppNotifications
        public IQueryable<InAppNotification> GetInAppNotification()
        {
            return db.InAppNotification;
        }

        // GET: api/InAppNotifications/5
        [ResponseType(typeof(InAppNotification))]
        public async Task<IHttpActionResult> GetInAppNotification(int id)
        {
            InAppNotification inAppNotification = await db.InAppNotification.FindAsync(id);
            if (inAppNotification == null)
            {
                return NotFound();
            }

            return Ok(inAppNotification);
        }

        // PUT: api/InAppNotifications/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInAppNotification(int id, InAppNotification inAppNotification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inAppNotification.Id)
            {
                return BadRequest();
            }

            db.Entry(inAppNotification).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                log.Error(ex.Message);
                if (!InAppNotificationExists(id))
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

        // POST: api/InAppNotifications
        [ResponseType(typeof(InAppNotification))]
        public async Task<IHttpActionResult> PostInAppNotification(InAppNotification inAppNotification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.InAppNotification.Add(inAppNotification);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = inAppNotification.Id }, inAppNotification);
        }

        // DELETE: api/InAppNotifications/5
        [ResponseType(typeof(InAppNotification))]
        public async Task<IHttpActionResult> DeleteInAppNotification(int id)
        {
            InAppNotification inAppNotification = await db.InAppNotification.FindAsync(id);
            if (inAppNotification == null)
            {
                return NotFound();
            }

            db.InAppNotification.Remove(inAppNotification);
            await db.SaveChangesAsync();

            return Ok(inAppNotification);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InAppNotificationExists(int id)
        {
            return db.InAppNotification.Count(e => e.Id == id) > 0;
        }
    }
}