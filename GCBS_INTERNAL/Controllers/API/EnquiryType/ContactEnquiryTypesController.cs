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

namespace GCBS_INTERNAL.Controllers.API.EnquiryType
{
    [CustomAuthorize]
    public class ContactEnquiryTypesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/ContactEnquiryTypes
        public IQueryable<ContactEnquiryType> GetContactEnquiryTypes()
        {
            return db.ContactEnquiryTypes;
        }

        // GET: api/ContactEnquiryTypes/5
        [ResponseType(typeof(ContactEnquiryType))]
        public async Task<IHttpActionResult> GetContactEnquiryType(int id)
        {
            ContactEnquiryType contactEnquiryType = await db.ContactEnquiryTypes.FindAsync(id);
            if (contactEnquiryType == null)
            {
                return NotFound();
            }

            return Ok(contactEnquiryType);
        }

        // PUT: api/ContactEnquiryTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContactEnquiryType(int id, ContactEnquiryType contactEnquiryType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactEnquiryType.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.ContactEnquiryTypes.FindAsync(id);
                contactEnquiryType.CreatedBy = re.CreatedBy;
                contactEnquiryType.CreatedOn = re.CreatedOn;
                contactEnquiryType.Status = re.Status;
                d.Dispose();
            }
            contactEnquiryType.UpdatedBy = userDetails.Id;
            contactEnquiryType.UpdatedOn = DateTime.Now;
            db.Entry(contactEnquiryType).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactEnquiryTypeExists(id))
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
        public async Task<IHttpActionResult> PutContactEnquiryType(ContactEnquiryTypeVisible contactEnquiryTypeVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (contactEnquiryTypeVisible == null)
            {
                return BadRequest();
            }
            ContactEnquiryType contactEnquiryType = await db.ContactEnquiryTypes.FindAsync(contactEnquiryTypeVisible.Id);
            contactEnquiryType.Status = contactEnquiryTypeVisible.Status;
            contactEnquiryType.UpdatedBy = userDetails.Id;
            contactEnquiryType.UpdatedOn = DateTime.Now;
            db.Entry(contactEnquiryType).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/ContactEnquiryTypes
        [ResponseType(typeof(ContactEnquiryType))]
        public async Task<IHttpActionResult> PostContactEnquiryType(ContactEnquiryType contactEnquiryType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            contactEnquiryType.CreatedBy = userDetails.Id;
            contactEnquiryType.CreatedOn = DateTime.Now;
            db.ContactEnquiryTypes.Add(contactEnquiryType);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = contactEnquiryType.Id }, contactEnquiryType);
        }

        // DELETE: api/ContactEnquiryTypes/5
        [ResponseType(typeof(ContactEnquiryType))]
        public async Task<IHttpActionResult> DeleteContactEnquiryType(int id)
        {
            ContactEnquiryType contactEnquiryType = await db.ContactEnquiryTypes.FindAsync(id);
            if (contactEnquiryType == null)
            {
                return NotFound();
            }

            db.ContactEnquiryTypes.Remove(contactEnquiryType);
            await db.SaveChangesAsync();

            return Ok(contactEnquiryType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactEnquiryTypeExists(int id)
        {
            return db.ContactEnquiryTypes.Count(e => e.Id == id) > 0;
        }
    }
}