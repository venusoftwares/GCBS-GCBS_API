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

namespace GCBS_INTERNAL.Controllers.API.PartnerBio
{
     [CustomAuthorize]
    public class MeetingsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/Meetings
        public IQueryable<Meeting> GetMeeting()
        {
            return db.Meeting;
        }

        // GET: api/Meetings/5
        [ResponseType(typeof(Meeting))]
        public async Task<IHttpActionResult> GetMeeting(int id)
        {
            Meeting meeting = await db.Meeting.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(meeting);
        }

        // PUT: api/Meetings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMeeting(int id, Meeting meeting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != meeting.Id)
            {
                return BadRequest();
            }
            using (var db2 = new DatabaseContext())
            {
                Meeting meeting1 = await db2.Meeting.FindAsync(id);
                meeting.CreatedBy = meeting1.CreatedBy;
                meeting.CreatedOn = meeting1.CreatedOn;
                meeting.Status = meeting1.Status;
                db2.Dispose();
            }
            meeting.UpdatedBy = userDetails.Id;
            meeting.UpdatedOn = DateTime.Now;
            db.Entry(meeting).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingExists(id))
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
        // PUT: api/Heights/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMeeting(MeetingsVisible meetingsVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (meetingsVisible == null)
            {
                return BadRequest();
            }
            Meeting meeting = await db.Meeting.FindAsync(meetingsVisible.Id);
            meeting.Status = meetingsVisible.Status;
            meeting.UpdatedBy = userDetails.Id;
            meeting.UpdatedOn = DateTime.Now;
            db.Entry(meeting).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/Meetings
        [ResponseType(typeof(Meeting))]
        public async Task<IHttpActionResult> PostMeeting(Meeting meeting)
        {
            if (meeting != null)
            {
                meeting.CreatedBy = userDetails.Id;
                meeting.CreatedOn = DateTime.Now;
                meeting.Status = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Meeting.Add(meeting);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = meeting.Id }, meeting);
        }

        // DELETE: api/Meetings/5
        [ResponseType(typeof(Meeting))]
        public async Task<IHttpActionResult> DeleteMeeting(int id)
        {
            Meeting meeting = await db.Meeting.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            db.Meeting.Remove(meeting);
            await db.SaveChangesAsync();

            return Ok(meeting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MeetingExists(int id)
        {
            return db.Meeting.Count(e => e.Id == id) > 0;
        }
    }
}