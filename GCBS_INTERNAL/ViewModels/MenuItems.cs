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
            menuItems.Add(new MenuItem { MenuId = 1, MainMenu = "Master", SubMenu = "Location Master" });
            menuItems.Add(new MenuItem { MenuId = 2, MainMenu = "Master", SubMenu = "Service Type" });
            menuItems.Add(new MenuItem { MenuId = 3, MainMenu = "Master", SubMenu = "Services" });
            menuItems.Add(new MenuItem { MenuId = 4, MainMenu = "Master", SubMenu = "Margin" });
            menuItems.Add(new MenuItem { MenuId = 5, MainMenu = "Master", SubMenu = "Prices" });
            menuItems.Add(new MenuItem { MenuId = 6, MainMenu = "Master", SubMenu = "Hotel" });
            menuItems.Add(new MenuItem { MenuId = 7, MainMenu = "Master", SubMenu = "Booking" });
            menuItems.Add(new MenuItem { MenuId = 8, MainMenu = "Master", SubMenu = "Partner Type" });
            menuItems.Add(new MenuItem { MenuId = 9, MainMenu = "Master", SubMenu = "Partner Rating" });


            menuItems.Add(new MenuItem { MenuId = 41, MainMenu = "Utilities", SubMenu = "Roles" });
            menuItems.Add(new MenuItem { MenuId = 42, MainMenu = "Utilities", SubMenu = "Role Permission" });
            menuItems.Add(new MenuItem { MenuId = 43, MainMenu = "Utitlies", SubMenu = "User Management" });


            menuItems.Add(new MenuItem { MenuId = 51, MainMenu = "Settings", SubMenu = "Payment Gateway" });
            menuItems.Add(new MenuItem { MenuId = 52, MainMenu = "Settings", SubMenu = "Email Settings" });
            menuItems.Add(new MenuItem { MenuId = 53, MainMenu = "Settings", SubMenu = "Sms Settings" });

            menuItems.Add(new MenuItem { MenuId = 54, MainMenu = "Settings", SubMenu = "Site Settings" });

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