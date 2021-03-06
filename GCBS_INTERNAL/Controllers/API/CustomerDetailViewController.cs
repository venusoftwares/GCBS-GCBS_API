using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using GCBS_INTERNAL.ViewModels;
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class CustomerDetailViewController  : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public List<CustomerDetailsViewModel> GetCustomerDetailView()
        {
            List<CustomerDetailsViewModel> result = new List<CustomerDetailsViewModel>();
            //Customer View 9 RoleCode
            var list = db.UserManagement.Include(x => x.CityMaster).Where(x => x.RoleId == Constant.CUSTOMER_ROLE_ID).ToList();
            foreach (var a in list)
            {
                string city = "";
                if (a.CityMaster != null)
                {
                    city = a.CityMaster.CityName + " " + a.PostalCode;
                }
                else
                {
                    city = Convert.ToString(a.PostalCode);
                }
                result.Add(new CustomerDetailsViewModel
                {
                    //Todo Image implementation
                    Image = a.Image,
                    Location = city,
                    Customer = a.Id,
                    CustomerName = a.FirstName + " " + a.SecondName,
                    RegisterDate = Convert.ToDateTime(a.DateOfSignUp).ToString("dd-MM-yyyy"),
                    Status = a.Status,
                    AccessStatus = a.AccessStatus == null ? a.Status ? 1 : 0 : a.AccessStatus,
                });
            }
            return result;
        }
    }
}
