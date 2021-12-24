using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Booking;
using GCBS_INTERNAL.Provider;
using GCBS_INTERNAL.ViewModels;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API
{
    [CustomAuthorize]
    public class BookingController : BaseApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DatabaseContext db = new DatabaseContext();

        [Route("api/customerDurationDropDown")]
        public async Task<IHttpActionResult> CustomerDurationDropDown()
        {
            return Ok(await db.DurationMaster.Where(x => x.Status).ToListAsync());
        }
        //Todo Date Values get drop down that availability here
        [Route("api/partnerId/{partnerId}/datetime/{dateTime}/customerDateDropDown")]
        public async Task<IHttpActionResult> CustomerDateDropDown(int partnerId, DateTime dateTime)
        {
            string time = "";
            var getTime = await db.Availability.Where(x => x.UserId == partnerId).FirstOrDefaultAsync();

            if (getTime != null)
            {
                var list = JsonConvert.DeserializeObject<List<Root>>(getTime.Time);
                foreach (var i in list)
                {
                    if (i.Day.ToUpper() == dateTime.Date.DayOfWeek.ToString().ToUpper())
                    {
                        time = $"{i.Time[0].StartTime} {i.Time[0].EndTime}";
                    }
                }
            }
            return Ok(new { Key = time });
        }

        [Route("api/PartnerId/{partnerId}/customerBookingServiceTypes")]
        public async Task<IHttpActionResult> CustomerBookingServiceTypes(int partnerId)
        {
            var result = await db.ServicesMasters
                   .Where(x => x.Visible)
                   .Select(x => new DropDownCommon { Key = x.Id, Value = x.Service }).ToListAsync();
            return Ok(result);
        }

        [Route("api/PartnerId/{partnerId}/ServiceTypeId/{serviceTypeId}/DurationId/{DurationId}/ServicesPrice")]
        public async Task<IHttpActionResult> CustomerBookingServiceTypes(int partnerId, int serviceTypeId, int DurationId)
        {
            var result = await db.ServiceDurartionPrice
                   .Where(x => x.UserId == partnerId && x.ServiceId == serviceTypeId && x.DurationId == DurationId)
                   .Select(x => x.Price).DefaultIfEmpty(0).SumAsync();

            return Ok(new { Key = result });
        }

        [Route("api/submitCustomerBooking")]
        public async Task<IHttpActionResult> SubmitCustomerBooking(CustomerBooking customerBooking)
        {
            try
            {

                if (customerBooking == null)
                {
                    return BadRequest();
                }

                log.Debug("SubmitCustomerBooking" + JsonConvert.SerializeObject(customerBooking));

                customerBooking.CustomerId = userDetails.Id;

                customerBooking.CreatedBy = userDetails.Id;

                customerBooking.CreatedOn = DateTime.Now;

                customerBooking.Status = Constant.CUSTOMER_BOOKING_STATUS_OPENED;

                //Price fixing 

                var totalAmount = customerBooking.TotalPrice;

                var marginMaster = await db.MarginMaster.Where(x => x.Status).FirstOrDefaultAsync();

                var margin = marginMaster.CommissionPer;

                db.CustomerBooking.Add(customerBooking);

                await db.SaveChangesAsync();

                log.Debug($"SubmitCustomerBooking" + JsonConvert.SerializeObject(customerBooking.Id));

                return Ok(customerBooking);
            }
            catch (Exception ex)
            {
                log.Error("SubmitCustomerBooking" + JsonConvert.SerializeObject(customerBooking));
                throw ex;
            }
        }
        #region Customer
        [Route("api/CustomerOrPartnerBookingListDetails")]

        public async Task<IHttpActionResult> CustomerOrPartnerBookingListDetails()
        {
            try
            {
                log.Debug("CustomerOrPartnerBookingListDetails");
                List<CustomerBooking> list = new List<CustomerBooking>();

                if (userDetails.RoleId == 3)
                {
                    list = await db.CustomerBooking
                  .Include(x => x.UserManagement)
                  .Where(x => x.ProviderId == userDetails.Id).ToListAsync();
                }
                else
                {
                    list = await db.CustomerBooking
                 .Include(x => x.UserManagement)
                 .Where(x => x.CustomerId == userDetails.Id).ToListAsync();
                }


                var list2 = list.Select(x =>
               new BookingList
               {
                   Id = x.Id,
                   Amount = x.TotalPrice.ToString(),
                   BookingDate = x.DateTime,
                   BookingTime = x.TimeSlot,
                   Status = "Opened",
                   Image = x.UserManagement.Image,
                   StausInt = x.Status

               }).ToList();

                BoolingListViewModel boolingListViewModel = new BoolingListViewModel();

                boolingListViewModel.OpenBookingList = list2.Where(x => x.StausInt == Constant.CUSTOMER_BOOKING_STATUS_OPENED).ToList();
                boolingListViewModel.AcceptBookingList = list2.Where(x => x.StausInt == Constant.CUSTOMER_BOOKING_STATUS_ACCEPT).ToList();
                boolingListViewModel.CompletedBookingList = list2.Where(x => x.StausInt == Constant.CUSTOMER_BOOKING_STATUS_COMPLETED).ToList();
                boolingListViewModel.RejectedBookingList = list2.Where(x => x.StausInt == Constant.CUSTOMER_BOOKING_STATUS_REJECTED).ToList();
                boolingListViewModel.CancelBookingList = list2.Where(x => x.StausInt == Constant.CUSTOMER_BOOKING_STATUS_CANCELED).ToList();

                log.Debug($"CustomerOrPartnerBookingListDetails");


                return Ok(boolingListViewModel);
            }


            catch (Exception ex)
            {
                log.Error("CustomerOrPartnerBookingListDetails" + ex.Message);
                throw ex;
            }
        }

        [Route("api/CustomerOrPartnerPaymentHistory")]

        public async Task<IHttpActionResult> CustomerOrPartnerPaymentHistory()
        {
            try
            {
                log.Debug("CustomerOrPartnerPaymentHistory");
                List<CustomerBooking> list = new List<CustomerBooking>();
                List<BookingPaymentHistory> histories = new List<BookingPaymentHistory>();

                if (userDetails.RoleId == 3)
                {
                    var list3 = await db.PartnerPayoutDetails.Include(x => x.customerBooking).OrderByDescending(x => x.CreatedAt).Where(x => x.PartnerId == userDetails.Id).ToListAsync();
                    var list2 = list3.Select(x =>
              new BookingPaymentHistory
              {
                  BookingNo = x.Id,
                  BookingDate = Convert.ToDateTime(x.customerBooking.CreatedOn).ToString("yyyy-MM-dd"),
                  ServiceDate = x.customerBooking.DateTime.ToString("yyyy-MM-dd"),
                  ReferenceNo = x.ReferenceNo,
                  Amount = x.Amount,
                  Status = x.Status ? "Transfered" : "Pending"

              }).ToList();
                    return Ok(list2);

                }
                else
                {
                    list = await db.CustomerBooking
                 .Include(x => x.UserManagement)
                 .Where(x => x.CustomerId == userDetails.Id).OrderByDescending(x => x.CreatedOn).ToListAsync();

                    var list2 = list.Select(x =>
               new BookingPaymentHistory
               {
                   BookingNo = x.Id,
                   BookingDate = Convert.ToDateTime(x.CreatedOn).ToString("yyyy-MM-dd"),
                   ServiceDate = x.DateTime.ToString("yyyy-MM-dd"),
                   Amount = x.JsonReponse.CustomerTotal,
                   ReferenceNo = "",
                   Status = ""

               }).ToList();
                    return Ok(list2);
                }
            }

            catch (Exception ex)
            {
                log.Error("CustomerOrPartnerPaymentHistory" + ex.Message);
                throw ex;
            }
        }

        [Route("api/PartnerOrCustomerStatusUpdate/bookigId/{bookingid}/status/{status}")]
        public IHttpActionResult PartnerOrCustomerStatusUpdate(int bookingid, int status)
        {
            try
            {
                log.Debug("PartnerOrCustomerStatusUpdate");
                var customer = db.CustomerBooking.Find(bookingid);
                customer.Status = status;
                customer.UpdatedBy = userDetails.Id;
                customer.UpdatedOn = DateTime.Now;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return Ok();
            }

            catch (Exception ex)
            {
                log.Error("CustomerOrPartnerPaymentHistory" + ex.Message);
                throw ex;
            }
        }
        //        Booking No,
        //        Booking Date, 
        //Service Date,
        //Service Amount , 130 % 7 + 25 = 155
        //ReferenceNumber,
        //Status, (Success)

        public class BookingPaymentHistory
        {
            public int BookingNo { get; set; }
            public string BookingDate { get; set; }
            public string ServiceDate { get; set; }
            public string ReferenceNo { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
        }
        #endregion
        public class BoolingListViewModel
        {
            public List<BookingList> OpenBookingList { get; set; }
            public List<BookingList> AcceptBookingList { get; set; }
            public List<BookingList> CancelBookingList { get; set; }
            public List<BookingList> RejectedBookingList { get; set; }
            public List<BookingList> CompletedBookingList { get; set; }
        }

        public class BookingList
        {
            public int Id { get; set; }
            public DateTime BookingDate { get; set; }
            public string DisplayBookingDate { get { return BookingDate.ToString("dd-MM-yyyy"); } }
            public string BookingTime { get; set; }
            public string Amount { get; set; }
            public string Location { get; set; }
            public string Status { get; set; }
            public int StausInt { get; set; }
            public string Image { get; set; }
            public string ServiceType { get; set; }
            public int UserDetailId { get; set; }
        }
    }
}
