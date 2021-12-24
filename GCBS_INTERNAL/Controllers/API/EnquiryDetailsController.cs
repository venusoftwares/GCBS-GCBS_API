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
    public class EnquiryDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/EnquiryDetails
        [ResponseType(typeof(EnquiryViewModel))]
        public async Task< IHttpActionResult> PostEnquiryDetails(EnquiryViewModel enquiryViewModel)
        {
            var res = await db.CustomerBooking
                .Include(x => x.CustomerManagement)
                .Include(x => x.UserManagement) 
                .Select(x => new EnquiryViewModel
                {
                    Id = x.Id,
                    Email = x.CustomerManagement.EmailId,                   
                    ServicePartner = x.UserManagement.Username+"_"+ x.ProviderId,
                    ServicePartnerId = x.Id,
                    Username = x.CustomerManagement.Username+"_"+x.CustomerId,
                    UserId = x.CustomerId,
                    ServiceStatus = x.PartnerStatus,
                    ServiceStatusToString = x.Status == 1 ? "Open" : x.PartnerStatus == 2 ? "Completed" : x.PartnerStatus == 3 ? "Closed" : x.PartnerStatus ==4? "Rejected" : x.PartnerStatus == 5 ? "Accepted": "" ,
                    PaymentStatus =x.Status,
                    PaymentStatusToString = x.Status == 1 ? Constant.PAYOUT_PENDING_STATUS_STRING : x.Status == 2 ? Constant.PAYOUT_SUCCESS_STATUS_STRING : "None",                    
                    Mobile = x.CustomerManagement.MobileNo,
                    ServiceDate = x.DateTime,
                    PartnerStatus = x.PartnerStatus
                }).ToListAsync();

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