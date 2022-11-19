using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cowboy.APIService.DataAccess.Helpers
{
    public static class HttpClientHelper
    {
        public static string DoHttpRequest(string httpMethod, string baseURI, string requestParam, string data = "")
        {
            string responseString = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add(Constants.ApiKey, "YvKMLhmwvKNj1gOAYqZMo10K4Fbmn759");
                HttpRequestMessage request = null;
                if (httpMethod == "GET")
                {
                    request = new HttpRequestMessage(HttpMethod.Get, requestParam);
                }
                if (httpMethod == "POST")
                {
                    request = new HttpRequestMessage(HttpMethod.Post, requestParam);
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                }
                var response = client.SendAsync(request).GetAwaiter().GetResult();
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            return responseString;
        }
    }
}
