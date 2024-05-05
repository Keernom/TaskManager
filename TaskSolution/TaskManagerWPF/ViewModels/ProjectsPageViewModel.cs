using Entities.DTOs;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using TaskManagerWPF.Models;
using TaskManagerWPF.Services;

namespace TaskManagerWPF.ViewModels
{
    public class ProjectsPageViewModel : BindableBase
    {
        private AuthToken _authToken;
        private UserRequestService _userRequestService;
        private ProjectRequestService _projectRequestService;
        private CommonViewService _viewService;

        #region COMMANDS

        public DelegateCommand OpenNewProjectWndCommand { get; private set; }
        public DelegateCommand<object> OpenEditProjectWndCommand { get; private set; }
        public DelegateCommand<object> ShowProjectInfoCommand { get; private set; }

        #endregion

        public ProjectsPageViewModel(AuthToken authToken)
        {
            _authToken = authToken;
            _userRequestService = new UserRequestService();
            _projectRequestService = new ProjectRequestService();
            _viewService = new CommonViewService();

            OpenNewProjectWndCommand = new DelegateCommand(OpenNewProjectWnd);
            OpenEditProjectWndCommand = new DelegateCommand<object>(OpenEditProjectWnd);
            ShowProjectInfoCommand = new DelegateCommand<object>(ShowProjectInfo);
        }

        #region PROPERTIES

        public List<CommonDtoClient<ProjectDTO>> UserProjects
        {
            get => _projectRequestService.GetAllProjects(_authToken).projects
                .Select(p => new CommonDtoClient<ProjectDTO>(p)) .ToList();
        }

        private CommonDtoClient<ProjectDTO> _selectedProject;

        public CommonDtoClient<ProjectDTO> SelectedProject
        {
            get { return _selectedProject; }
            set 
            { 
                _selectedProject = value; 
                RaisePropertyChanged(nameof(SelectedProject));

                if (SelectedProject.Model.UsersIds != null)
                    ProjectUsers = SelectedProject.Model.UsersIds?
                        .Select(uId => _userRequestService.GetUserById(_authToken, uId).user)
                        .ToList();
            }
        }

        private List<UserDTO> _projectUsers = new();

        public List<UserDTO> ProjectUsers
        {
            get { return _projectUsers; }
            set 
            { 
                _projectUsers = value; 
                RaisePropertyChanged(nameof(ProjectUsers));
            }
        }

        #endregion

        #region METHODS
        private void OpenNewProjectWnd()
        {
            _viewService.ShowMessage(nameof(OpenNewProjectWnd));
        }

        private void OpenEditProjectWnd(object param)
        {
            SelectedProject = param as CommonDtoClient<ProjectDTO>;
            _viewService.ShowMessage(nameof(OpenEditProjectWnd));
        }

        private void ShowProjectInfo(object param)
        {
            SelectedProject = param as CommonDtoClient<ProjectDTO>;
            _viewService.ShowMessage(nameof(ShowProjectInfo));
        }
        #endregion
    }
}
