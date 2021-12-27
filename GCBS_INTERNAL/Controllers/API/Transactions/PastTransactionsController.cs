using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;  
using System.Data.Entity;
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API.Transactions
{
     [CustomAuthorize]
    public class PastTransactionsController : BaseApiController
    {
        //(Service Status completed or cancelled) and Partner status pending
        private DatabaseContext db = new DatabaseContext();
        [ResponseType(typeof(PastTransactionsViewModel))]
        public async Task<IHttpActionResult> PostOpenTransactions(PastTransactionsViewModel enquiryViewModel)
        {
            var list = db.PartnerPayoutDetails.Include(x => x.customerBooking).Include(x => x.userManagement).Where(x => x.Status == true && x.customerBooking.Status == Constant.CUSTOMER_BOOKING_STATUS_COMPLETED)
                  .ToList();


            List<EnquiryViewModel> res = new List<EnquiryViewModel>();

            foreach (var x in list)
            {
                res.Add(new EnquiryViewModel
                {
                    Id = x.customerBooking.Id,
                    Email = x.userManagement.EmailId,
                    ServicePartner = x.userManagement.FirstName + "_" + x.PartnerId,
                    ServicePartnerId = x.Id,
                    ServiceStatus = x.customerBooking.Status,
                    ServiceStatusToString = x.customerBooking.Status == 1 ? "Opened"
                    : x.customerBooking.Status == 2 ? "Canceled" : x.customerBooking.Status == 3 ? "Completed" : x.customerBooking.Status == 4 ? "Rejected" : x.customerBooking.Status == 5 ? "Accepted" : "",
                    Mobile = x.userManagement.MobileNo,
                    ServiceDate = x.customerBooking.DateTime
                });
            }


            if (enquiryViewModel != null)
            {
                if (enquiryViewModel.UserId > 0)
                {
                    res = res.Where(x => x.UserId == enquiryViewModel.UserId).ToList();
                }
                if (enquiryViewModel.ServicePartnerId > 0)
                {
                    res = res.Where(x => x.ServicePartnerId == enquiryViewModel.ServicePartnerId).ToList();
                }
                if (enquiryViewModel.ServiceId > 0)
                {
                    res = res.Where(x => x.ServiceId == enquiryViewModel.ServiceId).ToList();
                }
                if (!string.IsNullOrEmpty(enquiryViewModel.Username))
                {
                    res = res.Where(x => x.Username.ToLower() == enquiryViewModel.Username.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(enquiryViewModel.Email))
                {
                    res = res.Where(x => x.Email.ToLower() == enquiryViewModel.Email.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(enquiryViewModel.Mobile))
                {
                    res = res.Where(x => x.Mobile.ToLower() == enquiryViewModel.Mobile.ToLower()).ToList();
                }
                if (enquiryViewModel.ServiceStatus > 0)
                {
                    res = res.Where(x => x.ServiceStatus == enquiryViewModel.ServiceStatus).ToList();
                }
                if (enquiryViewModel.FromDate != null && enquiryViewModel.ToDate != null)
                {
                    res = res.Where(x => x.ServiceDate >= Convert.ToDateTime(enquiryViewModel.FromDate)
                    && x.ServiceDate <= Convert.ToDateTime(enquiryViewModel.ToDate)).ToList();
                }
            }
            return Ok(res);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EnquiryDetailsExists(int id)
        {
            return db.EnquiryDetails.Count(e => e.Id == id) > 0;
        }
    }
    
}
