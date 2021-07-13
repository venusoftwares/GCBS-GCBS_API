using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GCBS_INTERNAL.Controllers.API.GCBS_FrontEnd
{
    public class HomeHotelController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DatabaseContext db = new DatabaseContext();
        public ImageServices imgser = new ImageServices();
        [ResponseType(typeof(HotelMasterViewModel))]
        public async Task<IHttpActionResult> GetHomeHotel()
        {
            try
            {
                log.Info("Called");
                List<HotelMasterViewModel> list = new List<HotelMasterViewModel>();
                var res = db.HotelMaster.Where(x => x.Status).ToList();
                foreach (var hotelMaster in res)
                {
                    var path = imgser.GetFiles(hotelMaster.Id, Constant.HOTEL_FOLDER_TYPE);
                    list.Add(new HotelMasterViewModel
                    {
                        HotelMaster = hotelMaster,
                        imageBase64 = path,
                        LocationMasters = await db.LocationMasters.FindAsync(hotelMaster.Location)     
                    });
                }
                log.Info("End");
                return Ok(list);
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }   
        }
        [ResponseType(typeof(HotelMasterViewModel))]
        public async Task<IHttpActionResult> GetHomeHotel(int id)
        {
            try
            {
                log.Info("Called");
                List<HotelMasterViewModel> list = new List<HotelMasterViewModel>();
                var res = db.HotelMaster.Where(x => x.Status && x.Id == id).ToList();
                foreach (var hotelMaster in res)
                {
                    var path = imgser.GetFiles(hotelMaster.Id, Constant.HOTEL_FOLDER_TYPE);
                    list.Add(new HotelMasterViewModel
                    {
                        HotelMaster = hotelMaster,
                        imageBase64 = path,
                        LocationMasters = await db.LocationMasters.FindAsync(hotelMaster.Location)
                    });
                }
                log.Info("End");
                return Ok(list);
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }
        }
    }
}
