using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Booking;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Admin
{
    public class AdminDashboardDetailsController : BaseApiController
    {


        private readonly DatabaseContext db = new DatabaseContext();
        [Route("api/admindashboardDetails")]
        [HttpGet]
        public async Task<IHttpActionResult> AdmindashboardDetailsAsync()
        {
            try
            {
                AdminDashboardViewModel adminDashboardViewModel = new AdminDashboardViewModel();


                var customerBookingDetails = await db.CustomerBooking.ToListAsync();

                adminDashboardViewModel.Totalbooking = customerBookingDetails.Count();

                adminDashboardViewModel.CompleteBooking = customerBookingDetails.Where(x => x.Status == Constant.CUSTOMER_BOOKING_STATUS_COMPLETED).Count();

                adminDashboardViewModel.OpenBooking = customerBookingDetails.Where(x => x.Status == Constant.CUSTOMER_BOOKING_STATUS_OPENED).Count();

                adminDashboardViewModel.AcceptBooking = customerBookingDetails.Where(x => x.Status == Constant.CUSTOMER_BOOKING_STATUS_ACCEPT).Count();

                adminDashboardViewModel.CanceledBooking = customerBookingDetails.Where(x => x.Status == Constant.CUSTOMER_BOOKING_STATUS_CANCELED).Count();

                adminDashboardViewModel.RejectedBooking = customerBookingDetails.Where(x => x.Status == Constant.CUSTOMER_BOOKING_STATUS_REJECTED).Count();

                adminDashboardViewModel.ReportCount = await db.Reports.CountAsync();

                adminDashboardViewModel.SupportCount = await db.Support.CountAsync();

                adminDashboardViewModel.ContactCount = await db.ContactEnquiryView.CountAsync();

                adminDashboardViewModel.ServicePartnerCount = await db.UserManagement.Where(x=>x.RoleId == Constant.PARTNER_ROLE_ID).CountAsync();

                adminDashboardViewModel.CustomerCount = await db.UserManagement.Where(x => x.RoleId == Constant.CUSTOMER_ROLE_ID).CountAsync();

                adminDashboardViewModel.TotalEarnings = TotalEarningGet(customerBookingDetails);

                adminDashboardViewModel.TotalPayout = await db.PartnerPayoutDetails.Where(x=>x.Status).Select(x => x.Amount).DefaultIfEmpty(0).SumAsync();

                adminDashboardViewModel.TotalPedingPayout = await db.PartnerPayoutDetails.Where(x => !x.Status).Select(x => x.Amount).DefaultIfEmpty(0).SumAsync();

                adminDashboardViewModel.BillingAmount = TotalBillingAmount(customerBookingDetails);

                adminDashboardViewModel.TotalTax = TotalTax(customerBookingDetails);

                return Ok(adminDashboardViewModel);


                
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        private decimal TotalEarningGet(List<CustomerBooking> customerBookingDetails)
        {
            decimal total = 0;
            foreach(var i in customerBookingDetails)
            {
                total = total + i.JsonReponse.Margin;
            }
            return total; 
        }
        private decimal TotalBillingAmount(List<CustomerBooking> customerBookingDetails)
        {
            decimal total = 0;
            foreach (var i in customerBookingDetails)
            {
                total = total + i.JsonReponse.TotalPrices.Total;
            }
            return total;
        }
        private decimal TotalTax(List<CustomerBooking> customerBookingDetails)
        {
            decimal total = 0;
            foreach (var i in customerBookingDetails)
            {
                total = total + i.JsonReponse.Tax;
            }
            return total;
        }
        public class AdminDashboardViewModel
        {
            public int Totalbooking { get; set; }
            public int CompleteBooking { get; set; }
            public int OpenBooking { get; set; }
            public int AcceptBooking { get; set; }
            public int CanceledBooking { get; set; }
          
            public int RejectedBooking { get; set; }
            public int ReportCount { get; set; }
            public int SupportCount { get; set; }
            public int ContactCount { get; set; }
            public int ServicePartnerCount { get; set; }
            public int CustomerCount { get; set; }
            public decimal TotalEarnings { get; set; }
            public decimal TotalPayout { get; set; }

            public decimal TotalPedingPayout { get; set; }

            public decimal BillingAmount { get; set; }
            public decimal TotalTax { get; set; }
        }

    } 
   
}
