using GCBS_INTERNAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GCBS_INTERNAL.Provider
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var response = HttpContext.Current.User.Identity.Name;
            var userDetails = JsonConvert.DeserializeObject<UserManagement>(response);
            using (var db = new DatabaseContext())
            {
                var user = db.UserManagement.Find(userDetails.Id);
                if (DateTime.Now <= user.LastActivateTime)
                {
                    user.LastActivateTime = DateTime.Now.AddMinutes(Constant.ExpireTime);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return;
                }
                HandleUnauthorizedRequest(actionContext);
            }   
        }
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);     
        }
        private bool AuthorizeRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //Write your code here to perform authorization
            return true;
        }
    }
}