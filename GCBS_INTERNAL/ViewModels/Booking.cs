using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.ViewModels
{
    public class Booking
    {
        //Input
        public string ServiceLocation { get; set; }

        //Input
        public string ServiceArea { get; set; }

        //Input
        public string HouseOrHotel { get; set; }

        //Input
        public DateTime DateTime { get; set; }

        //Drop Down To Get Input with token
        public string Durarion { get; set; }

        //Drop Down TimeSlot from DateTime with token
        public string TimeSlot { get; set; } 

        //Base Price From Database
        public decimal? BasePrice { get; set; }

        //Service Type From Drop Down
        public string ServiceType { get; set; }

        //Service Type From Previous setup to get value
        public decimal ServicePrice { get; set; }  

        //Base price + service Price
        public decimal TotalPrice { get; set; }

        public string Notes { get; set; }

        public int ProviderId { get; set; }
    }
}