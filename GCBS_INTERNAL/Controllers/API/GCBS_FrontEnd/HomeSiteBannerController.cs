using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Services;
using GCBS_INTERNAL.ViewModels.GCBSFrontEnd;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GCBS_INTERNAL.Controllers.API.GCBS_FrontEnd
{
    public class HomeSiteBannerController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ImageServices imgser = new ImageServices();

        private DatabaseContext db = new DatabaseContext();
        [ResponseType(typeof(List<SiteBannerViewIndex>))]
        public async Task<IHttpActionResult> GetHomeSiteBanner()
        {
            try
            {
                List<SiteBannerViewIndex> list = new List<SiteBannerViewIndex>();
                var res =await db.SiteBannerMasters.Where(x=>x.Status==true).ToListAsync();
                foreach (var a in res)
                {
                    string path = imgser.GetFiles(a.Id, Constant.SITE_BANNER_FOLDER_TYPE).FirstOrDefault();
                    list.Add(new SiteBannerViewIndex
                    {
                        Id = a.Id,
                        Image = path,
                        MainHeading = a.MainHeading,
                        MainTitle = a.MainTitle,
                        Status = a.Status
                    });
                }
                log.Info("End");
                return Ok(list);
            }
            catch(Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }
        }
    }
}
