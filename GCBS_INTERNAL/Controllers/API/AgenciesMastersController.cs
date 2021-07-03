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
using GCBS_INTERNAL.Services;

namespace GCBS_INTERNAL.Controllers.API
{
    [Authorize]
    public class AgenciesMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public ImageServices imgser = new ImageServices();

        // GET: api/AgenciesMasters
        public IQueryable<AgenciesMaster> GetAgenciesMaster()
        {
            return db.AgenciesMaster;
        }

        // GET: api/AgenciesMasters/5
        [ResponseType(typeof(AgenciesMaster))]
        public async Task<IHttpActionResult> GetAgenciesMaster(int id)
        {
            AgenciesMaster agenciesMaster = await db.AgenciesMaster.FindAsync(id);
            if (agenciesMaster == null)
            {
                return NotFound();
            }

            return Ok(agenciesMaster);
        }

        // PUT: api/AgenciesMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAgenciesMaster(int id, AgenciesMaster agenciesMaster)
        {
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
                d.Dispose();
            }
            agenciesMaster.UpdatedBy = userDetails.Id;
            agenciesMaster.UpdatedOn = DateTime.Now;
            db.Entry(agenciesMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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
            agenciesMaster.Status = agenciesMaster.Status;
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
                        imgser.SaveImage(imgbase64, "Agencies", agenciesMaster.Id, userDetails.Id);
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
                imgser.DeleteFiles(agenciesMaster.Id);
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