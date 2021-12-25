using GCBS_INTERNAL.Controllers.API;
using GCBS_INTERNAL.Models;
using GCBS_INTERNAL.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.Chat
{
    [CustomAuthorize]
    public class GcbsChatController : BaseApiController
    {
        private DatabaseContext db;
        public GcbsChatController()
        {
            db = new DatabaseContext();
        }

        [Route("api/GetChatNotificationUserList")]
        [HttpGet]
        public IHttpActionResult GetChatNotificationUserList()
        {
            List<ChatUserListViewModel> list = new List<ChatUserListViewModel>();
            try
            {
                if (userDetails.RoleId == 3)
                {
                    list = GetChatPartnerListViewModel();
                }
                else if (userDetails.RoleId == 9)
                {
                    list = GetChatCustomerListViewModel();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(list);
        }

        [Route("api/GetChatList/limit/{limit}/to/{to}")]
        [HttpGet]
        public IHttpActionResult GetChatList(int limit, int to)
        {
            List<string> list = new List<string>();
            try
            {
                int limits = limit > 0 ? limit : 50;

                var lists = GetChatLists(to, limits);

                list = ConvertToString(lists);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(list);
        }
        [Route("api/SendNotification/message/{message}/to/{to}")]
        [HttpGet]
        public IHttpActionResult SendNotification(string message, int to)
        {

            try
            {
                using(var db2 = new DatabaseContext())
                {
                    var i = db2.ChatNotifications.Where(x => x.ToUser == userDetails.Id && x.FromUser == to).ToList();
                    foreach (var e in i)
                    {
                        e.IsRead = true;
                        db.Entry(e).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    db2.Dispose();
                }
               

                db.ChatNotifications.Add(new ChatNotification
                {
                    CreatedAt = DateTime.Now,
                    FromUser = userDetails.Id,
                    ToUser = to,
                    IsDeleted = false,
                    IsRead = false,
                    Message = message
                });
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok();
        }

        private List<string> ConvertToString(List<ChatList> lists)
        {
            try
            {
                List<string> strin = new List<string>();

                List<DateTime> groupdate = lists.OrderBy(x => x.GroupDate).Select(x => x.GroupDate).Distinct().ToList();

                foreach (DateTime dateTime in groupdate)
                {

                    var lis = lists.OrderBy(x => x.CreatedAt).Where(x => x.GroupDate == dateTime).ToList();
                    strin.Add(" <li class=\"chat-date\">" + lis.FirstOrDefault().GroupDateString + "</li>");
                    string sta = "";
                    int i = 0;
                    string query = "";
                    int cou = 1;
                    int count = lis.Count();
                    foreach (var li in lis)
                    {
                        if (i == 0)
                        {
                            sta = li.status;
                        }
                        if (sta == li.status)
                        {
                            i++;
                            query += htmlmsginfo.Replace("{time}", li.Time).Replace("{text}", li.Message);
                        }
                        else
                        {
                            strin.Add(htmlmsgbody.Replace("{status}", sta).Replace("{user}", sta + "userImage").Replace("{msginfo}", query));
                            query = "";
                            i = 0;
                            sta = li.status;
                            query += htmlmsginfo.Replace("{time}", li.Time).Replace("{text}", li.Message);
                        }
                        if (cou == count)
                        {
                            strin.Add(htmlmsgbody.Replace("{status}", sta).Replace("{user}", sta + "userImage").Replace("{msginfo}", query));
                        }
                        cou++;
                    }
                }
                return strin;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private readonly string htmlmsginfo = "<div class=\"msg-box\">" +
            "            <div>  <p>{text}</p>   <ul class=\"chat-msg-info\"> " +
            "                <li>" +
            "                  <div class=\"chat-time\"> " +
            "            <span>{time}</span> </div> </li> </ul></div> </div>";

        private readonly string htmlmsgbody = "<li class=\"media {status}\"><div class=\"avatar\">" +
            //"<img src={{{user}}} alt=\"User Image\" class=\"avatar-img rounded-circle\">" +
            "        </div>        <div class=\"media-body\">{msginfo} </div>  </li>";

        public class ChatList
        {
            public long Id { get; set; }
            public int FromUser { get; set; }

            public int ToUser { get; set; }

            public string Message { get; set; }
            public string status { get; set; }
            public DateTime CreatedAt { get; set; }

            public DateTime GroupDate
            {
                get
                {
                    return CreatedAt.Date;
                }
            }

            public string GroupDateString
            {
                get
                {
                    return GroupDate == DateTime.Now.Date ? "Today" : GroupDate == DateTime.Now.Date.AddDays(-1) ? "Yesterday" : GroupDate.ToString("dd-MMM-yyyy");
                }
            }

            public string Time => TimeCalculate2(CreatedAt);

            private string TimeCalculate2(DateTime createdAt)
            {


                return $"{createdAt.ToString("hh:mm tt")}";



            }
        }

        private List<ChatUserListViewModel> GetChatCustomerListViewModel()
        { 
            var list = db.Database.SqlQuery<ChatUserListViewModel>($"exec Sproc_ChatUserList_customer {userDetails.Id}, {userDetails.RoleId}").ToList();
            return list;

        }
        private List<ChatUserListViewModel> GetChatPartnerListViewModel()
        {
            
            var list = db.Database.SqlQuery<ChatUserListViewModel>($"exec Sproc_ChatUserList_partner {userDetails.Id}, {userDetails.RoleId}").ToList();
            return list;

        }

        private List<ChatList> GetChatLists(int toids, int limits = 50)
        { 
            var list = db.Database.SqlQuery<ChatList>($"exec GetChatList {limits},{userDetails.Id}, {toids}").ToList();
            return list;

        }
    
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            try // Necesarry unfotunately.
            {
                var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();

                var properties = typeof(T).GetProperties();

                return dt.AsEnumerable().Select(row =>
                {
                    var objT = Activator.CreateInstance<T>();

                    foreach (var pro in properties)
                    {
                        if (columnNames.Contains(pro.Name))
                        {
                            if (row[pro.Name].GetType() == typeof(System.DBNull)) pro.SetValue(objT, null, null);
                            else pro.SetValue(objT, row[pro.Name], null);
                        }
                    }

                    return objT;
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to write data to list. Often this occurs due to type errors (DBNull, nullables), changes in SP's used or wrongly formatted SP output.");
                Console.WriteLine("ConvertToList Exception: " + e.ToString());
                return new List<T>();
            }
        }
        public class ChatUserListViewModel
        {
            public int UserId { get; set; }
            public string Image { get; set; }
            public string LastMessage { get; set; }

            public DateTime? createdAt { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string Time => TimeCalculate(createdAt);
            public int readCount { get; set; }
            public bool Online { get; set; }
        }

        private static string TimeCalculate(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                var currentDate = DateTime.Now;
                var total = (DateTime)currentDate - (DateTime)dateTime;
                var totalMinutes = (int)total.TotalMinutes;

                if (totalMinutes < 59)
                {
                    return $"{totalMinutes} ago";
                }
                if (totalMinutes < 1439)
                {
                    return $"{(totalMinutes / 60)} hours ago";
                }

                if (totalMinutes < 2879)
                {
                    return $"yesterday";
                }

                if (totalMinutes < 4319)
                {
                    return $"1 days ago";
                }


                return $"{((DateTime)dateTime).ToString("dd-MM-yyyy")}";


            }
            else
            {
                return "";
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
