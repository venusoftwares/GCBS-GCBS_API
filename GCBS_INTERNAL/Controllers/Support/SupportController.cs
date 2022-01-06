

using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Models.Support;
using GCBS_INTERNAL.Provider;
using GCBS_INTERNAL.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace GCBS_INTERNAL.Controllers.Support
{
    [CustomAuthorize]
    public class SupportController : BaseApiController
    {
        private DatabaseContext db = new DatabaseContext();

        [System.Web.Http.Route("api/GetSupportType")]
        public async Task<IHttpActionResult> GetSupportType()
        {
            try
            {
                return Ok(await db.SupportType.ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Http.Route("api/SumbitSupportType")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> SumbitSupportType(SupportType supportType)
        {
            try
            {

                db.SupportType.Add(supportType);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.Route("api/deleteSupportType/{id}")]
        [System.Web.Http.HttpDelete]
        public async Task<IHttpActionResult> DeleteSupportType(int id)
        {
            try
            {
                var list = db.SupportType.Find(id);
                var a = db.SupportType.Remove(list);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class SupportReplyDTO
        {
            public int Id { get; set; }
            public int StatusId { get; set; }
            public string Message { get; set; }
        }

        [System.Web.Http.Route("api/AdminSupportReply")]
        public async Task<IHttpActionResult> AdminSubmitReply(SupportReplyDTO supportReplyDTO)
        {
            try
            {
                var support = await db.Support.FindAsync(supportReplyDTO.Id);

                List<ReportAndSupportViewModel> reportAndSupportViewModels = new List<ReportAndSupportViewModel>();

                reportAndSupportViewModels.AddRange(JsonConvert.DeserializeObject<List<ReportAndSupportViewModel>>(support.Description));

                reportAndSupportViewModels.Add(new ReportAndSupportViewModel()
                {
                    CreatedDate = DateTime.Now,
                    Message = supportReplyDTO.Message,
                    Name = userDetails.FirstName,
                    UserId = userDetails.Id
                });

                support.Description = JsonConvert.SerializeObject(reportAndSupportViewModels);

                support.Status = supportReplyDTO.StatusId;

                support.UpdatedDate = DateTime.Now;

                db.Entry(support).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Ok(supportReplyDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [System.Web.Http.Route("api/AdminReportReply")]
        public async Task<IHttpActionResult> AdminReportReply(SupportReplyDTO supportReplyDTO)
        {
            try
            {
                var reports = await db.Reports.FindAsync(supportReplyDTO.Id);

                List<ReportAndSupportViewModel> reportAndSupportViewModels = new List<ReportAndSupportViewModel>();

                reportAndSupportViewModels.AddRange(JsonConvert.DeserializeObject<List<ReportAndSupportViewModel>>(reports.Description));

                reportAndSupportViewModels.Add(new ReportAndSupportViewModel()
                {
                    CreatedDate = DateTime.Now,
                    Message = supportReplyDTO.Message,
                    Name = userDetails.FirstName,
                    UserId = userDetails.Id
                });

                reports.Description = JsonConvert.SerializeObject(reportAndSupportViewModels);

                reports.Status = supportReplyDTO.StatusId;

                reports.UpdatedOn = DateTime.Now;

                reports.UpdatedBy = userDetails.Id;

                db.Entry(reports).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return Ok(supportReplyDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Http.Route("api/SubmitSupportDetails")]
        public async Task<IHttpActionResult> SubmitSupportDetails(SupportDetails supportDetails)
        {
            try
            {
                List<ReportAndSupportViewModel> reportAndSupportViewModels = new List<ReportAndSupportViewModel>()
                {
                     new ReportAndSupportViewModel()
                     {
                         CreatedDate = DateTime.Now,
                         Message = supportDetails.Description,
                         Name = userDetails.FirstName,
                         UserId = userDetails.Id
                     }
                };
                
                SupportTable supportTable = new SupportTable
                {
                    CreatedDate = DateTime.Now,
                    Description = JsonConvert.SerializeObject(reportAndSupportViewModels),
                    Status = 1,
                    SupportType = supportDetails.Type,
                    UserId = userDetails.Id,
                    UserName = userDetails.Username
                };
                db.Support.Add(supportTable);
                await db.SaveChangesAsync();
                return Ok(supportDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.Route("api/GetOpenSupportDetails")]
        public async Task<IHttpActionResult> GetOpenSupportDetails()
        {
            try
            {
                List<SupportViewDetails> supportViewDetails = new List<SupportViewDetails>();
                var list = await db.Support.Where(x => x.UserId == userDetails.Id && x.Status == 1).OrderByDescending(x => x.CreatedDate).ToListAsync();

                foreach (var a in list)
                {
                    supportViewDetails.Add(new SupportViewDetails
                    {
                        CreatedDate = a.CreatedDate,
                        id = a.id,
                        SupportType = a.SupportType,
                        UpdatedDate = a.UpdatedDate,
                        UserId = a.UserId,
                        UserName = a.UserName, 
                        reportAndSupportViewModels = ConvertJsonSupport(userDetails.Id, a.Description), 
                        Status = a.Status, 
                    });
                }
                return Ok(supportViewDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Http.Route("api/GetOpenReportDetails")]
        public async Task<IHttpActionResult> GetOpenReportDetails()
        {
            try
            {
                List<ReportViewDetails> reportViewDetails = new List<ReportViewDetails>();
                var list = await db.Reports.Where(x => x.ReportFrom == userDetails.Id && x.Status == 1).OrderByDescending(x => x.CreatedOn).ToListAsync();

                foreach (var a in list)
                {
                    reportViewDetails.Add(new ReportViewDetails
                    {
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        Date = a.Date,
                        Description = "",
                        Id = a.Id,
                        reportAndSupportViewModels = ConvertJsonSupport(userDetails.Id, a.Description),
                        ReportFrom = a.ReportFrom,
                        ReportTo = a.ReportTo,
                        ReportType = a.ReportType,
                        Status = a.Status,
                        UpdatedBy = a.UpdatedBy,
                        UpdatedOn = a.UpdatedOn,
                        userManagementFrom = a.userManagementFrom,
                        userManagementTo = a.userManagementTo

                    });
                }


                return Ok(reportViewDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ReportAndSupportViewModel> ConvertJsonSupport(int id, string description)
        {
            var list = JsonConvert.DeserializeObject<List<ReportAndSupportViewModel>>(description);
            List<ReportAndSupportViewModel> reportAndSupportViewModels = new List<ReportAndSupportViewModel>();
            foreach (var a in list)
            {
                string side = "left";
                if (id == a.UserId)
                {
                    side = "right";
                }
                a.Side = side;
                reportAndSupportViewModels.Add(a);
            }
            return reportAndSupportViewModels;
        }

        [System.Web.Http.Route("api/GetClosedSupportDetails")]
        public async Task<IHttpActionResult> GetClosedSupportDetails()
        {
            try
            {
                return Ok(await db.Support.Where(x => x.UserId == userDetails.Id && x.Status == 2).OrderByDescending(x => x.CreatedDate).ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Http.Route("api/GetClosedReportDetails")]
        public async Task<IHttpActionResult> GetClosedReportDetails()
        {
            try
            {
                return Ok(await db.Reports.Where(x => x.ReportFrom == userDetails.Id && x.Status == 2).OrderByDescending(x => x.CreatedOn).ToListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
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

        public class SupportDetails
        {
            public string Type { get; set; }
            public string Description { get; set; }
        }
    }

}