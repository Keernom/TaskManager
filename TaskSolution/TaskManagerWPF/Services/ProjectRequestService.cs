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
    public class ProjectRequestService : CommonRequestService
    {
        private string _projectController = $"{HOST}/project";

        public (List<ProjectDTO> projects, HttpStatusCode code) GetAllProjects(AuthToken token)
        {
            var projects = GetDataByUrl<List<ProjectDTO>>(_projectController, token);
            return projects;
        }

        public (ProjectDTO project, HttpStatusCode code) GetProjectById(AuthToken token, int id)
        {
            var project = GetDataByUrl<ProjectDTO>(_projectController + $"/{id}", token);
            return project;
        }

        public async Task<HttpStatusCode> CreateProject(AuthToken token, ProjectDTO project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            var result = await DoActionWithDataByUrl(HttpMethod.Post, _projectController, token, projectJson);
            return result;
        }

        public async Task<HttpStatusCode> UpdateProject(AuthToken token, ProjectDTO project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            var result = await DoActionWithDataByUrl(HttpMethod.Patch, _projectController + $"/{project.Id}", token, projectJson);
            return result;
        }

        public async Task<HttpStatusCode> AddUsersToProject(AuthToken token, int projectId, List<int> usersId)
        {
            string usersJson = JsonConvert.SerializeObject(usersId);
            var result = await DoActionWithDataByUrl(HttpMethod.Patch, _projectController + $"/{projectId}/users", token, usersJson);
            return result;
        }

        public async Task<HttpStatusCode> RemoveUsersFromProject(AuthToken token, int projectId, List<int> usersId)
        {
            string usersJson = JsonConvert.SerializeObject(usersId);
            var result = await DoActionWithDataByUrl(HttpMethod.Patch, _projectController + $"/{projectId}/users/remove", token, usersJson);
            return result;
        }

        public async Task<HttpStatusCode> DeleteProject(AuthToken token, int projectId)
        {
            var result = await DoActionWithDataByUrl(HttpMethod.Delete, _projectController + $"/{projectId}", token);
            return result;
        }
    }
}
