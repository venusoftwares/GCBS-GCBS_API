using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Available;
using GCBS_INTERNAL.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.Available
{
    [CustomAuthorize]
    public class UnAvailableController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public UnAvailableController()
        {

        }
         
        [HttpGet]
        [Route("api/date/{date}/GetAvailable")]
        public async  Task<IHttpActionResult> GetAvailable(DateTime date)
        {
            try
            {
                var list =await db.UnAvailableDates.Where(x => x.Date == date && x.UserId == userDetails.Id).FirstOrDefaultAsync();
                if(list!=null)
                {
                    return Ok(new ResponseModel { Available = false });
                }
                else
                {
                    return Ok(new ResponseModel { Available = true });
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("api/date/{date}/available/{available}/UpdateAvailable")]
        public async Task<IHttpActionResult> UpdateAvailable(DateTime date,bool available)
        {
            try
            {
                var list = await db.UnAvailableDates.Where(x => x.Date == date && x.UserId == userDetails.Id).FirstOrDefaultAsync();
                //True
                if (!available)
                { 
                    if (list==null)
                    {
                        UnAvailableDates unAvailableDates = new UnAvailableDates();
                        unAvailableDates.Date = date;
                        unAvailableDates.UserId = userDetails.Id;
                        unAvailableDates.CreatedDate = DateTime.Now;
                        db.UnAvailableDates.Add(unAvailableDates);
                        await db.SaveChangesAsync();
                    }
                }
                if (list != null)
                {
                    db.UnAvailableDates.Remove(list);
                    await db.SaveChangesAsync();
                }
                //False and Record here

                return Ok(new ResponseModel { Available = available });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private class ResponseModel
        { 
            public bool Available { get; set; }  
        }
        private class RequestModel
        { 
            public bool Available { get; set; }
        }
    }
}
