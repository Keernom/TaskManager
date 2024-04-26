using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManagerWPF.Models;

using static System.Net.WebRequestMethods;

namespace TaskManagerWPF.Services
{
    public class UserRequestService
    {
        private const string HOST = "https://localhost:7090/api";

        private T GetDataByUrl<T>(string url, string username, string password)
        {
            HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(url);
            webRequest.Method = "POST";

            if (username != null && password != null)
            {
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                webRequest.Headers.Add("Authorization", "Basic " + encoded);

                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseText = sr.ReadToEnd();
                    T res = JsonConvert.DeserializeObject<T>(responseText);
                    return res;
                }
            }

            return default(T);
        }

        public AuthToken GetToken(string username, string password)
        {
            string url = $"{HOST}/account/token";
            return GetDataByUrl<AuthToken>(url, username, password);
        }
    }
}
