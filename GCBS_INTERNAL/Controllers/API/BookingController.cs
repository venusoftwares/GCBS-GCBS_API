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
        public async Task<IHttpActionResult> CustomerDateDropDown(int partnerId,DateTime dateTime)
        {
            string time = "";
            var getTime =await db.Availability.Where(x => x.UserId == partnerId).FirstOrDefaultAsync();

            if(getTime!=null)
            {
                var list  = JsonConvert.DeserializeObject<List<Root>>(getTime.Time);
                foreach(var i in list)
                {
                    if(i.Day.ToUpper() == dateTime.Date.DayOfWeek.ToString().ToUpper())
                    {
                        time = $"{i.Time[0].StartTime} {i.Time[0].EndTime}";
                    }
                } 
            }  
            return Ok(time);
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

                log.Debug("SubmitCustomerBooking" +JsonConvert.SerializeObject(customerBooking));

                customerBooking.CustomerId = userDetails.Id;

                customerBooking.CreatedBy = userDetails.Id;

                customerBooking.CreatedOn = DateTime.Now;

                customerBooking.Status = Constant.CUSTOMER_BOOKING_STATUS_BOOKED; 

                customerBooking.PartnerStatus = Constant.PARTNER_BOOKING_STATUS_OPENED;

                customerBooking.CustomerStatus = Constant.CUSTOMER_BOOKING_STATUS_OPENED;

                //Price fixing 

                var totalAmount = customerBooking.TotalPrice;

                var marginMaster =await db.MarginMaster.Where(x => x.Status).FirstOrDefaultAsync();

                var margin = marginMaster.CommissionPer;

                customerBooking.AdminPrice = (totalAmount * (decimal)margin) / 100;

                customerBooking.PartnerPrice = totalAmount - ((totalAmount * (decimal)margin) / 100); 

                db.CustomerBooking.Add(customerBooking);

                await db.SaveChangesAsync();

                log.Debug($"SubmitCustomerBooking" + JsonConvert.SerializeObject(customerBooking.Id));

                return Ok(customerBooking);
            }
            catch(Exception ex)
            {
                log.Error("SubmitCustomerBooking" + JsonConvert.SerializeObject(customerBooking));
                throw ex;
            } 
        }
        #region Customer
        [Route("api/CustomerOpenBookingList")]

        public async Task<IHttpActionResult> CustomerOpenBookingList()
        {
            try
            {


                log.Debug("CustomerOpenBookingList");

                var list = await db.CustomerBooking
                    .Where(x => x.CustomerId == userDetails.Id
                    && x.CustomerStatus == Constant.CUSTOMER_BOOKING_STATUS_OPENED).ToListAsync(); 


                log.Debug($"CustomerOpenBookingList" + JsonConvert.SerializeObject(list));

                return Ok(list.Select(x =>
               new BookingList
               {
                   Id = x.Id,
                   Amount = x.TotalPrice.ToString(),
                   BookingDate = x.DateTime,
                   BookingTime = x.TimeSlot,
                   Location = x.ServiceLocation,
                   Status = "Opened"

               }));
            }
            catch (Exception ex)
            {
                log.Error("CustomerOpenBookingList" + ex.Message);
                throw ex;
            }
        }
        [Route("api/CustomerClosedBookingList")]
        public async Task<IHttpActionResult> CustomerClosedBookingList()
        {
            try
            {


                log.Debug("CustomerClosedBookingList");

                var list = await db.CustomerBooking
                    .Where(x => x.CustomerId == userDetails.Id
                    && x.CustomerStatus == Constant.CUSTOMER_BOOKING_STATUS_CLOSED).ToListAsync();



                log.Debug($"CustomerClosedBookingList" + JsonConvert.SerializeObject(list));

                return Ok(list.Select(x =>
               new BookingList
               {
                   Id = x.Id,
                   Amount = x.TotalPrice.ToString(),
                   BookingDate = x.DateTime,
                   BookingTime = x.TimeSlot,
                   Location = x.ServiceLocation,
                   Status = "Closed"

               }));
            }
            catch (Exception ex)
            {
                log.Error("CustomerClosedBookingList" + ex.Message);
                throw ex;
            }
        }
        [Route("api/CustomerRejectedBookingList")]
        public async Task<IHttpActionResult> CustomerRejectedBookingList()
        {
            try
            {


                log.Debug("CustomerRejectedBookingList");

                var list = await db.CustomerBooking
                    .Where(x => x.CustomerId == userDetails.Id
                     && x.CustomerStatus == Constant.CUSTOMER_BOOKING_STATUS_REJECTED).ToListAsync();



                log.Debug($"CustomerRejectedBookingList" + JsonConvert.SerializeObject(list));

                return Ok(list.Select(x =>
               new BookingList
               {
                   Id = x.Id,
                   Amount = x.TotalPrice.ToString(),
                   BookingDate = x.DateTime,
                   BookingTime = x.TimeSlot,
                   Location = x.ServiceLocation,
                   Status = "Rejected"

               }));
            }
            catch (Exception ex)
            {
                log.Error("CustomerRejectedBookingList" + ex.Message);
                throw ex;
            }
        }

        #endregion


        #region Partner

 
        [Route("api/PartnerOpenBookingList")]
        
        public async Task<IHttpActionResult> PartnerOpenBookingList()
        {
            try
            {
 

                log.Debug("PartnerOpenBookingList");

                var list = await db.CustomerBooking
                    .Where(x => x.ProviderId == userDetails.Id 
                    && x.PartnerStatus == Constant.PARTNER_BOOKING_STATUS_OPENED).ToListAsync();

               

                log.Debug($"PartnerOpenBookingList" + JsonConvert.SerializeObject(list));

                return Ok(list.Select(x =>
               new BookingList
               {
                   Id = x.Id,
                   Amount = x.PartnerPrice.ToString(),
                   BookingDate = x.DateTime,
                   BookingTime = x.TimeSlot, 
                   Location = x.ServiceLocation,
                   Status = "Opened"

               }));
            }
            catch (Exception ex)
            {
                log.Error("PartnerOpenBookingList" + ex.Message);
                throw ex;
            }
        }
        [Route("api/PartnerClosedBookingList")]
        public async Task<IHttpActionResult> PartnerClosedBookingList()
        {
            try
            {


                log.Debug("PartnerClosedBookingList");

                var list = await db.CustomerBooking
                    .Where(x => x.ProviderId == userDetails.Id
                    && x.PartnerStatus == Constant.PARTNER_BOOKING_STATUS_CLOSED).ToListAsync();



                log.Debug($"PartnerClosedBookingList" + JsonConvert.SerializeObject(list));

                return Ok(list.Select(x =>
               new BookingList
               {
                   Id = x.Id,
                   Amount = x.PartnerPrice.ToString(),
                   BookingDate = x.DateTime,
                   BookingTime = x.TimeSlot,
                   Location = x.ServiceLocation,
                   Status = "Closed"

               }));
            }
            catch (Exception ex)
            {
                log.Error("PartnerClosedBookingList" + ex.Message);
                throw ex;
            }
        }
        [Route("api/PartnerRejectedBookingList")]
        public async Task<IHttpActionResult> PartnerRejectedBookingList()
        {
            try
            {


                log.Debug("PartnerRejectedBookingList");

                var list = await db.CustomerBooking
                    .Where(x => x.ProviderId == userDetails.Id
                    && x.PartnerStatus == Constant.PARTNER_BOOKING_STATUS_REJECTED).ToListAsync();



                log.Debug($"PartnerRejectedBookingList" + JsonConvert.SerializeObject(list));

                return Ok(list.Select(x =>
               new BookingList
               {
                   Id = x.Id,
                   Amount = x.PartnerPrice.ToString(),
                   BookingDate = x.DateTime,
                   BookingTime = x.TimeSlot,
                   Location = x.ServiceLocation,
                   Status = "Rejected"

               }));
            }
            catch (Exception ex)
            {
                log.Error("PartnerRejectedBookingList" + ex.Message);
                throw ex;
            }
        }

        #endregion
        public class BookingList
        {
            public int Id { get; set; }
            public DateTime BookingDate { get; set; }
            public string DisplayBookingDate { get { return BookingDate.ToString("dd-MM-yyyy"); } }
            public string BookingTime { get; set; }
            public string Amount { get; set; }
            public string Location { get; set; }
            public string Status { get; set; }
        }
    }
}
