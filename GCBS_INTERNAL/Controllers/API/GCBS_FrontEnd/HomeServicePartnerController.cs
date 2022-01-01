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
using System.Web.Http.Description;
using System.Data.Entity;

namespace GCBS_INTERNAL.Controllers.API.GCBS_FrontEnd
{
    public class HomeServicePartnerController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ImageServices imgser = new ImageServices();

        private DatabaseContext db = new DatabaseContext();
        public async Task<IHttpActionResult> GetHomeServicePartner()
        {
            try
            {
                log.Info("Called");
                List<UserManagementViewModel> list = new List<UserManagementViewModel>();
                var res =await db.UserManagement
                    .Include(x=>x.CountryMaster)
                       .Include(x => x.StateMaster)
                          .Include(x => x.CityMaster)
                    .Where(x=>x.RoleId==3 && x.AccessStatus == Constant.ACCESS_STATUS_ACTIVE)
                    .ToArrayAsync();
                foreach (var a in res)
                {
                    // First or default 
                    var path = imgser.GetFiles(a.Id, Constant.SERVICE_PROVIDER_FOLDER_TYPE);  
                    list.Add(new UserManagementViewModel
                    {
                         UserManagements  =a,
                         imageBase64 = path  ,
                         Age = (DateTime.Now.Year - Convert.ToDateTime(a.DateOfBirth).Year),  
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
        public async Task<IHttpActionResult> GetHomeServicePartner(int id)
        {
            try
            {
                log.Info("Called");
                List<UserManagementViewModel> list = new List<UserManagementViewModel>();
                var res = await db.UserManagement
                      .Include(x => x.CountryMaster)
                       .Include(x => x.StateMaster)
                          .Include(x => x.CityMaster)
                    .Where(x=> x.RoleId == 3 && x.Id == id && x.AccessStatus == Constant.ACCESS_STATUS_ACTIVE)   
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
        [Route("api/HomeServicePartner/{limit}/{skip}")]
        public async Task<IHttpActionResult> GetHomeServicePartner(int limit,int skip)
        {
            try
            {
                log.Info("Called");
                List<UserManagementViewModel> list = new List<UserManagementViewModel>();
                var res = await db.UserManagement
                    .Include(x => x.CountryMaster)
                       .Include(x => x.StateMaster)
                          .Include(x => x.CityMaster)
                    .Where(x => x.RoleId == 3 && x.AccessStatus == Constant.ACCESS_STATUS_ACTIVE)    
                    .OrderBy(x=>x.Id)
                    .Skip(skip)
                    .Take(limit)
                    .ToListAsync();
                foreach (var a in res)
                {
                    List<string> img = new List<string>();
                    img.Add(a.Image);
                    // First or default 
                    //var path = imgser.GetFiles(a.Id, Constant.SERVICE_PROVIDER_FOLDER_TYPE);  
                    list.Add(new UserManagementViewModel
                    {
                        UserManagements = a,
                        imageBase64 = img,
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
