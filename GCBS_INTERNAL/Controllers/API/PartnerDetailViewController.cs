using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using GCBS_INTERNAL.Provider;
using log4net;

namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class PartnerDetailViewController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public List<PartnerDetailsViewModel> GetPartnerDetailView()
        {
            List<PartnerDetailsViewModel> result = new List<PartnerDetailsViewModel>();
            //Partner View 3 RoleCode
            var list = db.UserManagement.Include(x => x.CityMaster).Where(x => x.RoleId == 3).ToList();
            foreach(var a in list)
            {
                string city = "";
                if (a.CityMaster != null)
                {
                    city = a.CityMaster.CityName + " " + a.PostalCode;
                }
                else
                {
                    city =Convert.ToString(a.PostalCode);
                }
                result.Add(new PartnerDetailsViewModel
                {      
                    Image = a.Image,           
                    Location = city,
                    Partner = a.Id,
                    PartnerName= a.FirstName + " "+a.SecondName,
                    RegisterDate = Convert.ToDateTime(a.DateOfSignUp).ToString("dd-MM-yyyy"),                    
                    Status = a.Status
                });                                   
            }
            return result;   
        }
    }
}
