using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using GCBS_INTERNAL.ViewModels.GCBSFrontEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using Newtonsoft.Json;
using log4net;

namespace GCBS_INTERNAL.Controllers.API.Partner
{
    [CustomAuthorize]
    public class PartnerDetailPageController : BaseApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DatabaseContext db = new DatabaseContext();
        [HttpGet]
        [ResponseType(typeof(PartnerDetails))]
        [Route("api/partnerDashboardDetails/{ProviderId}")]
        public async Task<IHttpActionResult> GetPartnerDetailPage(int ProviderId)
        {
            PartnerDetails partnerDetails = new PartnerDetails();
            try
            {
                var userDetails = await db.UserManagement.FindAsync(ProviderId);
                //
                BioInformationMyProfile myProfile = new BioInformationMyProfile()
                {
                    DickSize = userDetails.DickSize,
                    Drinking = userDetails.Drinking,
                    Eyes = userDetails.Eyes,
                    Hair = userDetails.Hair,
                    Height = userDetails.Height,
                    OnlineStatus = userDetails.OnlineStatus,
                    ServiceTypeInCall = userDetails.ServiceTypeInCall,
                    ServiceTypeOutCall = userDetails.ServiceTypeOutCall,
                    SexualOrientation = userDetails.SexualOrientation,
                    Smoking = userDetails.Smoking,
                    Tits = userDetails.Tits,
                    TitType = userDetails.TitType,
                    Weight = userDetails.Weight,
                    Gender = userDetails.Gender
                };
                //
                List<ServiceOffered> serviceOffereds = new List<ServiceOffered>();

                var serviceDurationPrice = db.ServiceDurartionPrice
                    .Include(x=>x.DurationMaster)
                    .Include(x => x.ServicesMaster)
                    .Where(x => x.UserId == ProviderId).ToList();
                foreach(var li in serviceDurationPrice)
                {
                    serviceOffereds.Add(new ServiceOffered
                    {
                        Amount = li.Price,
                        Duration = li.DurationMaster.Duration,
                        Service = li.ServicesMaster.Service
                    });
                }
                ServiceProvider serviceProvider = new ServiceProvider()
                {
                    DateOfSignUp = userDetails.DateOfSignUp,
                    FirstName = userDetails.FirstName,
                    Online = userDetails.OnlineStatus,
                    SecondName = userDetails.SecondName ,
                    Image = userDetails.Image
                };

                List<Root> roots = new List<Root>();
                Availability availability = await db.Availability.Where(x => x.UserId == userDetails.Id).FirstOrDefaultAsync();
                 
                if(availability!=null)
                {
                    if (!String.IsNullOrEmpty(availability.Time))
                    {
                        roots = JsonConvert.DeserializeObject<List<Root>>(availability.Time);
                    }     
                }
               
                partnerDetails.serviceOffereds = serviceOffereds;
                partnerDetails.ServiceAvailability = roots;

                partnerDetails.serviceProviders = serviceProvider;
                partnerDetails.myProfile = myProfile;

                return Ok(partnerDetails);   
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }

        }
        
    }
}
