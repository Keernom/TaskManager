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

namespace TaskManagerWPF.Services
{
    public class UserRequestService : CommonRequestService
    {   
        private string _userController = $"{HOST}/users";

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
