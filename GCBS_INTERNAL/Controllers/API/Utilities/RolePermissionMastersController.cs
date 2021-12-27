using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using GCBS_INTERNAL.ViewModels;
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
        public MenuItemList GetRolePermissionMasters(int id)
        {
            return GetMenuItems(id);
        }
        

        #region
        private MenuItemList GetMenuItems(int RoleId)
        {
            MenuItemList menuItemList = new MenuItemList();
            List<MenuItem> menuItem = new List<MenuItem>();
            MenuItems menuItems = new MenuItems();
            var list = menuItems.GetMenuItems();
            var permissionViewModels = db.PermissionKeyValue.Select(x => new PermissionViewModel { Key = x.Key, Value = x.Value, Visible = x.Status }).ToList();
            var permissionMaster = db.RolePermissionMaster.Where(x => x.RoleId == RoleId).ToList();
            foreach (var a in list)
            {
                MenuItem menu = new MenuItem();
                menu = a;
                if (permissionMaster.Any(x => x.MenuId == a.MenuId))
                {
                    var b = permissionMaster.Where(x => x.MenuId == a.MenuId).FirstOrDefault();
                    menu.Id = b.Id;
                    menu.Status = PermissionConvert(permissionViewModels, b.Privilege);
                }
                else
                {
                    menu.Id = 0;
                    menu.Status = PermissionConvert(permissionViewModels);
                }
                menuItem.Add(menu);
            }
            menuItemList.permissionViewModels = permissionViewModels;
            menuItemList.menuItem = menuItem;
            return menuItemList;
        }
        private List<bool> PermissionConvert(List<PermissionViewModel> permissionViews, string Permission = "")
        {
            List<bool> list = new List<bool>();
            if (!String.IsNullOrEmpty(Permission))
            {
                var spl = Permission.Split('|');
                foreach (var a in permissionViews)
                {
                    if (spl.Contains(a.Value))
                    {
                        list.Add(true);

                    }
                    else
                    {
                        list.Add(false);
                    }
                }
            }
            else
            {
                foreach (var a in permissionViews)
                {
                    list.Add(false);
                }
            }
            return list;
        }
        #endregion

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRolePermissionMasters(RolePermissionRequest rolePermissionRequest)
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
