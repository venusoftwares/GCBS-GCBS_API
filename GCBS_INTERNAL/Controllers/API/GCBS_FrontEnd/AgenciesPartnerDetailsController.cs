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

namespace GCBS_INTERNAL.Controllers.API.GCBS_FrontEnd
{
    public class AgenciesPartnerDetailsController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ImageServices imgser = new ImageServices();

        private DatabaseContext db = new DatabaseContext();
        [HttpGet]
        [Route("api/agenciesPartnerDetails/{id}")]
        public async Task<IHttpActionResult> AgenciesPartnerDetails(int id)
        {
            try
            { 
                log.Info("Called");

                List<int> userIds = new List<int>();

                var partnerListDetails = await db.UserManagement.Where(x => x.Agencies != null && x.RoleId == 3 && x.Status ==true).ToListAsync();

                foreach(var a in partnerListDetails)
                {
                    if(!string.IsNullOrEmpty(a.Agencies))
                    {
                        var agenciesSplit = a.Agencies.Split('|');
                        int[] ints = Array.ConvertAll(agenciesSplit, s => int.Parse(s));
                        if (ints.Contains(id))
                        {
                            userIds.Add(a.Id);
                        }
                    }
               
                } 

                List<UserManagementViewModel> list = new List<UserManagementViewModel>();
                var res = await db.UserManagement
                    .Include(x => x.CountryMaster)
                       .Include(x => x.StateMaster)
                          .Include(x => x.CityMaster)
                    .Where(x => x.Status && x.RoleId == 3 && userIds.Contains(x.Id))
                    .ToArrayAsync();
                foreach (var a in res)
                {
                    List<string> img = new List<string>();
                    img.Add(a.Image);
                    list.Add(new UserManagementViewModel
                    {
                        UserManagements = a,
                        imageBase64 = img,
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
    }
}
