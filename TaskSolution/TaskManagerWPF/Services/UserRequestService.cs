using Entities.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManagerWPF.Models;

using static System.Net.WebRequestMethods;

namespace TaskManagerWPF.Services
{
    public class UserRequestService
    {
        private const string HOST = "https://localhost:7090/api";
        private string _userController = $"{HOST}/users";

        private (T value, HttpStatusCode code) GetDataByUrl<T>(string url, AuthToken token, string username = null, string password = null)
        {
            HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(url);
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

        private async Task<HttpStatusCode> DoActionWithDataByUrl(HttpMethod method, string url, AuthToken authToken, string data = "")
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

        public (AuthToken token, HttpStatusCode code) GetToken(string username, string password)
        {
            string url = $"{HOST}/account/token";
            return GetDataByUrl<AuthToken>(url, new AuthToken() ,username, password);
        }

        public async Task<HttpStatusCode> CreateUser(AuthToken token, UserDTO user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = await DoActionWithDataByUrl(HttpMethod.Post, _userController, token, userJson);

            return result;
        }

        public (List<UserDTO> users, HttpStatusCode code) GetUsers(AuthToken token)
        {
            var users = GetDataByUrl<List<UserDTO>>(_userController + $"/all", token);
            return users;
        }

        public async Task<HttpStatusCode> DeleteUser(AuthToken token, int userId)
        {
            var result = await DoActionWithDataByUrl(HttpMethod.Delete, _userController + $"/{userId}", token);

            return result;
        }

        public async Task<HttpStatusCode> CreateMultipleUsers(AuthToken token, List<UserDTO> users)
        {
            string usersJson = JsonConvert.SerializeObject(users);
            var result = await DoActionWithDataByUrl(HttpMethod.Post, _userController + "/mCreate", token, usersJson);

            return result;
        }

        public async Task<HttpStatusCode> UpdateUser(AuthToken token, UserDTO user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = await DoActionWithDataByUrl(HttpMethod.Patch, _userController + $"/{user.Id}", token, userJson);

            return result;
        }
    }
}
