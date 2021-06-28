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
    public class LanguageMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/LanguageMasters
        public IQueryable<LanguageMaster> GetLanguageMaster()
        {
            return db.LanguageMaster;
        }

        // GET: api/LanguageMasters/5
        [ResponseType(typeof(LanguageMaster))]
        public async Task<IHttpActionResult> GetLanguageMaster(int id)
        {
            LanguageMaster languageMaster = await db.LanguageMaster.FindAsync(id);
            if (languageMaster == null)
            {
                return NotFound();
            }

            return Ok(languageMaster);
        }

        // PUT: api/LanguageMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLanguageMaster(int id, LanguageMaster languageMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != languageMaster.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.LanguageMaster.FindAsync(id);
                languageMaster.CreatedBy = re.CreatedBy;
                languageMaster.CreatedOn = re.CreatedOn;
                d.Dispose();
            }
            languageMaster.UpdatedOn = DateTime.Now;
            languageMaster.UpdatedBy = userDetails.Id;
            db.Entry(languageMaster).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageMasterExists(id))
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
        public async Task<IHttpActionResult> PutPriceMaster(LanguageVisible languageVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (languageVisible == null)
            {
                return BadRequest();
            }
            LanguageMaster languageMaster = await db.LanguageMaster.FindAsync(languageVisible.Id);
            languageMaster.Status = languageVisible.Status;
            languageMaster.UpdatedOn = DateTime.Now;
            languageMaster.UpdatedBy = userDetails.Id;
            db.Entry(languageMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/LanguageMasters
        [ResponseType(typeof(LanguageMaster))]
        public async Task<IHttpActionResult> PostLanguageMaster(LanguageMaster languageMaster)
        {
            languageMaster.CreatedBy = userDetails.Id;
            languageMaster.CreatedOn = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            languageMaster.CreatedOn = DateTime.Now;
            languageMaster.CreatedBy = userDetails.Id;
            db.LanguageMaster.Add(languageMaster);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = languageMaster.Id }, languageMaster);
        }

        // DELETE: api/LanguageMasters/5
        [ResponseType(typeof(LanguageMaster))]
        public async Task<IHttpActionResult> DeleteLanguageMaster(int id)
        {
            LanguageMaster languageMaster = await db.LanguageMaster.FindAsync(id);
            if (languageMaster == null)
            {
                return NotFound();
            }
           
            db.LanguageMaster.Remove(languageMaster);
            await db.SaveChangesAsync();

            return Ok(languageMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LanguageMasterExists(int id)
        {
            return db.LanguageMaster.Count(e => e.Id == id) > 0;
        }
    }
}