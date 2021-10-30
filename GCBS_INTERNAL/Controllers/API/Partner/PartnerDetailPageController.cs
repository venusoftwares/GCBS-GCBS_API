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

                BioInformationMyProfile2 myProfile2 = new BioInformationMyProfile2()
                {
                    DickSize = db.DickSize.Where(x=>x.Id== userDetails.DickSize).Select(x=>x.DickSize1).FirstOrDefault(),
                    Drinking = userDetails.Drinking == true ? "Yes" : "No",
                    Eyes = db.Eye.Where(x => x.Id == userDetails.Eyes).Select(x => x.Eye1).FirstOrDefault(),
                    Hair = db.Hair.Where(x => x.Id == userDetails.Hair).Select(x => x.Hair1).FirstOrDefault(),
                    Height = db.Height.Where(x => x.Id == userDetails.Height).Select(x => x.Height1).FirstOrDefault(), 
                    SexualOrientation = db.Orientation.Where(x => x.Id == userDetails.SexualOrientation).Select(x => x.Orientation1).FirstOrDefault(),
                    Smoking = userDetails.Smoking == true ? "Yes" : "No",
                    Tits = db.Tit.Where(x => x.Id == userDetails.Tits).Select(x => x.Tit1).FirstOrDefault(),
                    TitType = db.TitType.Where(x => x.Id == userDetails.TitType).Select(x => x.TitType1).FirstOrDefault(),
                    Weight = db.Weight.Where(x => x.Id == userDetails.Weight).Select(x => x.Weight1).FirstOrDefault(),
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
                        var list = JsonConvert.DeserializeObject<List<Root>>(availability.Time);
                        roots = list.Where(x => x.Available).ToList();
                    }     
                }
               
                partnerDetails.serviceOffereds = serviceOffereds;
                partnerDetails.ServiceAvailability = roots;

                partnerDetails.serviceProviders = serviceProvider;
                partnerDetails.myProfile = myProfile;
                partnerDetails.myProfile2 = myProfile2;

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
