using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.Services
{
    public class ImageServices
    {
        public string urlLink = ConfigurationManager.AppSettings["ApiUrl"];
        public bool SaveImage(string ImgStr, string Type, int Id,int userId)
        {
            string[] b = ImgStr.Split(';');
            string[] c = b[0].Split('/');
            //Console.WriteLine(c[1]);
            string[] d = ImgStr.Split(',');
            string path = HttpContext.Current.Server.MapPath("~/"+ Type+"/"+ Id); //Path   
            //Check if directory exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }
            string imageName = DateTime.Now.ToString("yyyymmddhhmmssfff") + "." + c[1];
            //set the image path
            string imgPath = Path.Combine(path, imageName);
            byte[] imageBytes = Convert.FromBase64String(d[1]);
            File.WriteAllBytes(imgPath, imageBytes);
            using (var db = new DatabaseContext())
            {
                ImageMaster imageMaster = new ImageMaster();
                imageMaster.ImageUrl = imageName;
                imageMaster.Folder = "Uploads";
                imageMaster.Type = Type;
                imageMaster.ReferenceId = Id;
                imageMaster.CreatedOn = DateTime.Now;
                imageMaster.CreatedBy = userId;
                imageMaster.Status = false;
                db.ImageMaster.Add(imageMaster);
                db.SaveChanges();
                db.Dispose();
            }
            return true;
        }
        public List<Images> GetFiles(int Id, string Type)
        {
            string path = HttpContext.Current.Server.MapPath("~/"+Type+"/"+Id+"/");
            List<string> images = new List<string>();
            List<Images> images2 = new List<Images>();
            foreach (string file in Directory.GetFiles(path))
            {
                images.Add(Path.GetFileName(file));
            }
            using (var db = new DatabaseContext())
            {
                var list = db.ImageMaster.Where(x => x.ReferenceId == Id && x.Type == Type).ToList();
                foreach (var i in list)
                {
                    if (images.Contains(i.ImageUrl))
                    {
                        var a = new Images
                        {
                            Id = i.Id,
                            Path = urlLink+ "/"+ i.Type + "/" + i.ReferenceId + "/" + i.ImageUrl
                        };
                        images2.Add(a);
                    }
                }
            }
            return images2;
        }
        public List<string> EditGetFiles(int Id, string Type)
        {     
            List<string> images = new List<string>();   
            using (var db = new DatabaseContext())
            {
                var list = db.ImageMaster.Where(x => x.ReferenceId == Id && x.Type == Type).ToList();
                foreach (var i in list)
                {
                    string[] extension = i.ImageUrl.Split('.');
                    string base64string = ImageToBase64(HttpContext.Current.Server.MapPath("~/" + i.Type + "/" + i.ReferenceId + "/" + i.ImageUrl), extension[1]);
                    if(base64string!=null)
                    {
                        images.Add(base64string);
                    }    
                }
            }
            return images;
        }
        public string ImageToBase64(string path,string extension)
        {    
            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        return "data:image/"+ extension + ";base64,"+ base64String;
                    }
                }
            }
            catch
            {
                return null;
            }
            
        }
        public bool RemoveFiles(long Id)
        {     
            List<string> images = new List<string>();
            using (var db = new DatabaseContext())
            {
                var list = db.ImageMaster.Where(x => x.Id == Id).ToList();     
                foreach (var a in list)
                {
                    string path = HttpContext.Current.Server.MapPath("~/"+ a.Type+ "/" + a.ReferenceId+"/");
                    foreach (string file in Directory.GetFiles(path))
                    {
                        if (file.Contains(a.ImageUrl))
                        {
                            File.Delete(file);
                        }
                    }
                }                
                db.ImageMaster.RemoveRange(list);
                db.SaveChanges();
            }
            return true;
        }
        public bool DeleteFiles(long ReferenceId)
        {       
            List<string> images = new List<string>();
            using (var db = new DatabaseContext())
            {
                var list = db.ImageMaster.Where(x => x.ReferenceId == ReferenceId).ToList();
                foreach (var a in list)
                {
                    string path = HttpContext.Current.Server.MapPath("~/" + a.Type + "/" + a.ReferenceId + "/");
                    foreach (string file in Directory.GetFiles(path))
                    {
                        if (file.Contains(a.ImageUrl))
                        {
                            File.Delete(file);
                        }
                    }
                }
                db.ImageMaster.RemoveRange(list);
                db.SaveChanges();
            }
            return true;
        }

    }
}