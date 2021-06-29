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
    public class EnquiryDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/EnquiryDetails
        public IQueryable<EnquiryViewModel> GetEnquiryDetails()
        {
            return db.EnquiryDetails
                .Include(x=>x.UserManagements)
                .Include(x=>x.PartnerManagements)
                .Include(x=>x.servicesMasters)
                .Select(x=> new EnquiryViewModel 
                { 
                    Id = x.Id,  
                    Email = x.UserManagements.EmailId,
                    Service = x.servicesMasters.Service,
                    ServicePartner = x.PartnerManagements.Username,
                    ServicePartnerId = x.PartnerId,
                    Username = x.UserManagements.Username,
                    UserId = x.UserId,
                    ServiceStatus = x.EnquiryStatus,
                    UserStatus = x.UserStatus    
                });
        }

        // GET: api/EnquiryDetails/5
        [ResponseType(typeof(EnquiryDetails))]
        public async Task<IHttpActionResult> GetEnquiryDetails(int id)
        {
            EnquiryDetails enquiryDetails = await db.EnquiryDetails.FindAsync(id);
            if (enquiryDetails == null)
            {
                return NotFound();
            }

            return Ok(enquiryDetails);
        }

        // PUT: api/EnquiryDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEnquiryDetails(int id, EnquiryDetails enquiryDetails)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != enquiryDetails.Id)
            {
                return BadRequest();
            }
            using(var db2 = new DatabaseContext())
            {
                EnquiryDetails enquiry = await db2.EnquiryDetails.FindAsync(id);
                enquiryDetails.CreatedBy = enquiry.CreatedBy;
                enquiryDetails.CreatedOn = enquiry.CreatedOn;
                db2.Dispose();
            }
            enquiryDetails.UpdatedBy = userDetails.Id;
            enquiryDetails.UpdatedOn = DateTime.Now;
            db.Entry(enquiryDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnquiryDetailsExists(id))
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

        // POST: api/EnquiryDetails
        [ResponseType(typeof(EnquiryDetails))]
        public async Task<IHttpActionResult> PostEnquiryDetails(EnquiryDetails enquiryDetails)
        {
            if (enquiryDetails != null)
            {
                enquiryDetails.CreatedBy = userDetails.Id;
                enquiryDetails.CreatedOn = DateTime.Now;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EnquiryDetails.Add(enquiryDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = enquiryDetails.Id }, enquiryDetails);
        }

        // DELETE: api/EnquiryDetails/5
        [ResponseType(typeof(EnquiryDetails))]
        public async Task<IHttpActionResult> DeleteEnquiryDetails(int id)
        {
            EnquiryDetails enquiryDetails = await db.EnquiryDetails.FindAsync(id);
            if (enquiryDetails == null)
            {
                return NotFound();
            }

            db.EnquiryDetails.Remove(enquiryDetails);
            await db.SaveChangesAsync();

            return Ok(enquiryDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EnquiryDetailsExists(int id)
        {
            return db.EnquiryDetails.Count(e => e.Id == id) > 0;
        }
    }
}