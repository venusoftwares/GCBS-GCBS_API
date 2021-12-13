using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.Calender
{
    public class CalenderViewDashboardController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        [CustomAuthorize]

        public async Task<IHttpActionResult> GetProviderCalenderView()
        {
            try
            {
                List<CalenderDetails> calenderDetails = new List<CalenderDetails>();

                List<DateTime> dateTimes = new List<DateTime>();

                DateTime StartDate = DateTime.Now.Date;

                DateTime currentDate = DateTime.Now.Date;

                DateTime EndDate = StartDate.AddMonths(3);

                while (StartDate <= EndDate)
                {
                    dateTimes.Add(StartDate);
                    StartDate = StartDate.AddDays(1);
                }

                var bookingDetails =await db.CustomerBooking.Where(x => x.ProviderId == userDetails.Id && x.DateTime >= currentDate).ToListAsync(); 

                foreach (var date in dateTimes)
                {
                    var list = bookingDetails.Where(x => x.DateTime == date).ToList();

                    if(list.Count() > 0)
                    {
                        foreach(var i in list)
                        {
                            string[] split = i.TimeSlot.Split(' ');
                            DateTime startDate = Convert.ToDateTime($"{date.ToString("yyyy-MM-dd")} {split[0]} {split[1]}");
                            DateTime endDate = Convert.ToDateTime($"{date.ToString("yyyy-MM-dd")} {split[3]} {split[4]}");
                            calenderDetails.Add(new CalenderDetails { Date = i.DateTime, cssClass = "", Title = $"{i.TimeSlot}",Start=startDate,End=endDate });
                        } 
                    }
                     
                }

                return Ok(calenderDetails);

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

            Availability availability = await db.Availability.Where(x => x.UserId == userDetails.Id).FirstOrDefaultAsync();

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

        public class CalenderDetails
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
    }
}


