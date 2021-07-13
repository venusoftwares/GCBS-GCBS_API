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

namespace GCBS_INTERNAL.Controllers.API.GCBS_FrontEnd
{
    public class HomeAgenciesController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DatabaseContext db = new DatabaseContext();
        public ImageServices imgser = new ImageServices();
        [ResponseType(typeof(AgenciesMasterViewModel))]
        public async Task<IHttpActionResult> GetHomeAgencies()
        {
            try
            {
                log.Info("Called");
                List<AgenciesMasterViewModel> list = new List<AgenciesMasterViewModel>();
                var res = db.AgenciesMaster.Where(x => x.Status).ToList();
                foreach (var agenciesMaster in res)
                {
                    var path = imgser.GetFiles(agenciesMaster.Id, Constant.AGENCIES_FOLDER_TYPE);
                    list.Add(new AgenciesMasterViewModel
                    {
                        AgenciesMaster = agenciesMaster,
                        imageBase64 = path,
                        LocationMasters = await db.LocationMasters.FindAsync(agenciesMaster.Location)

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
        public async Task<IHttpActionResult> GetHomeAgencies(int id)
        {
            try
            {
                log.Info("Called");
                List<AgenciesMasterViewModel> list = new List<AgenciesMasterViewModel>();
                var res = db.AgenciesMaster.Where(x => x.Status && x.Id==id).ToList();
                foreach (var agenciesMaster in res)
                {
                    var path = imgser.GetFiles(agenciesMaster.Id, Constant.AGENCIES_FOLDER_TYPE);
                    list.Add(new AgenciesMasterViewModel
                    {
                        AgenciesMaster = agenciesMaster,
                        imageBase64 = path,
                        LocationMasters = await db.LocationMasters.FindAsync(agenciesMaster.Location)

                    });
                }
                log.Info("End");
                return Ok(list);
            }
            catch (Exception ex)
            {
                log.Error("Sending failed", ex);
                return Content(HttpStatusCode.InternalServerError, "Something went wrong try again");
            }

        }
    }
}
