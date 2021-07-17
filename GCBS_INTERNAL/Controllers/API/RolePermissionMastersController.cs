using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.ViewModels;
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
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class RolePermissionMastersController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/RolePermissionMasters
        public List<RoleMaster> GetRolePermissionMaster()
        {
            return db.RoleMaster.Where(x=>x.Status==true).ToList();
        }

        // GET: api/PriceMasters/5
        
        public MenuItemList GetRolePermissionMaster(int id)
        {
            return GetMenuItems(id);
        }

        // PUT: api/PriceMasters/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRolePermissionMaster(RolePermissionRequest priceMasterVisible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (priceMasterVisible == null)
            {
                return BadRequest();
            }
          
            RolePermissionMaster rolePermissionMaster = await db.RolePermissionMaster.Where(x=>x.MenuId == priceMasterVisible.MenuId && x.RoleId == priceMasterVisible.RoleId).FirstOrDefaultAsync();
            if(rolePermissionMaster!=null)
            {
                rolePermissionMaster.Privilege = ConvertString(priceMasterVisible.Permission);
                rolePermissionMaster.UpdatedBy = userDetails.Id;
                rolePermissionMaster.UpdatedOn = DateTime.Now;
                db.Entry(rolePermissionMaster).State = EntityState.Modified;
            }
            else
            {
                rolePermissionMaster = new RolePermissionMaster();
                rolePermissionMaster.MenuId = priceMasterVisible.MenuId;
                rolePermissionMaster.RoleId = priceMasterVisible.RoleId;
                rolePermissionMaster.Privilege = ConvertString(priceMasterVisible.Permission);
                rolePermissionMaster.CreatedBy = userDetails.Id;
                rolePermissionMaster.CreatedOn = DateTime.Now;
                db.RolePermissionMaster.Add(rolePermissionMaster);
            }   
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }


        #region
        private MenuItemList GetMenuItems(int RoleId)
        {
            MenuItemList menuItemList = new MenuItemList();
            List<MenuItem> menuItem = new List<MenuItem>();
            MenuItems menuItems = new MenuItems();
            var list =  menuItems.GetMenuItems();
            var permissionViewModels = db.PermissionKeyValue.Select(x=> new PermissionViewModel { Key = x.Key,Value=x.Value,Visible = x.Status }).ToList();
            var permissionMaster = db.RolePermissionMaster.Where(x => x.RoleId == RoleId).ToList();
            foreach(var a in list)
            {
                MenuItem menu = new MenuItem();
                menu = a;
                if (permissionMaster.Any(x=>x.MenuId == a.MenuId))
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

        private string ConvertString(bool[] list)
        {
            int count = 0;
            foreach(var o in list)
            {
                if(o)
                {
                    count++;
                }
            }
            var permissionViewModels = db.PermissionKeyValue.Select(x => new PermissionViewModel { Key = x.Key, Value = x.Value, Visible = x.Status }).ToList();
            string res = "";
            string[] t = new string[count];
            int i = 0;
            int j = 0;
            foreach(var k in permissionViewModels)
            {
                if(list[i])
                {
                    t[j] = k.Value;
                    j++;
                }
                i++;
            }
            res = string.Join("|", t);
            return res;
        }
        private List<bool> PermissionConvert(List<PermissionViewModel> permissionViews,string Permission="")
        {
            List<bool> list = new List<bool>();
            if(!String.IsNullOrEmpty(Permission))
            {
                var spl = Permission.Split('|');
                foreach(var a in permissionViews)
                {
                    if(spl.Contains(a.Value))
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}