using Entities.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManagerWPF.Models;

namespace TaskManagerWPF.Services
{
    public class DeskRequestService : CommonRequestService
    {
        private string _deskController = $"{HOST}/desk";

        public (List<DeskDTO> desks, HttpStatusCode code) GetUsersDesks(AuthToken token)
        {
            var desks = GetDataByUrl<List<DeskDTO>>(_deskController, token);
            return desks;
        }

        public (DeskDTO desk, HttpStatusCode code) GetDeskById(AuthToken token, int deskId)
        {
            var desk = GetDataByUrl<DeskDTO>(_deskController + $"/{deskId}", token);
            return desk;
        }

        public (List<DeskDTO> desks, HttpStatusCode code) GetDesksByProject(AuthToken token, int projectId)
        {
            var desks = GetDataByUrl<List<DeskDTO>>(_deskController + $"/project?projectId={projectId}", token);
            return desks;
        }

        public async Task<HttpStatusCode> CreateDesk(AuthToken token, DeskDTO desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            var result = await DoActionWithDataByUrl(HttpMethod.Post, _deskController, token, deskJson);
            return result;
        }

        public async Task<HttpStatusCode> UpdateDesk(AuthToken token, DeskDTO desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            var result = await DoActionWithDataByUrl(HttpMethod.Patch, _deskController + $"/{desk.Id}", token, deskJson);
            return result;
        }

        public async Task<HttpStatusCode> DeleteDesk(AuthToken token, int deskId)
        {
            var result = await DoActionWithDataByUrl(HttpMethod.Delete, _deskController + $"/{deskId}", token);
            return result;
        }
    }
}
