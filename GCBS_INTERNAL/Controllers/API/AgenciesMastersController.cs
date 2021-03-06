using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using GCBS_INTERNAL.Services;
using log4net;

namespace GCBS_INTERNAL.Controllers.API
{
    // [CustomAuthorize]    
    [CustomAuthorize]
    public class AgenciesMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public ImageServices imgser = new ImageServices();
       
        // GET: api/AgenciesMasters
        public IHttpActionResult GetAgenciesMaster()
        {
            try
            {
                List<AgenciesMasterView> list = new List<AgenciesMasterView>();
                var res = db.AgenciesMaster.Include(x => x.LocationMasters).ToList();
                foreach (var a in res)
                {
                    // First or default 
                    string path = imgser.GetFiles(a.Id, Constant.AGENCIES_FOLDER_TYPE).FirstOrDefault();
                    list.Add(new AgenciesMasterView
                    {
                        Id = a.Id,
                        Status = a.Status,
                        Email = a.Email,
                        HotelName = a.HotelName,
                        Location = a.Location,
                        WebsiteUrl = a.WebsiteUrl,
                        ValidEndDate = a.ValidEndDate.ToString("dd-MM-yyyy hh:mm tt"),
                        ValidStartDate = a.ValidStartDate.ToString("dd-MM-yyyy hh:mm tt"),
                        Image = path,
                        LocationMasters = a.LocationMasters
                    });
                }
                return Ok(list);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }
                
        }

        // GET: api/AgenciesMasters/5
        [ResponseType(typeof(AgenciesMasterViewModel))]
        public async Task<IHttpActionResult> GetAgenciesMaster(int id)
        {
            AgenciesMaster agenciesMaster = await db.AgenciesMaster.FindAsync(id);
            if (agenciesMaster == null)
            {
                return NotFound();
            }
            AgenciesMasterViewModel agenciesMasterViewModel = new AgenciesMasterViewModel();
            agenciesMasterViewModel.AgenciesMaster = agenciesMaster;
            agenciesMasterViewModel.imageBase64 = imgser.EditGetFiles(id, Constant.AGENCIES_FOLDER_TYPE);
            agenciesMasterViewModel.LocationMasters =await db.LocationMasters.FindAsync(agenciesMaster.Location);
            return Ok(agenciesMasterViewModel);
        }

        // PUT: api/AgenciesMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAgenciesMaster(int id, AgenciesMasterViewModel agenciesMasterViewModel)
        {
            AgenciesMaster agenciesMaster = agenciesMasterViewModel.AgenciesMaster;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != agenciesMaster.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.AgenciesMaster.FindAsync(id);
                agenciesMaster.CreatedBy = re.CreatedBy;
                agenciesMaster.CreatedOn = re.CreatedOn;
                agenciesMaster.Status = re.Status;
                d.Dispose();
            }
            agenciesMaster.UpdatedBy = userDetails.Id;
            agenciesMaster.UpdatedOn = DateTime.Now;
            db.Entry(agenciesMaster).State = EntityState.Modified;     
            try
            {
                await db.SaveChangesAsync();
                if (agenciesMaster.Id > 0 && agenciesMasterViewModel.imageBase64.Count() > 0)
                {
                    imgser.DeleteFiles(agenciesMaster.Id, Constant.AGENCIES_FOLDER_TYPE);
                    foreach (var imgbase64 in agenciesMasterViewModel.imageBase64)
                    {      
                        imgser.SaveImage(imgbase64, Constant.AGENCIES_FOLDER_TYPE, agenciesMaster.Id, userDetails.Id);
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenciesMasterExists(id))
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
        public async Task<IHttpActionResult> PutAgenciesMaster(AgenciesVisible agenciesVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (agenciesVisible == null)
            {
                return BadRequest();
            }
            AgenciesMaster agenciesMaster = await db.AgenciesMaster.FindAsync(agenciesVisible.Id);
            agenciesMaster.Status = agenciesVisible.Status;
            agenciesMaster.UpdatedBy = userDetails.Id;
            agenciesMaster.UpdatedOn = DateTime.Now;
            db.Entry(agenciesMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/AgenciesMasters
        [ResponseType(typeof(AgenciesMaster))]    
        public async Task<IHttpActionResult> PostAgenciesMaster(AgenciesMasterViewModel agenciesMasterViewModel)
        {
            try
            {
                string a = "";
                AgenciesMaster agenciesMaster = agenciesMasterViewModel.AgenciesMaster;
                agenciesMaster.CreatedBy = userDetails.Id;
                agenciesMaster.CreatedOn = DateTime.Now;
                agenciesMaster.Status = true;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                db.AgenciesMaster.Add(agenciesMaster);
                await db.SaveChangesAsync();
                if (agenciesMaster.Id > 0 && agenciesMasterViewModel.imageBase64.Count()>0)
                {
                    foreach (var imgbase64 in agenciesMasterViewModel.imageBase64)
                    {
                        imgser.SaveImage(imgbase64, Constant.AGENCIES_FOLDER_TYPE, agenciesMaster.Id, userDetails.Id);
                    }
                }
                return CreatedAtRoute("DefaultApi", new { id = agenciesMaster.Id }, agenciesMaster);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // DELETE: api/AgenciesMasters/5
        [ResponseType(typeof(AgenciesMaster))]
        public async Task<IHttpActionResult> DeleteAgenciesMaster(int id)
        {
            AgenciesMaster agenciesMaster = await db.AgenciesMaster.FindAsync(id);
            if (agenciesMaster == null)
            {
                return NotFound();
            }

            db.AgenciesMaster.Remove(agenciesMaster);
            await db.SaveChangesAsync();
            if(agenciesMaster!=null)
            {
                imgser.DeleteFiles(agenciesMaster.Id, Constant.AGENCIES_FOLDER_TYPE);
            }   
            return Ok(agenciesMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AgenciesMasterExists(int id)
        {
            return db.AgenciesMaster.Count(e => e.Id == id) > 0;
        }
    }
}