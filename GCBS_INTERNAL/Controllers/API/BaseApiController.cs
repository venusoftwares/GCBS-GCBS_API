﻿using GCBS_INTERNAL.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GCBS_INTERNAL.Controllers.API
{
    public class BaseApiController : ApiController
    {
        protected UserManagement userDetails = new UserManagement();
        private DatabaseContext context = new DatabaseContext();
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BaseApiController()
        {
            var response = HttpContext.Current.User.Identity.Name;
            userDetails = JsonConvert.DeserializeObject<UserManagement>(response);
            //using(var db =new DatabaseContext())
            //{
            //    var user = db.UserManagement.Find(userDetails.Id); 
            //    if(DateTime.Now <= user.LastActivateTime)
            //    {
            //        user.LastActivateTime = DateTime.Now;
            //        db.Entry(user).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }  
            //}
        }

      
    }

    public class OkXmlDownloadResult : IHttpActionResult
    {
        private readonly ApiController _controller;

        public OkXmlDownloadResult(string xml, string downloadFileName,
            ApiController controller)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            if (downloadFileName == null)
            {
                throw new ArgumentNullException("downloadFileName");
            }

            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            Xml = xml;
            ContentType = "application/xml";
            DownloadFileName = downloadFileName;
            _controller = controller;
        }

        public string Xml { get; private set; }

        public string ContentType { get; private set; }

        public string DownloadFileName { get; private set; }

        public HttpRequestMessage Request
        {
            get { return _controller.Request; }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(Xml);
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(ContentType);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = DownloadFileName
            };
            return response;
        }
    }
}
