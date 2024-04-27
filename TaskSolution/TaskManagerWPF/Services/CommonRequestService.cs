using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManagerWPF.Models;

namespace TaskManagerWPF.Services
{
    public abstract class CommonRequestService
    {
        protected const string HOST = "https://localhost:7090/api";

        protected (T value, HttpStatusCode code) GetDataByUrl<T>(string url, AuthToken token, string username = null, string password = null)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";

            if (username != null && password != null)
            {
                webRequest.Method = "POST";
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                webRequest.Headers.Add("Authorization", "Basic " + encoded);
            }

            if (token.Access_Token != null)
            {
                webRequest.Headers.Add("Authorization", "Bearer " + token.Access_Token);
            }

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string responseText = sr.ReadToEnd();
                T res = JsonConvert.DeserializeObject<T>(responseText);

                return (res, response.StatusCode);
            }
        }

        protected async Task<HttpStatusCode> DoActionWithDataByUrl(HttpMethod method, string url, AuthToken authToken, string data = "")
        {
            HttpResponseMessage result = new HttpResponseMessage();
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken.Access_Token);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            result = method.Method switch
            {
                "POST" => await client.PostAsync(url, content),
                "PATCH" => await client.PatchAsync(url, content),
                "DELETE" => await client.DeleteAsync(url)
            };

            return result.StatusCode;
        }
    }
}
