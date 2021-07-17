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
namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class BankAccountDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

      

        // GET: api/BankAccountDetails/5
        [ResponseType(typeof(BankAccountDetails))]
        public async Task<IHttpActionResult> GetBankAccountDetails()
        {
            BankAccountDetails bankAccountDetails = await db.BankAccountDetails.Where(x => x.UserId == userDetails.Id).FirstOrDefaultAsync();
            if (bankAccountDetails == null)
            {
                return Ok(new BankAccountDetails { Id = 0, UserId = 0, AccountHolderName = "", BankName = "", AccountNumber = "", IFSCCode = "", RoutingNumber = "" });
            }

            return Ok(bankAccountDetails);
        }

        // PUT: api/BankAccountDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBankAccountDetails(int id, BankAccountDetails bankAccountDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }  
            if (id != bankAccountDetails.Id)
            {
                return BadRequest();
            }
            using (var d = new DatabaseContext())
            {
                var re = await d.BankAccountDetails.FindAsync(id);
                bankAccountDetails.CreatedBy = re.CreatedBy;
                bankAccountDetails.CreatedOn = re.CreatedOn;
                bankAccountDetails.UserId = userDetails.Id;
                d.Dispose();
            }
            bankAccountDetails.UpdatedBy = userDetails.Id;
            bankAccountDetails.UpdatedOn = DateTime.Now;
            db.Entry(bankAccountDetails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountDetailsExists(id))
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

        // POST: api/BankAccountDetails
        [ResponseType(typeof(BankAccountDetails))]
        public async Task<IHttpActionResult> PostBankAccountDetails(BankAccountDetails bankAccountDetails)
        {
            bankAccountDetails.CreatedBy = userDetails.Id;
            bankAccountDetails.CreatedOn = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bankAccountDetails.UserId = userDetails.Id;
            db.BankAccountDetails.Add(bankAccountDetails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = bankAccountDetails.Id }, bankAccountDetails);
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BankAccountDetailsExists(int id)
        {
            return db.BankAccountDetails.Count(e => e.Id == id) > 0;
        }
    }
}