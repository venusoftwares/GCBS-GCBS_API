using ExcelDataReader;
using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GCBS_INTERNAL.Provider;
namespace GCBS_INTERNAL.Controllers.API
{
     [CustomAuthorize]
    public class ImportController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();
        [HttpPost]
        [Route("api/ImportExcelCountry")]
        public IHttpActionResult ImportCountry()
        {
            string message = "";
            int r = ImportExcel("Country");
            if(r==0)
            {
                return BadRequest();
            }  
            return Ok(message);
        }
        [HttpPost]
        [Route("api/ImportExcelState/{CountryId}")]
        public IHttpActionResult ImportState(int CountryId)
        {
            string message = "";
            int r = ImportExcel("State",CountryId);
            if (r == 0)
            {
                return BadRequest();
            } 
            return Ok(message);
        }
        [HttpPost]
        [Route("api/ImportExcelCity/{CountryId}/{StateId}")]
        public IHttpActionResult ImportCity(int CountryId,int StateId)
        {
            string message = "";
            int r = ImportExcel("City", CountryId, StateId);
            if (r == 0)
            {
                return BadRequest();
            }    
            return Ok(message);
        }
        [HttpPost]
        [Route("api/ImportExcelLocation/{CountryId}/{StateId}/{CityId}")]
        public IHttpActionResult ImportState(int CountryId, int StateId,int CityId)
        {
            string message = "";
            int r = ImportExcel("Location", CountryId, StateId, CityId);
            if (r == 0)
            {
                return BadRequest();
            }  
            return Ok(message);
        }

        private int ImportExcel(string type,int CountryId=0,int StateId=0,int CityId=0)
        {
            int res = 0;      
            var httpRequest = HttpContext.Current.Request;             
            if (httpRequest.Files.Count > 0)
            {
                HttpPostedFile file = httpRequest.Files[0];
                Stream stream = file.InputStream;
                IExcelDataReader reader = null;
                if (file.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (file.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    return 3;  //Model error
                }
                DataSet excelRecords = reader.AsDataSet();
                reader.Close();
                if(type== "Country")
                {
                    return UpdateCountry(excelRecords.Tables[0]);
                }
                if(type== "State")
                {
                    return UpdateState(excelRecords.Tables[0],CountryId);
                }
                if (type == "City")
                {
                    return UpdateCity(excelRecords.Tables[0], CountryId,StateId);
                }
                if (type == "Location")
                {
                    return UpdateLocation(excelRecords.Tables[0], CountryId, StateId, CityId);
                }

            }  
            return res;
        }

       

        public int UpdateCountry(DataTable dataTable)
        {
            using (DatabaseContext db = new DatabaseContext())
            {    
                for (int i = 1; i < dataTable.Rows.Count; i++)
                {
                    CountryMaster countryMaster = new CountryMaster();
                    countryMaster.CountryName = dataTable.Rows[i][0].ToString();
                    countryMaster.ShortName = dataTable.Rows[i][1].ToString();
                    countryMaster.CountryCode = Convert.ToInt32(dataTable.Rows[i][2].ToString());
                    countryMaster.CreatedBy = userDetails.Id;
                    countryMaster.CreatedOn = DateTime.Now;
                    countryMaster.Status = true;
                    using (var a = new DatabaseContext())
                    {
                        if (!a.CountryMaster.Any(x => x.CountryName.ToLower() == countryMaster.CountryName.ToLower()))
                        {
                            db.CountryMaster.Add(countryMaster);
                        }
                        a.Dispose();
                    }
                }
                int output = db.SaveChanges();
                if (output > 0)
                {
                    return  1;  //success
                }
                else
                {
                    return  2;  //failed
                }
                
            }
        }
        private int UpdateState(DataTable dataTable, int countryId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                for (int i = 1; i < dataTable.Rows.Count; i++)
                {
                    StateMaster stateMaster = new StateMaster();
                    stateMaster.CountryId = countryId;
                    stateMaster.StateName = dataTable.Rows[i][0].ToString();  
                    stateMaster.CreatedBy = userDetails.Id;
                    stateMaster.CreatedOn = DateTime.Now;
                    stateMaster.Status = true;
                    using (var a = new DatabaseContext())
                    {
                        if (!a.StateMaster.Where(x=>x.CountryId == countryId)
                            .Any(x => x.StateName.ToLower() == stateMaster.StateName.ToLower()))
                        {
                            db.StateMaster.Add(stateMaster);
                        }
                        a.Dispose();
                    }
                }
                int output = db.SaveChanges();
                if (output > 0)
                {
                    return 1;  //success
                }
                else
                {
                    return 2;  //failed
                }

            }
        }
        private int UpdateCity(DataTable dataTable, int countryId, int stateId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                for (int i = 1; i < dataTable.Rows.Count; i++)
                {
                    CityMaster cityMaster = new CityMaster();
                    cityMaster.CountryId = countryId;
                    cityMaster.StateId = stateId;
                    cityMaster.CityName = dataTable.Rows[i][0].ToString();
                    cityMaster.CreatedBy = userDetails.Id;
                    cityMaster.CreatedOn = DateTime.Now;
                    cityMaster.Status = true;
                    using (var a = new DatabaseContext())
                    {
                        if (!a.CityMaster.Where(x => x.CountryId == countryId && x.StateId == stateId)
                            .Any(x => x.CityName.ToLower() == cityMaster.CityName.ToLower()))
                        {
                            db.CityMaster.Add(cityMaster);
                        }
                        a.Dispose();
                    }
                }
                int output = db.SaveChanges();
                if (output > 0)
                {
                    return 1;  //success
                }
                else
                {
                    return 2;  //failed
                }            
            }
        } 
        private int UpdateLocation(DataTable dataTable, int countryId, int stateId, int cityId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                for (int i = 1; i < dataTable.Rows.Count; i++)
                {
                    LocationMasters locationMasters = new LocationMasters();
                    locationMasters.CountryId = countryId;
                    locationMasters.StateId = stateId;
                    locationMasters.CityId = cityId;
                    locationMasters.Location = dataTable.Rows[i][0].ToString();
                    locationMasters.PinCode = Convert.ToInt32(dataTable.Rows[i][1].ToString());
                    locationMasters.CreatedBy = userDetails.Id;
                    locationMasters.CreatedOn = DateTime.Now;
                    locationMasters.Status = true;
                    using (var a = new DatabaseContext())
                    {
                        if (!a.LocationMasters.Where(x => x.CountryId == countryId && x.StateId == stateId && x.CityId == cityId)
                            .Any(x => x.Location.ToLower() == locationMasters.Location.ToLower() 
                            || x.PinCode == locationMasters.PinCode))
                        {
                            db.LocationMasters.Add(locationMasters);
                        }
                        a.Dispose();
                    }
                }
                int output = db.SaveChanges();
                if (output > 0)
                {
                    return 1;  //success
                }
                else
                {
                    return 2;  //failed
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
