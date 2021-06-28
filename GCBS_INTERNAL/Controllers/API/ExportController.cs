using GCBS_INTERNAL.Models;
using OpenHtmlToPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace GCBS_INTERNAL.Controllers.API
{
    [Authorize]
    public class ExportController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
       
      
        [HttpPost]
        [Route("api/ExportExcelCountry")]
        [ResponseType(typeof(HttpPostFile))]
        public IHttpActionResult ExportExcelCountry()
        {       
            return Ok(new HttpPostFile {  FileData = CountryToHtml().ToString(),FileName=DateTime.Now.ToString("yyyyMMddhhmmtt")+".xls",FileType= "application/vnd.ms-excel" });
        }
        [HttpPost]
        [Route("api/ExportExcelState")]
        [ResponseType(typeof(HttpPostFile))]
        public IHttpActionResult ExportExcelState()
        {
            return Ok(new HttpPostFile { FileData = StateToHtml().ToString(), FileName = DateTime.Now.ToString("yyyyMMddhhmmtt") + ".xls", FileType = "application/vnd.ms-excel" });
           
        }
        [HttpPost]
        [Route("api/ExportExcelCity")]
        [ResponseType(typeof(HttpPostFile))]
        public IHttpActionResult ExportExcelCity()
        {
            return Ok(new HttpPostFile { FileData = CityToHtml().ToString(), FileName = DateTime.Now.ToString("yyyyMMddhhmmtt") + ".xls", FileType = "application/vnd.ms-excel" });
           
        }
        [HttpPost]
        [Route("api/ExportExcelLocation")]
        [ResponseType(typeof(HttpPostFile))]
        public IHttpActionResult ExportExcelLocation()
        {
            return Ok(new HttpPostFile { FileData = LocationToHtml().ToString(), FileName = DateTime.Now.ToString("yyyyMMddhhmmtt") + ".xls", FileType = "application/vnd.ms-excel" });            
        }

      

        [HttpPost]
        [Route("api/ExportPdfCountry")]
        [ResponseType(typeof(HttpPostFile))]
        public IHttpActionResult ExportPdfCountry()
        {
            string path = HttpContext.Current.Server.MapPath("~/Template/CountryTemp.html");
            string html = File.ReadAllText(path);
            var obj = GetcountryDetails();
            StringBuilder str = new StringBuilder();  
            str.Append("</tr>");
            foreach (RecordCountry val in obj)
            {
                str.Append("<tr>");
                str.Append("<td>"+ val.CountryName.ToString() + "</td>");
                str.Append("<td>"+ val.ShortName.ToString() + "</td>");
                str.Append("<td>"+ val.CountryCode.ToString() + "</td>");
                str.Append("</tr>");
            }
            html = html.Replace("{td}", str.ToString());  
             
            return Ok(new HttpPostFile { FileData = HtmlToPdf(html).ToString(), FileName = DateTime.Now.ToString("yyyyMMddhhmmtt") + ".pdf", FileType = "application/pdf" });
        }
     
        [HttpPost]
        [Route("api/ExportPdfState")]
        [ResponseType(typeof(HttpPostFile))]
        public IHttpActionResult ExportPdfState()
        {
            string path = HttpContext.Current.Server.MapPath("~/Template/StateTemp.html");
            string html = File.ReadAllText(path);
            var obj = GetStateDetails();
            StringBuilder str = new StringBuilder();
            str.Append("</tr>");
            foreach (RecordState val in obj)
            {
                str.Append("<tr>");
                str.Append("<td>" + val.State.ToString() + "</td>");
                str.Append("<td>" + val.Country.ToString() + "</td>");  
                str.Append("</tr>");
            }
            html = html.Replace("{td}", str.ToString());
            return Ok(new HttpPostFile { FileData = HtmlToPdf(html).ToString(), FileName = DateTime.Now.ToString("yyyyMMddhhmmtt") + ".pdf", FileType = "application/pdf" });
             
        }
        [HttpPost]
        [Route("api/ExportPdfCity")]
        [ResponseType(typeof(HttpPostFile))]
        public IHttpActionResult ExportPdfCity()
        {
            string path = HttpContext.Current.Server.MapPath("~/Template/CityTemp.html");
            string html = File.ReadAllText(path);
            var obj = GetCityDetails();
            StringBuilder str = new StringBuilder();
            str.Append("</tr>");
            foreach (RecordCity val in obj)
            {
                str.Append("<tr>");
                str.Append("<td>" + val.City.ToString() + "</td>");
                str.Append("<td>" + val.State.ToString() + "</td>");
                str.Append("<td>" + val.Country.ToString() + "</td>");
                str.Append("</tr>");
            }
            html = html.Replace("{td}", str.ToString());
            return Ok(new HttpPostFile { FileData = HtmlToPdf(html).ToString(), FileName = DateTime.Now.ToString("yyyyMMddhhmmtt") + ".pdf", FileType = "application/pdf" });
        }
        [HttpPost]
        [Route("api/ExportPdfLocation")]
        [ResponseType(typeof(HttpPostFile))]
        public IHttpActionResult ExportPdfLocation()
        {
            string path = HttpContext.Current.Server.MapPath("~/Template/LocationTemp.html");
            string html = File.ReadAllText(path);
            var obj = GetLocationDetails();
            StringBuilder str = new StringBuilder();
            str.Append("</tr>");
            foreach (RecordLocation val in obj)
            {
                str.Append("<tr>");
                str.Append("<td>" + val.City.ToString() + "</td>");
                str.Append("<td>" + val.Location.ToString() + "</td>");
                str.Append("<td>" + val.PinCode.ToString() + "</td>");
                str.Append("<td>" + val.State.ToString() + "</td>");
                str.Append("<td>" + val.Country.ToString() + "</td>");
                str.Append("</tr>");
            }
            html = html.Replace("{td}", str.ToString());
            return Ok(new HttpPostFile { FileData = HtmlToPdf(html).ToString(), FileName = DateTime.Now.ToString("yyyyMMddhhmmtt") + ".pdf", FileType = "application/pdf" });
        }



        private string CountryToHtml()
        {
            var obj = GetcountryDetails();
            StringBuilder str = new StringBuilder();
            str.Append("<table border=`" + "1px" + "`b>");
            str.Append("<tr>");
            str.Append("<td><b><font face=Arial Narrow size=3>CountryName</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>ShortName</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>CountryCode</font></b></td>");
            str.Append("</tr>");
            foreach (RecordCountry val in obj)
            {
                str.Append("<tr>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.CountryName.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.ShortName.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.CountryCode.ToString() + "</font></td>");
                str.Append("</tr>");
            }
            str.Append("</table>");
            byte[] temp = Encoding.UTF8.GetBytes(str.ToString());   
            return Convert.ToBase64String(temp);
        }
   

        private string StateToHtml()
        {
            var obj = GetStateDetails();
            StringBuilder str = new StringBuilder();
            str.Append("<table border=`" + "1px" + "`b>");
            str.Append("<tr>");
            str.Append("<td><b><font face=Arial Narrow size=3>State</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Country</font></b></td>");    
            str.Append("</tr>");
            foreach (RecordState val in obj)
            {
                str.Append("<tr>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.State.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.Country.ToString() + "</font></td>");  
                str.Append("</tr>");
            }
            str.Append("</table>");
            byte[] temp = Encoding.UTF8.GetBytes(str.ToString());
            return str.ToString();
        }
        private string CityToHtml()
        {
            var obj = GetCityDetails();
            StringBuilder str = new StringBuilder();
            str.Append("<table border=`" + "1px" + "`b>");
            str.Append("<tr>");
            str.Append("<td><b><font face=Arial Narrow size=3>City</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>State</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Country</font></b></td>");
            str.Append("</tr>");
            foreach (RecordCity val in obj)
            {
                str.Append("<tr>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.City.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.State.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.Country.ToString() + "</font></td>");
                str.Append("</tr>");
            }
            str.Append("</table>");
            byte[] temp = Encoding.UTF8.GetBytes(str.ToString());
            return str.ToString();
        }
        private string LocationToHtml()
        {
            var obj = GetLocationDetails();
            StringBuilder str = new StringBuilder();
            str.Append("<table border=`" + "1px" + "`b>");
            str.Append("<tr>");
            str.Append("<td><b><font face=Arial Narrow size=3>City</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Location</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>PinCode</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>State</font></b></td>");
            str.Append("<td><b><font face=Arial Narrow size=3>Country</font></b></td>");
            str.Append("</tr>");
            foreach (RecordLocation val in obj)
            {
                str.Append("<tr>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.City.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.Location.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.PinCode.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.State.ToString() + "</font></td>");
                str.Append("<td><font face=Arial Narrow size=" + "10px" + ">" + val.Country.ToString() + "</font></td>");
                str.Append("</tr>");
            }
            str.Append("</table>");
            byte[] temp = Encoding.UTF8.GetBytes(str.ToString());
            return str.ToString();
        }

    

        private List<RecordCountry> GetcountryDetails()
        {
            return db.CountryMaster.Select(x => new RecordCountry
            {
                CountryCode = x.CountryCode,
                ShortName = x.ShortName,
                CountryName = x.CountryName
            }).ToList();
        }
        private List<RecordState> GetStateDetails()
        {
            return db.StateMaster.Include(x => x.CountryMaster)
                .Select(x => new RecordState
                {
                    Country = x.CountryMaster.CountryName,
                    State = x.StateName
                }).ToList();
        }
        private List<RecordCity> GetCityDetails()
        {
            return db.CityMaster
                .Include(x => x.CountryMaster)
                .Include(x=>x.StateMaster)
               .Select(x => new RecordCity
               {
                   Country = x.CountryMaster.CountryName,
                   State = x.StateMaster.StateName,
                   City = x.CityName

               }).ToList();
        }

        private List<RecordLocation> GetLocationDetails()
        {
            return db.LocationMasters
                .Include(x => x.CountryMaster)
                .Include(x => x.StateMaster)
                  .Include(x => x.CityMaster)
               .Select(x => new RecordLocation
               {
                   Country = x.CountryMaster.CountryName,
                   State = x.StateMaster.StateName,
                   City = x.CityMaster.CityName,
                   Location = x.Location,
                   PinCode = x.PinCode    
               }).ToList();
        }
        private class RecordCountry
        {
            public string CountryName { get; set; }
            public string ShortName { get; set; }
            public int CountryCode { get; set; }
        }
        private class RecordState
        {
            public string Country { get; set; }
            public string State { get; set; }
            
        }
        private class RecordCity
        {
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }

        }
        private class RecordLocation
        {
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Location { get; set; }
            public int PinCode { get; set; }

        }
        public class HttpPostFile
        {
            public string FileName { get; set; }
            public string FileType { get; set; }
            public string FileData { get; set; }   
        }

        private string HtmlToPdf(string htmlContent)
        {
            var pdfBytes = Pdf.From(htmlContent).OfSize(PaperSize.A4).WithGlobalSetting("orientation", "Portrait").WithObjectSetting("web.defaultEncoding", "utf-8").Content();
            string base64 = Convert.ToBase64String(pdfBytes);
            return base64;
        }
    }
}
