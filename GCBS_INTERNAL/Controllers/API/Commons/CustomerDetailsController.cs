using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Commons
{
    [CustomAuthorize]
    public class CustomerDetailsController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        [HttpPut]
        [Route("api/UpdateCustomerImage")]
        public async Task<IHttpActionResult> UpdateImage(Base64string base64String)
        {
            var us = await db.UserManagement.FindAsync(userDetails.Id);
            us.Image = base64String.base64stringFormat;
            us.UpdatedBy = userDetails.Id;
            us.UpdatedOn = DateTime.Now;
            db.Entry(us).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("api/UserChangePassword")]
        public async Task<IHttpActionResult> UserChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var us = await db.UserManagement.FindAsync(userDetails.Id);
            if (us.Password == changePasswordViewModel.OldPassword && changePasswordViewModel.OldPassword != changePasswordViewModel.NewPassword)
            {
                if (changePasswordViewModel.NewPassword == changePasswordViewModel.ReTypePassword)
                {
                    us.Password = changePasswordViewModel.NewPassword;
                    us.UpdatedBy = userDetails.UpdatedBy;
                    us.UpdatedOn = DateTime.Now;

                    db.Entry(us).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return StatusCode(HttpStatusCode.OK);
                }
                else
                {

                    return StatusCode(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        [Route("api/CustomerDashboardCountDetails")]
        public IHttpActionResult CustomerDashboardCountDetails()
        {
            try
            {
                CustomerDashbordDetails customerDashbordDetails = new CustomerDashbordDetails()
                {
                    AcceptedBooking = db.CustomerBooking.Where(x => x.CustomerId == userDetails.Id && x.Status == Constant.CUSTOMER_BOOKING_STATUS_ACCEPT).Count(),
                    CanceledBooking = db.CustomerBooking.Where(x => x.CustomerId == userDetails.Id && x.Status == Constant.CUSTOMER_BOOKING_STATUS_CANCELED).Count(),
                    CompletedBooking = db.CustomerBooking.Where(x => x.CustomerId == userDetails.Id && x.Status == Constant.CUSTOMER_BOOKING_STATUS_COMPLETED).Count(),
                    RejectedBooking = db.CustomerBooking.Where(x => x.CustomerId == userDetails.Id && x.Status == Constant.CUSTOMER_BOOKING_STATUS_REJECTED).Count(),
                    OpenBooking = db.CustomerBooking.Where(x => x.CustomerId == userDetails.Id && x.Status == Constant.CUSTOMER_BOOKING_STATUS_OPENED).Count(),
                }; 
                return Ok(customerDashbordDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("api/GetPartnerStatus")]
        public IHttpActionResult GetPartnerStatus()
        {
            try
            {
                var response = db.UserManagement.Find(userDetails.Id);

                StatusDetails statusDetails = new StatusDetails()
                {
                    Id = (int)response.AccessStatus,
                    Status = response.AccessStatus == 1 ? "Active" : response.AccessStatus == 0 ? "Deactive" : response.AccessStatus == 2 ? "Blocked" : ""
                };

                return Ok(statusDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut]
        [Route("api/GetPartnerStatus/AccessStatus/{status}")]
        public async Task<IHttpActionResult> PutPartnerStatusAsync(int status)
        {
            try
            {
                var response =await db.UserManagement.FindAsync(userDetails.Id);

                response.AccessStatus = status;

                response.UpdatedOn = DateTime.Now;

                response.UpdatedBy = userDetails.Id;


                db.Entry(response).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Ok();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("api/PutPartnerStatus/partnerId/{partnerId}/emailflag/{email}")]
        public async Task<IHttpActionResult> PutPartnerEmailStatusAsync(int partnerId, bool email)
        {
            try
            {
                var response =await db.UserManagement.FindAsync(partnerId);

                response.Status = email;

                response.UpdatedOn = DateTime.Now;

                response.UpdatedBy = userDetails.Id;

                db.Entry(response).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Ok(response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("api/PutPartnerStatus/partnerId/{partnerId}/kycflag/{kyc}")]
        public async Task<IHttpActionResult> PutPartnerKycStatusAsync(int partnerId, bool kyc)
        {
            try
            {
                var response = await db.UserManagement.FindAsync(partnerId);

                response.KycVerification = kyc;

                response.UpdatedOn = DateTime.Now;

                response.UpdatedBy = userDetails.Id;

                db.Entry(response).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Ok(response);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class StatusDetails
        {
            public int Id { get; set; }
            public string Status { get; set; }
        }

        public class CustomerDashbordDetails
        {
            public int OpenBooking { get; set; }

            public int AcceptedBooking { get; set; }
            public int CompletedBooking { get; set; }
            public int RejectedBooking { get; set; }
            public int CanceledBooking { get; set; }
        }
        public class Base64string
        {
            public string base64stringFormat { get; set; }
        }

        public class ChangePasswordViewModel
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }

            public string ReTypePassword { get; set; }
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
