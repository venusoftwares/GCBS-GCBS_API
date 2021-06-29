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
                    ServicePartner = x.PartnerManagements.Username,
                    ServicePartnerId = x.PartnerId,
                    Username = x.UserManagements.Username,
                    UserId = x.UserId,
                    ServiceStatus = x.EnquiryStatus,
                    UserStatus = x.UserStatus,
                    ServiceId = x.ServiceId,
                    Mobile = x.UserManagements.MobileNo  ,
                    EnquiryDate = x.EnquiryDate
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
                if (!string.IsNullOrEmpty(enquiryViewModel.ServiceStatus))
                {
                    res = res.Where(x => x.ServiceStatus.ToLower() == enquiryViewModel.ServiceStatus.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(enquiryViewModel.UserStatus))
                {
                    res = res.Where(x => x.UserStatus.ToLower() == enquiryViewModel.UserStatus.ToLower()).ToList();
                }
                if (enquiryViewModel.FromDate != null && enquiryViewModel.ToDate != null)
                {
                    res = res.Where(x => x.EnquiryDate >= Convert.ToDateTime(enquiryViewModel.FromDate)
                    && x.EnquiryDate <= Convert.ToDateTime(enquiryViewModel.ToDate)).ToList();
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