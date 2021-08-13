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
using System.Data.Entity;
using GCBS_INTERNAL.Provider;

namespace GCBS_INTERNAL.Controllers.API.Partner
{
    [CustomAuthorize]
    public class PartnerMyProfilePageController : BaseApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ImageServices imgser = new ImageServices();

        private DatabaseContext db = new DatabaseContext();
        public async Task<IHttpActionResult> GetPartnerMyProfilePage()
        {
            try
            {
                log.Info("Called");
                List<UserManagementViewModel> list = new List<UserManagementViewModel>();
                var res = await db.UserManagement
                      .Include(x => x.CountryMaster)
                       .Include(x => x.StateMaster)
                          .Include(x => x.CityMaster)
                    .Where(x => x.Status && x.RoleId == 3 && x.Id == userDetails.Id)
                    .ToArrayAsync();
                foreach (var a in res)
                {
                    // First or default 
                    var path = imgser.GetFiles(a.Id, Constant.SERVICE_PROVIDER_FOLDER_TYPE);

                    list.Add(new UserManagementViewModel
                    {
                        UserManagements = a,
                        imageBase64 = path,
                        Age = (DateTime.Now.Year - Convert.ToDateTime(a.DateOfBirth).Year)
                    });
                }
                log.Info("End");
                return Ok(list);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }

        }
    }
}
