using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GCBS_INTERNAL.Models;

namespace GCBS_INTERNAL.Controllers.API
{
    [Authorize]
    public class RolePermissionMastersController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/RolePermissionMasters
        public IQueryable<RoleMaster> GetRolePermissionMaster()
        {
            return db.RoleMaster.Where(x=>x.Status==true);
        }   

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RolePermissionMasterExists(int id)
        {
            return db.RolePermissionMaster.Count(e => e.Id == id) > 0;
        }
    }
}