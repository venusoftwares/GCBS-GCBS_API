using GCBS_INTERNAL.Models;
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
   
    [Authorize]
    public class ImageAPIController : BaseApiController
    {      
        [HttpPost]
        [Route("api/ImageAPI/MultipleUploadFiles/{Id}/{Type}")]
        public IHttpActionResult MultiUploadFiles(int Id,string Type)
        {
            if(Id>0 && !string.IsNullOrEmpty(Type))
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
                        postedFile.SaveAs(path + imageName);
                        using (var db = new DatabaseContext())
                        {
                            ImageMaster imageMaster = new ImageMaster();
                            imageMaster.ImageUrl = imageName;
                            imageMaster.Folder = "Uploads";
                            imageMaster.Type = Type;
                            imageMaster.ReferenceId = Id;
                            imageMaster.CreatedOn = DateTime.Now;
                            imageMaster.CreatedBy = userDetails.Id;
                            imageMaster.Status = false;
                            db.ImageMaster.Add(imageMaster);
                            db.SaveChanges();
                            db.Dispose();
                        }
                        i++;
                    }
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }                              
        }     

        [HttpPost]
        [Route("api/ImageAPI/GetFiles/{Id}/{Type}")]
        public IHttpActionResult GetFiles(int Id, string Type)
        {
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");  
            List<string> images = new List<string>();
            List<Images> images2 = new List<Images>();
            foreach (string file in Directory.GetFiles(path))
            {
                images.Add(Path.GetFileName(file));
            } 
            using(var db = new DatabaseContext())
            {
                var list = db.ImageMaster.Where(x => x.ReferenceId == Id && x.Type == Type).ToList();
                foreach(var i in list)
                {
                    if(images.Contains(i.ImageUrl))
                    {
                        var a = new Images
                        {
                            Id = i.Id,
                            Path = i.Folder + "/" + i.ImageUrl
                        };
                        images2.Add(a);
                    }
                }
            }
            return Ok(images2);
        }
        [HttpPost]
        [Route("api/ImageAPI/RemoveFiles/{Id}")]
        public IHttpActionResult RemoveFiles(long Id)
        {
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");
            List<string> images = new List<string>();   
            using (var db = new DatabaseContext())
            {
                var list = db.ImageMaster.Where(x => x.Id == Id).ToList();
                foreach (string file in Directory.GetFiles(path))
                {
                    foreach(var a in list)
                    {
                        if(file.Contains(a.ImageUrl))
                        {
                            File.Delete(file);
                        }
                    }
                }
                db.ImageMaster.RemoveRange(list);
                db.SaveChanges();
            }
            return Ok();
        }
        public class Images
        {
            public long Id { get; set; }
            public string Path { get; set; }
        }
    }   
}
