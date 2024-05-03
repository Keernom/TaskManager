using Entities.DTOs;
using Entities.Enums;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManagerWPF.Models;
using TaskManagerWPF.Views;
using TaskManagerWPF.Views.Pages;

namespace TaskManagerWPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region COMMANDS

        public DelegateCommand OpenInfoPageCommand { get; private set; }
        public DelegateCommand OpenTasksPageCommand { get; private set; }
        public DelegateCommand OpenDesksPageCommand { get; private set; }
        public DelegateCommand OpenProjectsPageCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }

        public DelegateCommand OpenManageUsersPageCommand { get; private set; }

        #endregion

        public MainWindowViewModel(AuthToken authToken, UserDTO user, Window currentWindow = null)
        {
            AuthToken = authToken;
            User = user;
            _currentWindow = currentWindow;

            OpenInfoPageCommand = new DelegateCommand(OpenInfoPage);
            NavButtons.Add(_userInfoBtnName, OpenInfoPageCommand);

            OpenTasksPageCommand = new DelegateCommand(OpenTasksPage);
            NavButtons.Add(_userTasksBtnName, OpenTasksPageCommand);

            OpenDesksPageCommand = new DelegateCommand(OpenDesksPage);
            NavButtons.Add(_userDesksBtnName, OpenDesksPageCommand);

            OpenProjectsPageCommand = new DelegateCommand(OpenProjectsPage);
            NavButtons.Add(_userProjectsBtnName, OpenProjectsPageCommand);

            if (User.Status == UserStatus.Admin) 
            {
                OpenManageUsersPageCommand = new DelegateCommand(OpenManageUsersPage);
                NavButtons.Add(_manageUsersBtnName, OpenManageUsersPageCommand);
            }

            LogoutCommand = new DelegateCommand(Logout);
            NavButtons.Add(_logoutBtnName, LogoutCommand);

            OpenInfoPage();
        }

        #region PROPERTIES

        private readonly string _userInfoBtnName = "Личный кабинет";
        private readonly string _userTasksBtnName = "Задачи";
        private readonly string _userDesksBtnName = "Доски";
        private readonly string _userProjectsBtnName = "Проекты";
        private readonly string _logoutBtnName = "Выход";

        private readonly string _manageUsersBtnName = "Пользователи";

        private Window _currentWindow;

        private Dictionary<string, DelegateCommand> _navButtons = new();

        public Dictionary<string, DelegateCommand> NavButtons
        {
            get => _navButtons; 
            set 
            { 
                _navButtons = value; 
                RaisePropertyChanged(nameof(NavButtons));
            }
        }

        private AuthToken _authToken;

        public AuthToken AuthToken
        {
            get { return _authToken; }
            private set
            { 
                _authToken = value; 
                RaisePropertyChanged(nameof(AuthToken));
            }
        }

        private UserDTO _user;

        public UserDTO User
        {
            get { return _user; }
            private set 
            { _user = value; 
                RaisePropertyChanged(nameof(User));
            }
        }

        private string _selectedPageName;

        public string SelectedPageName
        {
            get { return _selectedPageName; }
            set 
            { 
                _selectedPageName = value; 
                RaisePropertyChanged(nameof(SelectedPageName));
            }
        }


        private Page _selectedPage;

        public Page SelectedPage
        {
            get { return _selectedPage; }
            set 
            { 
                _selectedPage = value; 
                RaisePropertyChanged(nameof(SelectedPage));
            }
        }
        #endregion

        #region METHODS

        private void OpenInfoPage()
        {
            Page page = new UserInfoPage();
            OpenPage(page, _userInfoBtnName, this);
        }

        private void OpenTasksPage()
        {
            Page page = new UserTasksPage();
            OpenPage(page, _userTasksBtnName, new UserTasksPageViewModel(AuthToken));
        }

        private void OpenDesksPage()
        {
            SelectedPageName = _userDesksBtnName;
            ShowMessage(_userDesksBtnName);
        }

        private void OpenProjectsPage()
        {
            SelectedPageName = _userProjectsBtnName;
            ShowMessage(_userProjectsBtnName);
        }

        private void Logout()
        {
            var question = MessageBox.Show("Вы уверены?", "Выход", MessageBoxButton.YesNo);

            if (question == MessageBoxResult.Yes && _currentWindow != null)
            {
                Login loginWindow = new Login();
                loginWindow.Show();

                _currentWindow.Close();
            }
        }

        private void OpenManageUsersPage()
        {
            SelectedPageName = _manageUsersBtnName;
            ShowMessage(_manageUsersBtnName);
        }

        #endregion

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void OpenPage(Page page, string pageName, BindableBase viewModel)
        {
            SelectedPageName = pageName;
            SelectedPage = page;
            SelectedPage.DataContext = viewModel;
        }
    }
}
