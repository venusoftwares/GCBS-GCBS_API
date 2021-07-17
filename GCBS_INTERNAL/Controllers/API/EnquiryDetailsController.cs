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
            var res = await db.EnquiryDetails
                .Include(x => x.UserManagements)
                .Include(x => x.PartnerManagements)
                .Include(x => x.servicesMasters)
                .Select(x => new EnquiryViewModel
                {
                    Id = x.Id,
                    Email = x.UserManagements.EmailId,
                    Service = x.servicesMasters.Service,
                    ServicePartner = x.PartnerManagements.Username+"_"+ x.PartnerId,
                    ServicePartnerId = x.PartnerId,
                    Username = x.UserManagements.Username+"_"+x.UserId,
                    UserId = x.UserId,
                    ServiceStatus = x.ServiceStatus,
                    ServiceStatusToString = x.ServiceStatus == 1 ? "Active" : x.ServiceStatus == 2 ? "Completed" : x.ServiceStatus == 3 ? "Cancel" : "None",
                    PaymentStatus = x.PaymentStatus,
                    PaymentStatusToString = x.PaymentStatus == 1 ? "Active" : x.PaymentStatus == 2 ? "Completed" : x.PaymentStatus == 3 ? "Cancel" : "None",
                    ServiceId = x.ServiceId,
                    Mobile = x.UserManagements.MobileNo,
                    ServiceDate = x.ServiceDate
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