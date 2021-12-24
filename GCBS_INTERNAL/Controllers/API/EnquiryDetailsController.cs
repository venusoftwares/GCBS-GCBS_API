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
using GCBS_INTERNAL.Models.Booking;
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
            var s = await db.CustomerBooking.ToListAsync();
            var list =  db.CustomerBooking
                .Include(x => x.CustomerManagement)
                .Include(x => x.UserManagement).OrderByDescending(x=>x.Id).ToList();


            List<EnquiryViewModel> res = new List<EnquiryViewModel>();
            
            foreach(CustomerBooking x in list)
            {
                res.Add(new EnquiryViewModel
                {
                    Id = x.Id,
                    Email = x.CustomerManagement.EmailId,
                    ServicePartner = x.UserManagement.FirstName + "_" + x.ProviderId,
                    ServicePartnerId = x.Id,
                    Username = x.CustomerManagement.FirstName + "_" + x.CustomerId,
                    UserId = x.CustomerId,
                    ServiceStatus = x.Status,
                    ServiceStatusToString = x.Status == 1 ? "Open" : x.Status == 2 ? "Canceled" : x.Status == 3 ? "Completed" : x.Status == 4 ? "Rejected" : x.Status == 5 ? "Accepted" : "",
                    Mobile = x.CustomerManagement.MobileNo,
                    ServiceDate = x.DateTime,
                    BookingDate = (DateTime)x.CreatedOn,
                    PartnerStatus = x.PartnerStatus
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