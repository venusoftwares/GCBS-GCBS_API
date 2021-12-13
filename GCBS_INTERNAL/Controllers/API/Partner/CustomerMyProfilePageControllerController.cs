using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Services;
using log4net;
using System;
using System.Collections.Generic;
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
    public class CustomerMyProfilePageController : BaseApiController
    {
         
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ImageServices imgser = new ImageServices();

        private DatabaseContext db = new DatabaseContext();
        public async Task<IHttpActionResult> GetCustomerMyProfilePage()
        {
            try
            {
                log.Info("Called");
               UserManagementViewModel list = new UserManagementViewModel();
                var res = await db.UserManagement
                      .Include(x => x.CountryMaster)
                       .Include(x => x.StateMaster)
                          .Include(x => x.CityMaster)
                    .Where(x => x.Status && x.RoleId == 9 && x.Id == userDetails.Id)
                    .FirstOrDefaultAsync();
               
                    // First or default 
                    var path = imgser.GetFiles(res.Id, Constant.CUSTOMER_FOLDER_TYPE);


                list.UserManagements = res;
                list.imageBase64 = path;
                list.Age = (DateTime.Now.Year - Convert.ToDateTime(res.DateOfBirth).Year);
                    
                
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
