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
using Newtonsoft.Json;

namespace GCBS_INTERNAL.Controllers.API
{
    [Authorize]
    public class AvailabilitiesController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();     

        // GET: api/Availabilities/5
        [ResponseType(typeof(RootAvailability))]
        public async Task<IHttpActionResult> GetAvailability()
        {
            RootAvailability availability2 = new RootAvailability();
            Availability availability = await db.Availability.Where(x=>x.UserId == userDetails.Id).FirstOrDefaultAsync();
            if (availability == null)
            {
                DateTime now = DateTime.Now;
                return Ok(availability2);
            }
            availability2.Availability = availability;
            if(!String.IsNullOrEmpty(availability.Time))
            {
                availability2.Times = JsonConvert.DeserializeObject<List<Root>>(availability.Time);
            }
            else
            {
                availability2.Times = new List<Root>();
            }     
            return Ok(availability2);
        }

        // PUT: api/Availabilities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAvailability(int id, RootAvailability rootAvailability)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rootAvailability.Availability.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.Availability.FindAsync(id);
                rootAvailability.Availability.CreatedBy = re.CreatedBy;
                rootAvailability.Availability.CreatedOn = re.CreatedOn;
                rootAvailability.Availability.UserId = userDetails.Id;
                rootAvailability.Availability.Time = JsonConvert.SerializeObject(rootAvailability.Times);
                d.Dispose();
            }
            rootAvailability.Availability.UpdatedBy = userDetails.Id;
            rootAvailability.Availability.UpdatedOn = DateTime.Now;
            db.Entry(rootAvailability.Availability).State = EntityState.Modified;   
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvailabilityExists(id))
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

        // POST: api/Availabilities
        [ResponseType(typeof(Availability))]
        public async Task<IHttpActionResult> PostAvailability(RootAvailability rootAvailability)
        {
            rootAvailability.Availability.CreatedBy = userDetails.Id;
            rootAvailability.Availability.CreatedOn = DateTime.Now;
            rootAvailability.Availability.Time = JsonConvert.SerializeObject(rootAvailability.Times);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            rootAvailability.Availability.UserId = userDetails.Id;
            db.Availability.Add(rootAvailability.Availability);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = rootAvailability.Availability.Id }, rootAvailability.Availability);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AvailabilityExists(int id)
        {
            return db.Availability.Count(e => e.Id == id) > 0;
        }
    }
}