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
using GCBS_INTERNAL.Services;
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class HotelMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public ImageServices imgser = new ImageServices();

        // GET: api/HotelMasters
        public List<HotelMasterView> GetHotelMaster()
        {
            List<HotelMasterView> list = new List<HotelMasterView>();
            var res = db.HotelMaster.Include(x => x.LocationMasters).ToList();
            foreach (var a in res)
            {
                // First or default 
                string path = imgser.GetFiles(a.Id, Constant.HOTEL_FOLDER_TYPE).FirstOrDefault();
                list.Add(new HotelMasterView
                {
                    Id = a.Id,
                    Status = a.Status,
                    Email = a.Email,
                    HotelName = a.HotelName,
                    Location = a.Location,
                    WebsiteUrl = a.WebsiteUrl,
                    ValidEndDate = a.ValidEndDate.ToString("dd-MM-yyyy hh:mm tt"),
                    ValidStartDate = a.ValidStartDate.ToString("dd-MM-yyyy hh:mm tt"),
                    Image = path ,
                    LocationMasters = a.LocationMasters
                });
            }
            return list;
        }

        // GET: api/HotelMasters/5
        [ResponseType(typeof(HotelMasterViewModel))]
        public async Task<IHttpActionResult> GetHotelMaster(int id)
        {
            HotelMaster hotelMaster = await db.HotelMaster.FindAsync(id);
            if (hotelMaster == null)
            {
                return NotFound();
            }
            HotelMasterViewModel hotelMasterViewModel = new HotelMasterViewModel();
            hotelMasterViewModel.HotelMaster = hotelMaster;
            hotelMasterViewModel.imageBase64 = imgser.EditGetFiles(id, Constant.HOTEL_FOLDER_TYPE);
            hotelMasterViewModel.LocationMasters = await db.LocationMasters.FindAsync(hotelMaster.Location);
            return Ok(hotelMasterViewModel);
        }

        // PUT: api/HotelMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHotelMaster(int id, HotelMasterViewModel hotelMasterViewModel)
        {
            HotelMaster hotelMaster = hotelMasterViewModel.HotelMaster;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hotelMaster.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.HotelMaster.FindAsync(id);
                hotelMaster.CreatedBy = re.CreatedBy;
                hotelMaster.CreatedOn = re.CreatedOn;
                hotelMaster.Status = re.Status;
                d.Dispose();
            }
            hotelMaster.UpdatedBy = userDetails.Id;
            hotelMaster.UpdatedOn = DateTime.Now;
            db.Entry(hotelMaster).State = EntityState.Modified;   
            try
            {
                await db.SaveChangesAsync();
                if (hotelMaster.Id > 0 && hotelMasterViewModel.imageBase64.Count() > 0)
                {
                    imgser.DeleteFiles(hotelMaster.Id, Constant.HOTEL_FOLDER_TYPE);
                    foreach (var imgbase64 in hotelMasterViewModel.imageBase64)
                    {
                        imgser.SaveImage(imgbase64, Constant.HOTEL_FOLDER_TYPE, hotelMaster.Id, userDetails.Id);
                    }
                }
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
            
        // PUT: api/PriceMasters
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHotelMaster(HotelMasterVisible hotelMasterVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (hotelMasterVisible == null)
            {
                return BadRequest();
            }
            HotelMaster hotelMaster = await db.HotelMaster.FindAsync(hotelMasterVisible.Id);
            hotelMaster.Status = hotelMasterVisible.Status;
            hotelMaster.UpdatedBy = userDetails.Id;
            hotelMaster.UpdatedOn = DateTime.Now;
            db.Entry(hotelMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/HotelMasters
        [ResponseType(typeof(HotelMasterViewModel))]
        public async Task<IHttpActionResult> PostHotelMaster(HotelMasterViewModel hotelMasterViewModel)
        {
            HotelMaster hotelMaster = hotelMasterViewModel.HotelMaster;
            if(hotelMaster!=null)
            {
                hotelMaster.CreatedBy = userDetails.Id;
                hotelMaster.CreatedOn = DateTime.Now;
                hotelMaster.Status = true;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HotelMaster.Add(hotelMaster);
            await db.SaveChangesAsync();
            if (hotelMaster.Id > 0 && hotelMasterViewModel.imageBase64.Count() > 0)
            {
                foreach (var imgbase64 in hotelMasterViewModel.imageBase64)
                {
                    imgser.SaveImage(imgbase64, Constant.HOTEL_FOLDER_TYPE, hotelMaster.Id, userDetails.Id);
                }
            }
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
            if (hotelMaster != null)
            {
                imgser.DeleteFiles(hotelMaster.Id, Constant.HOTEL_FOLDER_TYPE);
            }
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