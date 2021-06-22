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
    public class ContactEnquiryViewsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/ContactEnquiryViews
        public IQueryable<ContactEnquiryView> GetContactEnquiryView()
        {
            return db.ContactEnquiryView;
        }

        // GET: api/ContactEnquiryViews/5
        [ResponseType(typeof(ContactEnquiryView))]
        public async Task<IHttpActionResult> GetContactEnquiryView(int id)
        {
            ContactEnquiryView contactEnquiryView = await db.ContactEnquiryView.FindAsync(id);
            if (contactEnquiryView == null)
            {
                return NotFound();
            }

            return Ok(contactEnquiryView);
        }

        // PUT: api/ContactEnquiryViews/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContactEnquiryView(int id, ContactEnquiryView contactEnquiryView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactEnquiryView.Id)
            {
                return BadRequest();
            }

            db.Entry(contactEnquiryView).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactEnquiryViewExists(id))
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

        // POST: api/ContactEnquiryViews
        [ResponseType(typeof(ContactEnquiryView))]
        public async Task<IHttpActionResult> PostContactEnquiryView(ContactEnquiryView contactEnquiryView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ContactEnquiryView.Add(contactEnquiryView);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = contactEnquiryView.Id }, contactEnquiryView);
        }

        // DELETE: api/ContactEnquiryViews/5
        [ResponseType(typeof(ContactEnquiryView))]
        public async Task<IHttpActionResult> DeleteContactEnquiryView(int id)
        {
            ContactEnquiryView contactEnquiryView = await db.ContactEnquiryView.FindAsync(id);
            if (contactEnquiryView == null)
            {
                return NotFound();
            }

            db.ContactEnquiryView.Remove(contactEnquiryView);
            await db.SaveChangesAsync();

            return Ok(contactEnquiryView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactEnquiryViewExists(int id)
        {
            return db.ContactEnquiryView.Count(e => e.Id == id) > 0;
        }
    }
}