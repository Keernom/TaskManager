using Entities.DTOs;
using Entities.Models;
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
    public class TaskRequestService : CommonRequestService
    {
        private string _taskController = $"{HOST}/task";

        public (List<TaskDTO> tasks, HttpStatusCode code) GetTasksByDeskId(AuthToken token, int deskId)
        {
            var tasks = GetDataByUrl<List<TaskDTO>>(_taskController + $"?deskId={deskId}", token);
            return tasks;
        }

        public (List<TaskDTO> tasks, HttpStatusCode code) GetUsersTasks(AuthToken token)
        {
            var tasks = GetDataByUrl<List<TaskDTO>>(_taskController, token);
            return tasks;
        }

        public (TaskDTO task, HttpStatusCode code) GetTaskById(AuthToken token, int taskId)
        {
            var desk = GetDataByUrl<TaskDTO>(_taskController + $"/{taskId}", token);
            return desk;
        }

        public async Task<HttpStatusCode> CreateTask(AuthToken token, TaskDTO task)
        {
            string taskJson = JsonConvert.SerializeObject(task);
            var result = await DoActionWithDataByUrl(HttpMethod.Post, _taskController, token, taskJson);
            return result;
        }

        public async Task<HttpStatusCode> UpdateTask(AuthToken token, TaskDTO task)
        {
            string taskJson = JsonConvert.SerializeObject(task);
            var result = await DoActionWithDataByUrl(HttpMethod.Patch, _taskController + $"/{task.Id}", token, taskJson);
            return result;
        }

        public async Task<HttpStatusCode> DeleteTask(AuthToken token, int taskId)
        {
            var result = await DoActionWithDataByUrl(HttpMethod.Delete, _taskController + $"/{taskId}", token);
            return result;
        }
    }
}
