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

namespace GCBS_INTERNAL.Controllers.API
{
    [Authorize]
    public class SiteBannerMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public ImageServices imgser = new ImageServices();

        // GET: api/SiteBannerMasters
        public List<SiteBannerViewIndex> GetSiteBannerMasters()
        {
            List<SiteBannerViewIndex> list = new List<SiteBannerViewIndex>();
            var res = db.SiteBannerMasters.ToList();
            foreach(var a in res)
            {
                string path = imgser.GetFiles(a.Id, Constant.SITE_BANNER_FOLDER_TYPE).FirstOrDefault();
                list.Add(new SiteBannerViewIndex
                {  
                    Id = a.Id,
                    Image = path,
                    MainHeading = a.MainHeading,
                    MainTitle = a.MainTitle,
                    Status = a.Status
                });
            }
            return list;
        }

        // GET: api/SiteBannerMasters/5
        [ResponseType(typeof(SiteBannerViewModel))]
        public async Task<IHttpActionResult> GetSiteBannerMaster(int id)
        {
            SiteBannerMaster siteBannerMaster = await db.SiteBannerMasters.FindAsync(id);
            if (siteBannerMaster == null)
            {
                return NotFound();
            }
            SiteBannerViewModel siteBannerViewModel = new SiteBannerViewModel();
            siteBannerViewModel.SiteBannerMasters = siteBannerMaster;
            siteBannerViewModel.imageBase64 = imgser.EditGetFiles(id, Constant.SITE_BANNER_FOLDER_TYPE);   
            return Ok(siteBannerViewModel);
        }

        // PUT: api/SiteBannerMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSiteBannerMaster(int id, SiteBannerViewModel siteBannerViewModel)
        {
            SiteBannerMaster siteBannerMaster = siteBannerViewModel.SiteBannerMasters;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != siteBannerMaster.Id)
            {
                return BadRequest();
            }
            using(var db2 = new DatabaseContext())
            {
                SiteBannerMaster siteBanner = await db2.SiteBannerMasters.FindAsync(id);
                siteBannerMaster.CreatedOn = DateTime.Now;
                siteBannerMaster.CreatedBy = userDetails.Id;
                db2.Dispose();
            }
            siteBannerMaster.UpdatedOn = DateTime.Now;
            siteBannerMaster.UpdatedBy = userDetails.Id;
            db.Entry(siteBannerMaster).State = EntityState.Modified;    
            try
            {
                await db.SaveChangesAsync();
                if (siteBannerMaster.Id > 0 && siteBannerViewModel.imageBase64.Count() > 0)
                {
                    //Delete
                    imgser.DeleteFiles(siteBannerMaster.Id, Constant.SITE_BANNER_FOLDER_TYPE);
                    foreach (var imgbase64 in siteBannerViewModel.imageBase64)
                    {
                        //Save
                        imgser.SaveImage(imgbase64, Constant.SITE_BANNER_FOLDER_TYPE, siteBannerMaster.Id, userDetails.Id);
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteBannerMasterExists(id))
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
        public async Task<IHttpActionResult> PutSiteBannerMaster(SiteBannerVisible siteBannerVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (siteBannerVisible == null)
            {
                return BadRequest();
            }
            SiteBannerMaster siteBannerMaster = await db.SiteBannerMasters.FindAsync(siteBannerVisible.Id);
            siteBannerMaster.Status = siteBannerVisible.Status;
            siteBannerMaster.UpdatedOn = DateTime.Now;
            siteBannerMaster.UpdatedBy = userDetails.Id;
            db.Entry(siteBannerMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SiteBannerMasters
        [ResponseType(typeof(SiteBannerMaster))]
        public async Task<IHttpActionResult> PostSiteBannerMaster(SiteBannerViewModel siteBannerViewModel)
        {
            SiteBannerMaster siteBannerMaster = siteBannerViewModel.SiteBannerMasters;
            if (siteBannerMaster!=null)
            {
                siteBannerMaster.CreatedBy = userDetails.Id;
                siteBannerMaster.CreatedOn = DateTime.Now;
            }     
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SiteBannerMasters.Add(siteBannerMaster);
            await db.SaveChangesAsync();
            if (siteBannerMaster.Id > 0 && siteBannerViewModel.imageBase64.Count() > 0)
            {
                foreach (var imgbase64 in siteBannerViewModel.imageBase64)
                {
                    imgser.SaveImage(imgbase64, Constant.SITE_BANNER_FOLDER_TYPE, siteBannerMaster.Id, userDetails.Id);
                }
            }
            return CreatedAtRoute("DefaultApi", new { id = siteBannerMaster.Id }, siteBannerMaster);
        }

        // DELETE: api/SiteBannerMasters/5
        [ResponseType(typeof(SiteBannerMaster))]
        public async Task<IHttpActionResult> DeleteSiteBannerMaster(int id)
        {
            SiteBannerMaster siteBannerMaster = await db.SiteBannerMasters.FindAsync(id);
            if (siteBannerMaster == null)
            {
                return NotFound();
            }

            db.SiteBannerMasters.Remove(siteBannerMaster);
            await db.SaveChangesAsync();

            return Ok(siteBannerMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SiteBannerMasterExists(int id)
        {
            return db.SiteBannerMasters.Count(e => e.Id == id) > 0;
        }
    }
}