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
        {    try
            {
                string[] b = ImgStr.Split(';');
                string[] c = b[0].Split('/');
                //Console.WriteLine(c[1]);
                string[] d = ImgStr.Split(',');
                string path = HttpContext.Current.Server.MapPath("~/" + Type + "/" + Id); //Path   
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
            catch(Exception ex)
            {
                return false; 
            }
           
        }
        public List<string> GetFiles(int Id, string Type)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/" + Type + "/" + Id + "/");
                List<string> images = new List<string>();
                List<string> images2 = new List<string>();
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
                            images2.Add(urlLink + "/" + i.Type + "/" + i.ReferenceId + "/" + i.ImageUrl);
                        }
                    }
                }
                return images2;
            }
            catch(Exception ex)
            {
                return new List<string>();
            }    
        }
        public List<string> EditGetFiles(int Id, string Type)
        {
            string path = HttpContext.Current.Server.MapPath("~/" + Type + "/" + Id); //Path   
                                                                                      //Check if directory exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }
            string temp = "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";
            List<string> images = new List<string>();
            try
            {
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
                    if(list.Count()==0)
                    {
                        images.Add("data:image/png;base64," + temp);
                    }
                }
                return images;
            }
            catch (Exception ex)
            {
                images.Add("data:image/png;base64," + temp);
                return images;
            }
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
        public bool DeleteFiles(long ReferenceId,string Type)
        {       
            List<string> images = new List<string>();
            using (var db = new DatabaseContext())
            {
                var list = db.ImageMaster.Where(x => x.ReferenceId == ReferenceId && x.Type == Type).ToList();
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