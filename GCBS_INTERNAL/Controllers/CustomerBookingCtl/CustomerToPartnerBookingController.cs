using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Booking;
using GCBS_INTERNAL.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.CustomerBookingCtl
{
    [CustomAuthorize]

    public class CustomerToPartnerBookingController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();


        [HttpGet]
        [Route("api/partnerId/{partnerId}/GetBookingListOgTimesView")]
        public async Task<IHttpActionResult> GetBookingListOgTimesView(int partnerId)
        {
            try
            {
                BookingResponse bookingResponse = new BookingResponse();

                var basePrices = await db.PartnerBasePrice
                    .Include(x => x.DurationAndBasePrice)
                    .Where(x => x.UserId == partnerId && x.Status)
                    .ToListAsync();

                bookingResponse.BasePrices = basePrices.Select(x => new BasePrices
                {
                    Id = x.id,
                    BasePrice = x.BasePrice,
                    Minutes = x.DurationAndBasePrice.Minutes,
                    Time = x.DurationAndBasePrice.DurationOrTime,
                    TimeDurationId = x.DurationId

                }).ToList();

                var additionalPrice = await db.PartnerAdditionalPrice.Where(x => x.UserId == partnerId && x.Status).ToListAsync();

                bookingResponse.AdditionalPrices = additionalPrice.Select(x => new AdditionalPrices
                {
                    Id = x.id,
                    AdditionalPrice = x.AdditionalPrice,
                    ServiceType = x.ServiceType

                }).ToList();

                bookingResponse.CalenderDetails = new List<CalenderDetails>();
                //await GetCalenderDetailsAsync(partnerId, bookingResponse.BasePrices[0].TimeDurationId);

                return Ok(bookingResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("api/partnerId/{partnerId}/GetBookingTotal")]
        public async Task<IHttpActionResult> GetBookingTotal(int partnerId, BookingTotalRequest bookingTotalRequest)
        {
            try
            {
                var basePrices = await db.PartnerBasePrice
                   .Where(x => x.UserId == partnerId && x.id == bookingTotalRequest.TimeDurationId)
                   .Select(x => x.BasePrice).DefaultIfEmpty(0).SumAsync();

                var additionalPrice = await db.PartnerAdditionalPrice
                  .Where(x => x.UserId == partnerId && bookingTotalRequest.ServiceTypeIDs.Contains(x.id))
                  .Select(x => x.AdditionalPrice).DefaultIfEmpty(0).SumAsync();

                return Ok(new BookingTotalResponse { BasePrice = basePrices, AdditionalPrice = additionalPrice, Total = basePrices + additionalPrice });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("api/partnerId/{partnerId}/durationId/{durationId}/GetCalenderViewFromDuration")]
        public async Task<IHttpActionResult> GetBookingTotal(int partnerId, int durationId)
        {
            try
            {

                return Ok(await GetCalenderDetailsAsync(partnerId, durationId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("api/partnerId/{partnerId}/SubmitCustomerBookingDetails")]
        public async Task<IHttpActionResult> SubmitCustomerBookingDetails(int partnerId, BookingRequest bookingRequest)
        {
            try
            {
                var basePrices = await db.PartnerBasePrice
                   .Where(x => x.UserId == partnerId && x.id == bookingRequest.TimeDurationId)
                   .Select(x => x.BasePrice).DefaultIfEmpty(0).SumAsync();

                var additionalPrice = await db.PartnerAdditionalPrice
                  .Where(x => x.UserId == partnerId && bookingRequest.ServiceTypeIDs.Contains(x.id))
                  .Select(x => x.AdditionalPrice).DefaultIfEmpty(0).SumAsync();


                decimal Total = basePrices + additionalPrice;

                log.Debug("SubmitCustomerBookingDetails" + JsonConvert.SerializeObject(bookingRequest));

                var duration = await db.PartnerBasePrice
                    .Include(x => x.DurationAndBasePrice)
                   .Where(x => x.UserId == partnerId)
                   .Select(x => x.DurationAndBasePrice.DurationOrTime).FirstOrDefaultAsync();




                if (basePrices > 0)
                {
                    JsonReponse jsonReponse = new JsonReponse()
                    {
                        AdditionalPrice = GetAdditionalPrices(bookingRequest.ServiceTypeIDs),
                        basePrice = GetBasePrice(bookingRequest.TimeDurationId),
                        Date = bookingRequest.date.ToString("dd-MM-yyyy"),
                        TimeSlot = bookingRequest.TimeSlot,
                        TotalPrices = new BookingTotalResponse { AdditionalPrice = additionalPrice, BasePrice = basePrices, Total = Total }
                    };
                    CustomerBooking customerBooking = new CustomerBooking()
                    {

                        //ProviderId = partnerId,
                        //AdditionalPrice = additionalPrice,
                        //BasePrice = basePrices,
                        //CreatedBy = userDetails.Id,
                        //PartnerPrice = 0,
                        //PartnerStatus = 0,
                        //TimeSlot = bookingRequest.TimeSlot,
                        //TotalPrice = Total,
                        //Status = Constant.CUSTOMER_BOOKING_STATUS_BOOKED,
                        //CustomerStatus = Constant.CUSTOMER_BOOKING_STATUS_BOOKED,
                        //CustomerId = userDetails.Id,
                        //CreatedOn = DateTime.Now,
                        //DateTime = bookingRequest.date,
                        //Durarion = duration,
                        //Json = JsonConvert.SerializeObject(jsonReponse),

                        ProviderId = partnerId,
                        AdditionalPrice = additionalPrice,
                        BasePrice = basePrices,
                        CreatedBy = userDetails.Id,
                        PartnerPrice = basePrices + additionalPrice,
                        PartnerStatus = Constant.CUSTOMER_BOOKING_STATUS_OPENED,
                        TimeSlot = bookingRequest.TimeSlot,
                        TotalPrice = Total,
                        Status = Constant.CUSTOMER_BOOKING_STATUS_OPENED,
                        CustomerStatus = Constant.CUSTOMER_BOOKING_STATUS_OPENED,
                        CustomerId = userDetails.Id,
                        CreatedOn = DateTime.Now,
                        DateTime = bookingRequest.date,
                        Durarion = duration,
                        Json = JsonConvert.SerializeObject(jsonReponse),


                    };


                    db.CustomerBooking.Add(customerBooking);

                    await db.SaveChangesAsync();
                    log.Debug($"SubmitCustomerBooking" + JsonConvert.SerializeObject(customerBooking.Id));


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(bookingRequest);
        }

        private BasePrice GetBasePrice(int timeDurationId)
        {
            var list = db.PartnerBasePrice.Include(x => x.DurationAndBasePrice).Where(x => x.id == timeDurationId).FirstOrDefault();

            return new BasePrice { Price = list.BasePrice, Time = list.DurationAndBasePrice.DurationOrTime };
        }

        private List<AdditionPrice> GetAdditionalPrices(List<int> serviceTypeIDs)
        {
            List<AdditionPrice> additionPrices = new List<AdditionPrice>();

            var list = db.PartnerAdditionalPrice.Where(x => serviceTypeIDs.Contains(x.id));

            foreach (var a in list)
            {
                additionPrices.Add(new AdditionPrice { Price = a.AdditionalPrice, Type = a.ServiceType });
            }

            return additionPrices;

        }


        private async Task<List<CalenderDetails>> GetCalenderDetailsAsync(int partnerId, int timeDuraionId = 0)
        {
            try
            {
                List<CalenderDetails> calenderDetails = new List<CalenderDetails>();

                List<DateTime> dateTimes = new List<DateTime>();

                DateTime currentDate = DateTime.Now;

                DateTime StartDate = DateTime.Now.Date;

                DateTime EndDate = StartDate.AddMonths(3);

                while (StartDate <= EndDate)
                {

                    dateTimes.Add(StartDate);

                    StartDate = StartDate.AddDays(1);

                }

                decimal minutes = 0;

                var partner = await db.PartnerBasePrice.FindAsync(timeDuraionId);

                var duration = await db.DurationAndBasePrice.FindAsync(partner.DurationId);

                if (duration != null)
                {
                    minutes = (duration.Hour * 60) + duration.Minutes;
                }



                var unavailableDate = db.UnAvailableDates.Where(x => x.UserId == partnerId).ToList();

                var availbaleDate = await GetAvailableDateAsync(partnerId);

                var date2 = DateTime.Now.Date;

                var customerBookingDetails = db.CustomerBooking.Where(x => x.ProviderId == partnerId && x.DateTime >= date2).ToList();

                foreach (var date in dateTimes)
                {

                    var dayOfWeek = date.Date.DayOfWeek.ToString().ToUpper();

                    var available = availbaleDate.Times.Where(x => x.Day.ToUpper() == dayOfWeek && x.Available).ToList();

                    if (!unavailableDate.Any(x => x.Date == date))
                    {

                        foreach (var list in available)
                        {

                            foreach (var time in list.Time)
                            {

                                DateTime startDate = Convert.ToDateTime($"{date.ToString("yyyy-MM-dd")} {time.StartTime}");

                                DateTime endDate = Convert.ToDateTime($"{date.ToString("yyyy-MM-dd")} {time.EndTime}");

                                double min = (endDate - startDate).TotalMinutes;

                                string timeSlot = $"{startDate.ToString("hh:mm tt")} To {endDate.ToString("hh:mm tt")}";

                                bool cj = customerBookingDetails.Any(x => x.DateTime == date && x.TimeSlot == timeSlot);

                                bool check = customerBookingDetails.Any(x => x.DateTime == date && x.TimeSlot == timeSlot && x.PartnerStatus == Constant.PARTNER_BOOKING_STATUS_OPENED);

                                if (!check)
                                {
                                    if (((minutes <= (decimal)min) || duration != null) && currentDate < startDate)
                                    {
                                        if (!calenderDetails.Any(x => x.Date == date && x.Title == timeSlot && x.Start == startDate && x.End == endDate))
                                        {
                                            if (minutes <= (decimal)min)
                                            {
                                                DateTime STime = startDate;
                                                DateTime ETime = startDate;
                                                var div = (int)min / (int)minutes;
                                                for (int i = 0; i < div; i++)
                                                {
                                                    if (ETime <= endDate)
                                                    {
                                                        ETime = ETime.AddMinutes((int)minutes);
                                                        string timeSlot2 = $"{STime.ToString("hh:mm tt")} To {ETime.ToString("hh:mm tt")}";
                                                        bool check2 = customerBookingDetails.Any(x => x.DateTime == date && x.TimeSlot == timeSlot2 && x.PartnerStatus == Constant.PARTNER_BOOKING_STATUS_OPENED);
                                                        if (!check2)
                                                        {
                                                            calenderDetails.Add(new CalenderDetails { Date = date, cssClass = "", Title = timeSlot2, Start = STime, End = ETime });
                                                        }
                                                        ETime = ETime.AddMinutes(Constant.BOOKING_TIME_INTERVEL);
                                                        STime = ETime;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }

                    }
                }

                return calenderDetails.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<Root> RooleeRoot()
        {
            return new List<Root>
            {
                new Root { Day = "Sunday", Time = new List<Time>() { new Time { StartTime = "09:00 AM", EndTime = "10:00 PM", Minutes = 540 } } },
                new Root { Day = "Monday", Time = new List<Time>() { new Time { StartTime = "09:00 AM", EndTime = "10:00 PM", Minutes = 540 } } },
                new Root { Day = "Tuesday", Time = new List<Time>() { new Time { StartTime = "09:00 AM", EndTime = "10:00 PM", Minutes = 540 } } },
                new Root { Day = "Wednesday", Time = new List<Time>() { new Time { StartTime = "09:00 AM", EndTime = "10:00 PM", Minutes = 540 } } },
                new Root { Day = "Thursday", Time = new List<Time>() { new Time { StartTime = "09:00 AM", EndTime = "10:00 PM", Minutes = 540 } } },
                new Root { Day = "Friday", Time = new List<Time>() { new Time { StartTime = "09:00 AM", EndTime = "10:00 PM", Minutes = 540 } } },
                new Root { Day = "Saturday", Time = new List<Time>() { new Time { StartTime = "09:00 AM", EndTime = "10:00 PM", Minutes = 540 } } }
            };
        }
        private async Task<RootAvailability> GetAvailableDateAsync(int id)
        {

            RootAvailability availability2 = new RootAvailability();

            Availability availability = await db.Availability.Where(x => x.UserId == id).FirstOrDefaultAsync();

            if (availability == null)
            {

                DateTime now = DateTime.Now;

                availability2.Availability = new Availability { Id = 0, UserId = 0 };

                availability2.Times = RooleeRoot();

                return availability2;

            }

            availability2.Availability = availability;

            if (!String.IsNullOrEmpty(availability.Time))
            {

                availability2.Times = JsonConvert.DeserializeObject<List<Root>>(availability.Time);

            }
            else
            {
                availability2.Times = RooleeRoot();

            }
            return availability2;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private class BookingResponse
        {
            public List<BasePrices> BasePrices { get; set; }
            public List<AdditionalPrices> AdditionalPrices { get; set; }
            public List<CalenderDetails> CalenderDetails { get; set; }

        }

        private class BasePrices
        {
            public int Id { get; set; }
            public string Time { get; set; }

            public decimal BasePrice { get; set; }

            public decimal Minutes { get; set; }

            public int TimeDurationId { get; set; }

        }

        private class AdditionalPrices
        {
            public int Id { get; set; }
            public string ServiceType { get; set; }
            public decimal AdditionalPrice { get; set; }

        }

        private class CalenderDetails
        {
            public DateTime Date { get; set; }

            [JsonProperty("start")]
            public DateTime Start { get; set; }
            [JsonProperty("end")]
            public DateTime End { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("cssClass")]
            public string cssClass { get; set; }
        }

        public class BookingTotalRequest
        {
            public int TimeDurationId { get; set; }
            public List<int> ServiceTypeIDs { get; set; }
        }

        public class BookingTotalResponse
        {
            public decimal BasePrice { get; set; }
            public decimal AdditionalPrice { get; set; }
            public decimal Total { get; set; }
        }
        public class BookingRequest
        {
            [Required]
            public int TimeDurationId { get; set; }
            public List<int> ServiceTypeIDs { get; set; }
            [Required]
            public DateTime date { get; set; }
            public string TimeSlot { get; set; }
        }

        public class AdditionPrice
        {
            public string Type { get; set; }
            public decimal Price { get; set; }

        }
        public class BasePrice
        {
            public string Time { get; set; }
            public decimal Price { get; set; }
        }

        public class JsonReponse
        {
            public string Date { get; set; }
            public string TimeSlot { get; set; }
            public BasePrice basePrice { get; set; }
            public List<AdditionPrice> AdditionalPrice { get; set; }
            public BookingTotalResponse TotalPrices { get; set; }
        }
    }
}
