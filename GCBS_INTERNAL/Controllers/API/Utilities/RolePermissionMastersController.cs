using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GCBS_INTERNAL.Controllers.API.Utilities
{
    [CustomAuthorize]
    public class RolePermissionMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        public RolePermissionMastersController()
        {

        }
       

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRolePermissionMaster(RolePermissionRequest rolePermissionRequest)
        {
            if(rolePermissionRequest==null)
            {
                return BadRequest();
            }
            List<string> list = new List<string>();
            List<PermissionKeyValue> permissionKeyValues =await db.PermissionKeyValue.ToListAsync();
            int i = 0;
            foreach(var a in permissionKeyValues)
            {
                if (rolePermissionRequest.Permission[i])
                {
                    list.Add(a.Value);
                }
                i++;
            }      
            RolePermissionMaster rolePermissionMaster = await db.RolePermissionMaster
                .Where(x=>x.MenuId == rolePermissionRequest.MenuId && x.RoleId == rolePermissionRequest.RoleId).FirstOrDefaultAsync();
            rolePermissionMaster.Privilege = string.Join("|",list);
            rolePermissionMaster.UpdatedBy = userDetails.Id;
            rolePermissionMaster.UpdatedOn = DateTime.Now;
            db.Entry(rolePermissionMaster).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
