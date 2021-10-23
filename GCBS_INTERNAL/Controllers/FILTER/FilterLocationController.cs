using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.FILTER
{
    public class FilterLocationController : ApiController
    {
        public async Task<IHttpActionResult> GetLocation(double fromlat, double fromlon,double tolat,double tolon)
        {
            //var sCoord = new GeoCoordinate(11.1205, 77.3322);

            //var eCoord = new GeoCoordinate(11.103830, 77.391910);
            var sCoord = new GeoCoordinate(fromlat, fromlon);

            var eCoord = new GeoCoordinate(tolat, tolon);

            var result = sCoord.GetDistanceTo(eCoord)/1000;

            return Ok(result);
        }
    }
}
