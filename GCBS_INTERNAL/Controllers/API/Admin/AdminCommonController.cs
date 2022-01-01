using GCBS_INTERNAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API.Admin
{
    public class AdminCommonController : BaseApiController
    {
        [Route("api/UpdateUserAccessStatus/UserId/{userid}/Status/{status}")]
        [HttpGet]
        public IHttpActionResult UpdateUserAccessStatus(int userid, int status)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    UserManagement userManagement = db.UserManagement.Find(userid);
                    userManagement.AccessStatus = status;
                    //if (status == 1)
                    //{
                    //    userManagement.Status = true;
                    //}
                    //if (status == 2)
                    //{
                    //    userManagement.Status = false;
                    //}
                    //userManagement.AccessStatus = status;
                    db.Entry(userManagement).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Route("api/ChatLogHistory")]
        [HttpGet]
        public IHttpActionResult ChatLogHistory()
        {
            using (var db = new DatabaseContext())
            {
                var chat = db.CustomerBooking
                    .Include(x => x.UserManagement)
                    .Include(x => x.CustomerManagement)
                    .Select(x => new ChatLogViewModel
                    {
                        ProviderId = x.ProviderId,
                        CustomerId = x.CustomerId,
                        ProviderDetails = x.UserManagement,
                        CustomerDetails = x.CustomerManagement
                    }).Distinct().ToList();
               
                return Ok(chat);

            }

        }
        [Route("api/ChatLogDetails/{providerId}/{customerId}")]
        [HttpGet]
        public IHttpActionResult ChatLogDetails(int providerId, int customerId)
        {
            using (var db = new DatabaseContext())
            {
                var chat = db.ChatNotifications
                    .Where(x => (x.FromUser == providerId && x.ToUser == customerId) || (x.ToUser == providerId && x.FromUser == customerId))
                    .OrderBy(x => x.CreatedAt).ToList(); 
                 
                return Ok(chat.Select(x => new MessageViewModel
                {
                    Side = x.FromUser == customerId ? "left" : "right",
                    Message = x.Message,
                    Time = x.CreatedAt.ToString("dd-MM-yyyy hh:mm tt")

                }).ToList());
            }

        }
        public class MessageViewModel
        {
            public string Side { get; set; }
            public string Message { get; set; }

            public string Time { get; set; }
        }

        public class ChatLogViewModel
        {
            public int ProviderId { get; set; }
            public int CustomerId { get; set; }
            public object ProviderDetails { get; set; }
            public object CustomerDetails { get; set; }
        }
    }
}
