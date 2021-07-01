using GCBS_INTERNAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API
{
    public class BaseApiController : ApiController
    {
        protected UserManagement userDetails = new UserManagement();
        private DatabaseContext context = new DatabaseContext();
        public BaseApiController()
        {
            var response = HttpContext.Current.User.Identity.Name;
            userDetails = JsonConvert.DeserializeObject<UserManagement>(response);
        }

        //public   List<string> MultiUploadFiles(string folderName)
        //{
        //    List<string> list = new List<string>();
        //    string path = HttpContext.Current.Server.MapPath("~/"+ folderName + "/");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    int i = 0;
        //    var httpRequest = HttpContext.Current.Request;
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        foreach (string file in httpRequest.Files)
        //        {
        //            var postedFile = httpRequest.Files[i];
        //            string imageName = DateTime.Now.ToString("yyyymmddhhmmssfff") +
        //                Path.GetExtension(postedFile.FileName);
        //            postedFile.SaveAs(path + imageName);
        //            list.Add(imageName);
        //            i++;
        //        }
        //    }
        //    return list;
        //}

        //public async Task SaveImage(string folderName,List<string> imageUrl, int referenceId,string type)
        //{
        //    using(var db = new DatabaseContext())
        //    {
        //        if(imageUrl.Count() > 0)
        //        {
        //            List<ImageMaster> imageMasters = new List<ImageMaster>();
        //            foreach (var list in imageUrl)
        //            {
        //                ImageMaster imageMaster = new ImageMaster();
        //                imageMaster.Folder = folderName;
        //                imageMaster.ImageUrl = list;
        //                imageMaster.ReferenceId = referenceId;
        //                imageMaster.Type = type;
        //                imageMasters.Add(imageMaster);
        //            }
        //            db.ImageMaster.AddRange(imageMasters);
        //            await db.SaveChangesAsync();
        //        }     
        //    }
        //}
    }
}
