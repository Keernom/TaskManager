using Entities.DTOs;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using TaskManagerWPF.Models;
using TaskManagerWPF.Services;

namespace TaskManagerWPF.ViewModels
{
    public class UserTasksPageViewModel : BindableBase
    {
        private AuthToken _authToken;
        private TaskRequestService _taskService;
        private UserRequestService _userService;

        public UserTasksPageViewModel(AuthToken token)
        {
            _authToken = token;
            _taskService = new TaskRequestService();
            _userService = new UserRequestService();
        }

        public List<TaskClient> AllTasks
        {
            get => _taskService.GetUsersTasks(_authToken).tasks
                .Select(ToTaskClient).ToList();
        }

        private TaskClient ToTaskClient(TaskDTO task)
        {
            TaskClient result = new TaskClient(task);

            var creatorResponse = _userService.GetUserById(_authToken, task.CreatorId);

            if (creatorResponse.code == System.Net.HttpStatusCode.OK)
            {
                result.Creator = creatorResponse.user;
            }

            var executorResponse = _userService.GetUserById(_authToken, task.ExecutorId);

            if (executorResponse.code == System.Net.HttpStatusCode.OK)
            {
                result.Executor = executorResponse.user;
            }

            return result;
        }
    }
}
