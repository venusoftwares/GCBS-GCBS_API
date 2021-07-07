using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using GCBS_INTERNAL.ViewModels;

namespace GCBS_INTERNAL.Controllers.API
{
    [Authorize]
    public class CustomerDetailViewController  : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public List<CustomerDetailsViewModel> GetCustomerDetailView()
        {
            List<CustomerDetailsViewModel> result = new List<CustomerDetailsViewModel>();
            //Customer View 9 RoleCode
            var list = db.UserManagement.Include(x => x.LocationMasters).Where(x => x.RoleId == 9).ToList();
            foreach (var a in list)
            {         
                result.Add(new CustomerDetailsViewModel
                {
                    //Todo Image implementation
                    Image = "",
                    Location = a.LocationMasters.Location + "_" + a.LocationMasters.PinCode,   
                    Customer = a.Id,
                    CustomerName = a.Username,
                    RegisterDate = a.CreatedOn.ToString("dd-MM-yyyy"),
                    Status = a.Status
                });
            }
            return result;
        }
    }
}
