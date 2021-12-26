using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PushNotificationPOC.Controllers
{
    
    [System.Web.Http.Route("[controller]")]
    public class PushNotificationController : ApiController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<PushNotificationController> _logger;

        private NotificationHubClient hub;

        public PushNotificationController(ILogger<PushNotificationController> logger)
        {
            _logger = logger;

            // Initialize the Notification Hub
            hub = NotificationHubClient.CreateClientFromConnectionString(
                "Endpoint=sb://testnotificationnscr.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=gc7zIsL8q82P0fl4ArcXnbSozbNKE6DaYmR3i5I2W/0="
                //"Endpoint=sb://testnotificationnscr.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=UwLo17cdl9UYCO7j8G/Bp24xNhQSaITpuRyafG2HR9I="
                , "testnotificationhub", true);
        }

        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> Get()
        {

            var regisrations = await hub.GetAllRegistrationsAsync(2);

            Installation installation = new Installation();
            installation.InstallationId = "testInstallation";
            installation.PushChannel = "sampleTOken";
            // In the backend we can control if a user is allowed to add tags

            installation.Tags = new List<string>();
            installation.Tags.Add("username:" + "test");

            var platform = "fcm";

            switch (platform)
            {
                case "mpns":
                    installation.Platform = NotificationPlatform.Mpns;
                    break;
                case "wns":
                    installation.Platform = NotificationPlatform.Wns;
                    break;
                case "apns":
                    installation.Platform = NotificationPlatform.Apns;
                    break;
                case "fcm":
                    installation.Platform = NotificationPlatform.Fcm;
                    break;
                default:
                    installation.Platform = NotificationPlatform.Fcm;
                    break;
            }

            await hub.CreateOrUpdateInstallationAsync(installation);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("UpdateHandle")]
        // Custom API on the backend
        public async Task<HttpResponseMessage> UpdateHandle()
        {
            Installation installation = new Installation();
            installation.InstallationId = "newInstallation";
            installation.PushChannel = "cYq-M_KjTAiaHoPwVx8XOX:APA91bF1vQzvEZS1AS17XmHJVu3ZDLOgQFZjHXRWKwXFGIYcxTRxaaOxC6nsiEkiRZ2vyS5_9LSkVkHPRGoBkL5UXSTk1kC6rjO-PHxrzQqIE1pyV9EZ3LC2qlu070fJ6n4Q7ss53uss";
            // In the backend we can control if a user is allowed to add tags

            installation.Tags = new List<string>();
            installation.Tags.Add("user:test");

            //installation.Templates = new Dictionary<string, InstallationTemplate>()

            var platform = "fcm";

            switch (platform)
            {
                case "mpns":
                    installation.Platform = NotificationPlatform.Mpns;
                    break;
                case "wns":
                    installation.Platform = NotificationPlatform.Wns;
                    break;
                case "apns":
                    installation.Platform = NotificationPlatform.Apns;
                    break;
                case "fcm":
                    installation.Platform = NotificationPlatform.Fcm;
                    break;
                default:
                    installation.Platform = NotificationPlatform.Fcm;
                    break;
            }

            await hub.CreateOrUpdateInstallationAsync(installation);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("SendNotification")]
        public async Task<HttpResponseMessage> Post([System.Web.Http.FromBody] Message message)
        {
            var user = "test";//HttpContext.Current.User.Identity.Name;
            string[] userTag = new string[1];
            userTag[0] = "oid";
            //userTag[1] = "from:" + user;
            //var message = "hello test";
            var pns = "FCM";

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            switch (pns.ToLower())
            {
                case "wns":
                    // Windows 8.1 / Windows Phone 8.1
                    var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" +
                                "From " + user + ": " + message + "</text></binding></visual></toast>";
                    outcome = await hub.SendWindowsNativeNotificationAsync(toast, userTag);
                    break;
                case "apns":
                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"" + "From " + user + ": " + message + "\"}}";
                    outcome = await hub.SendAppleNativeNotificationAsync(alert, userTag);
                    break;
                case "fcm":
                    // Android
                    var notif = "{\"notification\":{\"title\":\"" + message.Title + "\",\"body\":\"" + message.Body + "\"}, \"data\" : {\"message\":\"" + "From " + user + ": " + message + "\"}}";
                    outcome = hub.SendFcmNativeNotificationAsync(notif, userTag).Result;
                    break;
            }

            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }
            }

            return new HttpResponseMessage(ret);
        }

    }

    public class Message
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}