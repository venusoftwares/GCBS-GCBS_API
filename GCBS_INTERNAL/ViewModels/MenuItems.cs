using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.ViewModels
{
    public class MenuItems
    {
        public List<MenuItem> GetMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            menuItems.Add(new MenuItem { MenuId = 1, MainMenu = "Master", SubMenu = "Price Master" });
            menuItems.Add(new MenuItem { MenuId = 2, MainMenu = "Master", SubMenu = "Partner Types" });
            return menuItems;
        }      
    }
    public class MenuItem
    {       
        public int Id { get; set; }
        public int MenuId { get; set; }   
        public string MainMenu { get; set; }
        public string SubMenu { get; set; }
        public List<bool> Status { get; set; }
      
    }
    public class MenuItemList
    {
        public List<PermissionViewModel> permissionViewModels { get; set; }
        public List<MenuItem> menuItem { get; set; }
    }
}