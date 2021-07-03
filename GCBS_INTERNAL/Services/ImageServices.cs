using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL.Services
{
    public class ImageServices
    {
        public bool SaveImage(string ImgStr, string Type, int Id,int userId)
        {
            string[] b = ImgStr.Split(';');
            string[] c = b[0].Split('/');
            //Console.WriteLine(c[1]);
            string[] d = ImgStr.Split(',');
            string path = HttpContext.Current.Server.MapPath("~/Uploads"); //Path   
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
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");
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
                            Path = i.Folder + "/" + i.ImageUrl
                        };
                        images2.Add(a);
                    }
                }
            }
            return images2;
        }
        public bool RemoveFiles(long Id)
        {
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");
            List<string> images = new List<string>();
            using (var db = new DatabaseContext())
            {
                var list = db.ImageMaster.Where(x => x.Id == Id).ToList();
                foreach (string file in Directory.GetFiles(path))
                {
                    foreach (var a in list)
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
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");
            List<string> images = new List<string>();
            using (var db = new DatabaseContext())
            {
                var list = db.ImageMaster.Where(x => x.ReferenceId == ReferenceId).ToList();
                foreach (string file in Directory.GetFiles(path))
                {
                    foreach (var a in list)
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