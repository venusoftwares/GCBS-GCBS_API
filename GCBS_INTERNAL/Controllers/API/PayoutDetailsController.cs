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
    public class PayoutDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PayoutDetails
        public List<PayoutViewModel> GetPayoutDetails(int id)
        {
            List<PayoutViewModel> payoutViewModels = new List<PayoutViewModel>();
            var res = db.PayoutDetails
                .Include(x=>x.UserManagement).Select(x => new PayoutViewModel
                {
                    Id = x.Id,
                    Date = x.PayoutDate,
                    PartnerId = x.PartnerId,
                    PartnerName = x.UserManagement.Username,
                    Status = x.Status,
                    Payment = x.Payment
                }).ToList();   
            if(id >0)
            {
                res = res.Where(x => x.PartnerId == id).ToList();
            }   
            return res;
        }   
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }    
    }
}