using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using GCBS_INTERNAL.Services;
using GCBS_INTERNAL.ViewModels.GCBSFrontEnd;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GCBS_INTERNAL.Controllers.API.Partner
{
    [CustomAuthorize]
    public class PartnerCommonController : BaseApiController
    {
        public string urlLink = ConfigurationManager.AppSettings["ApiUrl"];
        private readonly DatabaseContext db = new DatabaseContext();
        public ImageServices imgser = new ImageServices();
        [HttpGet]
        [ResponseType(typeof(PartnerCommon))]
        [Route("api/partnerDashboardDetails")]
        public async Task<IHttpActionResult> GetPartnerDashboardDetails()
        {
            var user = await db.UserManagement.FindAsync(userDetails.Id);
            if(user==null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new PartnerCommon
                {
                    DateOfSignUp = user.DateOfSignUp,
                    FirstName = user.FirstName,
                    Image = user.Image,
                    NickName = user.Name,
                    SecondName = user.SecondName
                });
            }   
        }
        [HttpGet]
        [ResponseType(typeof(PartnerPhotoGallery))]
        [Route("api/partnerPhotoGalleryDetails")]
        public async Task<IHttpActionResult> GetPartnerPhotoGallery()
        {
            PartnerPhotoGallery partnerPhotoGallery = new PartnerPhotoGallery();
            var user = await db.UserManagement.FindAsync(userDetails.Id);
            if (user == null)
            {
                return NotFound();
            }   
            var path = imgser.EditGetFiles(userDetails.Id, Constant.SERVICE_PROVIDER_FOLDER_TYPE);
            partnerPhotoGallery.SecondaryImage = path;
            partnerPhotoGallery.PrimaryImage = user.Image;
            return Ok(partnerPhotoGallery);   
        }
        [HttpPut]    
        [Route("api/partnerPhotoGalleryDetails")]
        public async Task<IHttpActionResult> PutPartnerPhotoGallery(PartnerPhotoGallery partnerPhotoGallery)
        {     
            var user = await db.UserManagement.FindAsync(userDetails.Id);
            if (user == null)
            {
                return NotFound();
            }
            user.Image = partnerPhotoGallery.PrimaryImage;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
            if (user.Id > 0 && partnerPhotoGallery.SecondaryImage.Count() > 0)
            {
                imgser.DeleteFiles(user.Id, Constant.SERVICE_PROVIDER_FOLDER_TYPE);
                foreach (var imgbase64 in partnerPhotoGallery.SecondaryImage)
                {
                    imgser.SaveImage(imgbase64, Constant.SERVICE_PROVIDER_FOLDER_TYPE, user.Id, userDetails.Id);
                }
            }
            return Ok(partnerPhotoGallery);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }  
}
