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

    public class CalenderViewController : BaseApiController
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

                DateTime EndDate = StartDate.AddMonths(3);

                while(StartDate <= EndDate)
                {
                    dateTimes.Add(StartDate);
                    StartDate = StartDate.AddDays(1);
                }

                var unavailableDate = db.UnAvailableDates.Where(x => x.UserId == userDetails.Id).ToList();

                var availbaleDate =await GetAvailableDateAsync(userDetails.Id);

                foreach(var date in dateTimes)
                {

                    var dayOfWeek = date.Date.DayOfWeek.ToString().ToUpper();

                    var available = availbaleDate.Times.Where(x => x.Day.ToUpper() == dayOfWeek && x.Available).ToList();

                    if(!unavailableDate.Any(x=>x.Date == date))
                    {
                        foreach (var list in available)
                        {
                            foreach(var time in list.Time)
                            {
                                DateTime startDate = Convert.ToDateTime($"{date.ToString("yyyy-MM-dd")} {time.StartTime}");
                                DateTime endDate = Convert.ToDateTime($"{date.ToString("yyyy-MM-dd")} {time.EndTime}");

                                calenderDetails.Add(new CalenderDetails { Date = date, cssClass = "", Title = $"{time.StartTime} To {time.EndTime}", Start = startDate,End = endDate });

                            }
                           
                        }
                    }
                }

                return Ok(calenderDetails);

            }
            catch(Exception ex)
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
