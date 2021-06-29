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
    public class TaxSettingsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

    
        // GET: api/TaxSettings/5
        [ResponseType(typeof(TaxSettings))]
        public async Task<IHttpActionResult> GetTaxSettings()
        {
            TaxSettings taxSettings = await db.TaxSettings.FindAsync(1);
            if (taxSettings == null)
            {
                return NotFound();
            }

            return Ok(taxSettings);
        }

        // PUT: api/TaxSettings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTaxSettings(int id, TaxSettings taxSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taxSettings.Id)
            {
                return BadRequest();
            }
            using(var db3 = new DatabaseContext())
            {
                TaxSettings tax = await db3.TaxSettings.FindAsync(id);
                taxSettings.CreatedBy = tax.CreatedBy;
                taxSettings.CreatedOn = tax.CreatedOn;
                db3.Dispose();    
            }

            taxSettings.UpdatedBy = userDetails.Id;
            taxSettings.UpdatedOn = DateTime.Now;
            db.Entry(taxSettings).State = EntityState.Modified;     
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxSettingsExists(id))
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

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaxSettingsExists(int id)
        {
            return db.TaxSettings.Count(e => e.Id == id) > 0;
        }
    }
}