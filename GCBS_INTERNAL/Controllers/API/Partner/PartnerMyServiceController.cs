using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using log4net;
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API.Partner
{
    [CustomAuthorize]
    public class PartnerMyServiceController : BaseApiController
    {
        private readonly DatabaseContext db = new DatabaseContext();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Route("api/DropDownPartnerServicesMaster")]
        public async Task<IHttpActionResult> GetServices()
        {
            try
            {
                log.Info("Called");
                List<DropDownCommon> dropDownCommons = new List<DropDownCommon>();
                var serviceMaster = await db.ServicesMasters.Where(x => x.Visible).Select(x => new DropDownCommon { Key = x.Id, Value = x.Service }).ToListAsync();
                var duration = await db.DurationMaster.Where(x => x.Status).ToListAsync();
                var serviceDuration = await db.ServiceDurartionPrice.Where(x => x.UserId == userDetails.Id).ToListAsync();
                foreach (var service in serviceMaster)
                {
                    foreach (var dur in duration)
                    {
                        if (!serviceDuration.Any(x => x.ServiceId == service.Key && x.DurationId == dur.Id))
                        {
                            dropDownCommons.Add(service);
                        }
                    }
                }
                log.Info("End");
                return Ok(dropDownCommons.Distinct());
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("api/DropDownPartnerDuration/{ServiceId}")]
        public async Task<IHttpActionResult> GetDuration(int ServiceId)
        {
            try
            {
                log.Info("Called");
                List<DropDownCommon> dropDownCommons = new List<DropDownCommon>();
                var duration = await db.DurationMaster.Where(x => x.Status).Select(x => new DropDownCommon { Key = x.Id, Value = x.Duration }).ToListAsync();
                var serviceDuration = await db.ServiceDurartionPrice.Where(x => x.UserId == userDetails.Id && x.ServiceId == ServiceId).ToListAsync();
                foreach (var dur in duration)
                {
                    if (!serviceDuration.Any(x => x.DurationId == dur.Key))
                    {
                        dropDownCommons.Add(dur);
                    }
                }
                log.Info("End");
                return Ok(dropDownCommons.Distinct());
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/PartnerMyService")]
        public async Task<IHttpActionResult> GetPartnerMyService()
        {
            try
            {
                log.Info("Called");
                var result = await db.ServiceDurartionPrice.Where(X => X.UserId == userDetails.Id).Include(x => x.DurationMaster).Include(x => x.ServicesMaster).ToListAsync();
                log.Info("End");
                return Ok(result);
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/PartnerMyService")]
        public async Task<IHttpActionResult> GetPartnerMyService(ServiceDurartionPrice serviceDurartionPrice)
        {
            try
            {
                log.Info("Called");
                using (var d = new DatabaseContext())
                {
                    if (!d.ServiceDurartionPrice.Any(x => x.ServiceId == serviceDurartionPrice.ServiceId && x.DurationId == serviceDurartionPrice.DurationId && x.UserId == userDetails.Id))
                    {
                        serviceDurartionPrice.CreatedBy = userDetails.Id;
                        serviceDurartionPrice.CreatedOn = DateTime.Now;
                        serviceDurartionPrice.UserId = userDetails.Id;
                        db.ServiceDurartionPrice.Add(serviceDurartionPrice);
                        await db.SaveChangesAsync();
                    }
                    d.Dispose();
                }
                log.Info("End");
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        [Route("api/PartnerMyService")]
        public async Task<IHttpActionResult> PutPartnerMyService(ServiceDurationPriceVisible serviceDurationPriceVisible)
        {
            try
            {
                ServiceDurartionPrice serviceDurartionPrice = new ServiceDurartionPrice();
                log.Info("Called");
                using (var d = new DatabaseContext())
                {
                    var re = await d.ServiceDurartionPrice.FindAsync(serviceDurationPriceVisible.Id);
                    serviceDurartionPrice = re;
                    d.Dispose();
                }
                serviceDurartionPrice.Price = serviceDurationPriceVisible.Price;
                serviceDurartionPrice.Status = serviceDurationPriceVisible.Status;
                serviceDurartionPrice.UpdatedBy = userDetails.Id;
                serviceDurartionPrice.UpdatedOn = DateTime.Now;
                if (userDetails.Id == serviceDurartionPrice.UserId)
                {
                    db.Entry(serviceDurartionPrice).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                log.Info("End");
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
