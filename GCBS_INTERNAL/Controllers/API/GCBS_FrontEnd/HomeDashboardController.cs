using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.ViewModels.GCBSFrontEnd;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GCBS_INTERNAL.Controllers.API.GCBS_FrontEnd
{
    public class HomeDashboardController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DatabaseContext db = new DatabaseContext();
        [ResponseType(typeof(HomeDashboardViewModels))]
        public IHttpActionResult GetHomeDashboard()
        {
            try
            {
                log.Info("Called");
                HomeDashboardViewModels homeDashboardViewModels = new HomeDashboardViewModels();
                homeDashboardViewModels.homePageContent = db.HomePageContent.Where(x => x.Id == 1)
                    .Select(x => new HomePageContents
                    {
                        Title = x.Title,
                        Image = x.Image,
                        Body = x.Body
                    }).FirstOrDefault();
                homeDashboardViewModels.AboutUs = db.AboutUsContent.Where(x => x.Id == 1)
                    .Select(x => new AboutUs
                    {
                        Title = x.Title,
                        Image = x.Image,
                        Body = x.Body
                    }).FirstOrDefault();
                homeDashboardViewModels.Disclaimer = db.DisclaimerContent.Where(x => x.Id == 1)
                    .Select(x => new Disclaimer
                    {
                        Title = x.Title,
                        Body = x.Body
                    }).FirstOrDefault();
                homeDashboardViewModels.TermsAndCondition = db.TermsAndConditions.Where(x => x.Id == 1)
                    .Select(x => new TermsAndCondition
                    {
                        Title = x.Title,
                        Body = x.Body
                    }).FirstOrDefault();
                homeDashboardViewModels.WarningEighteenPlus = db.Warning18Content.Where(x => x.Id == 1)
                    .Select(x => new WarningEighteenPlus
                    {
                        Title = x.Title,
                        Body = x.Body
                    }).FirstOrDefault();
                homeDashboardViewModels.PrivacyPolicy = db.PrivacyAndPolicy.Where(x => x.Id == 1)
                    .Select(x => new PrivacyPolicy
                    {
                        Title = x.Title,
                        Body = x.Body
                    }).FirstOrDefault();
                homeDashboardViewModels.BookingTerm = db.BookingTerms.Where(x => x.Id == 1)
                    .Select(x => new BookingTermss
                    {
                        Title = x.Title,
                        Body = x.Body
                    }).FirstOrDefault();
                homeDashboardViewModels.RefundTerm = db.RefundTerms.Where(x => x.Id == 1)
                   .Select(x => new RefundTerms
                   {
                       Title = x.Title,
                       Body = x.Body
                   }).FirstOrDefault();

                homeDashboardViewModels.ContactUs = db.SiteSettings.Select(x => new ContactUs
                {
                    Address = x.Address1 + " " + x.Address2,
                    Email = x.Email,
                    PhoneNo = x.Phone
                }).FirstOrDefault();
                log.Info("End");
                return Ok(homeDashboardViewModels);
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }
        }
    }
}
