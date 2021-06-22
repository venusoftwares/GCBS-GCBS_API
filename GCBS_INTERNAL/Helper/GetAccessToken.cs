using GCBS_INTERNAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace GCBS_INTERNAL.Helper
{
    public class GetAccessToken
    {
        public async Task<string> GetToken(UserManagement userManagement)
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            url = url.Remove(url.LastIndexOf("/") + 1);
            string json = JsonConvert.SerializeObject(userManagement);
            string accessToken = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url + "token");

                // We want the response to be JSON.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Build up the data to POST.
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("grant_type", "password"));
                postData.Add(new KeyValuePair<string, string>("username", json));
                postData.Add(new KeyValuePair<string, string>("password", json));
                FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                // Post to the Server and parse the response.
                HttpResponseMessage response = await client.PostAsync("Token", content);
                string jsonString = await response.Content.ReadAsStringAsync();
                object responseData = JsonConvert.DeserializeObject(jsonString);

                // return the Access Token.
                accessToken = ((dynamic)responseData).access_token;
            }
            return accessToken;
        }
    }
}