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
    public class HotelMastersController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/HotelMasters
        public IQueryable<HotelMaster> GetHotelMaster()
        {
            return db.HotelMaster;
        }

        // GET: api/HotelMasters/5
        [ResponseType(typeof(HotelMaster))]
        public async Task<IHttpActionResult> GetHotelMaster(int id)
        {
            HotelMaster hotelMaster = await db.HotelMaster.FindAsync(id);
            if (hotelMaster == null)
            {
                return NotFound();
            }

            return Ok(hotelMaster);
        }

        // PUT: api/HotelMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHotelMaster(int id, HotelMaster hotelMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hotelMaster.Id)
            {
                return BadRequest();
            }

            db.Entry(hotelMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelMasterExists(id))
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

        // POST: api/HotelMasters
        [ResponseType(typeof(HotelMaster))]
        public async Task<IHttpActionResult> PostHotelMaster(HotelMaster hotelMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HotelMaster.Add(hotelMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = hotelMaster.Id }, hotelMaster);
        }

        // DELETE: api/HotelMasters/5
        [ResponseType(typeof(HotelMaster))]
        public async Task<IHttpActionResult> DeleteHotelMaster(int id)
        {
            HotelMaster hotelMaster = await db.HotelMaster.FindAsync(id);
            if (hotelMaster == null)
            {
                return NotFound();
            }

            db.HotelMaster.Remove(hotelMaster);
            await db.SaveChangesAsync();

            return Ok(hotelMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HotelMasterExists(int id)
        {
            return db.HotelMaster.Count(e => e.Id == id) > 0;
        }
    }
}