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
    public class PayoutDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/PayoutDetails
        public IQueryable<PayoutViewModel> GetPayoutDetails(int id)
        {
            var res = db.PayoutDetails
                .Include(x=>x.UserManagement);   
            if(id >0)
            {
                res = res.Where(x => x.UserManagement.Id == id);
            }
            return res.Select(x => new PayoutViewModel
            {
                Id = x.Id,
                Date = x.PayoutDate.ToString(Constant.DATE_FORMAT),
                PartnerId = x.PartnerId,
                PartnerName = x.UserManagement.Username,
                Status = x.Status,
                Payment = x.Payment
            });
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