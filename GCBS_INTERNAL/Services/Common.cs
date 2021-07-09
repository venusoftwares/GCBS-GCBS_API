using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.Services
{
    public class Common
    {
        public string FolderForRole(int RoleId)
        {
            if(RoleId== 1)
            {
                return Constant.SUPER_ADMIN_FLODER_TYPE;
            }
            if (RoleId == 3)
            {
                return Constant.SERVICE_PROVIDER_FOLDER_TYPE;
            }
            if (RoleId == 6)
            {
                return Constant.SERVICE_MANAGER_FOLDER_TYPE;
            }
            if (RoleId == 7)
            {
                return Constant.SUPPORT_TEAM_FOLDER_TYPE;
            }
            if (RoleId == 8)
            {
                return Constant.HUMAN_RESOURCE_FOLDER_TYPE;
            }
            if (RoleId == 9)
            {
                return Constant.CUSTOMER_FOLDER_TYPE;
            }
            return "Temp";
        }
    }
}