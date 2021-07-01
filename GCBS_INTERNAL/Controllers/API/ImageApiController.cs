using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API
{
   
    public class ImageAPIController : ApiController
    {
        [Route("api/ImageAPI/MultipleUploadFiles")]
        [HttpPost]
        public HttpResponseMessage MultiUploadFiles()
        {
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }      
            int i = 0;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {                           
                    var postedFile = httpRequest.Files[i];      
                    string imageName = DateTime.Now.ToString("yyyymmddhhmmssfff") +
                        Path.GetExtension(postedFile.FileName);   
                    postedFile.SaveAs(path+ imageName);   
                    i++;
                }
            }    
            return Request.CreateResponse(HttpStatusCode.Created);
        }     

        [HttpPost]
        [Route("api/ImageAPI/GetFiles")]
        public HttpResponseMessage GetFiles()
        {
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");  
            List<string> images = new List<string>(); 
            foreach (string file in Directory.GetFiles(path))
            {
                images.Add(Path.GetFileName(file));
            }  
            return Request.CreateResponse(HttpStatusCode.OK, images);
        }
        [HttpPost]
        [Route("api/ImageAPI/UploadFiles")]
        public HttpResponseMessage UploadFiles()
        {
            string uname = HttpContext.Current.Request["uploadername"];
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            int i = 0;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[i];
                    string imageName = DateTime.Now.ToString("yyyymmddhhmmssfff") +
                        Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(path + imageName);
                    i++;
                }
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }

}
