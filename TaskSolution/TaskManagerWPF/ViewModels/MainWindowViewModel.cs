﻿using Entities.DTOs;
using Entities.Enums;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerWPF.Models;

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

        public MainWindowViewModel(AuthToken authToken, UserDTO user)
        {
            AuthToken = authToken;
            User = user;

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
        }

        #region PROPERTIES

        private readonly string _userInfoBtnName = "Личный кабинет";
        private readonly string _userTasksBtnName = "Задачи";
        private readonly string _userDesksBtnName = "Доски";
        private readonly string _userProjectsBtnName = "Проекты";
        private readonly string _logoutBtnName = "Выход";

        private readonly string _manageUsersBtnName = "Пользователи";

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


        #endregion

        #region METHODS

        private void OpenInfoPage()
        {
            ShowMessage(_userInfoBtnName);
        }

        private void OpenTasksPage()
        {
            ShowMessage(_userTasksBtnName);
        }

        private void OpenDesksPage()
        {
            ShowMessage(_userDesksBtnName);
        }

        private void OpenProjectsPage()
        {
            ShowMessage(_userProjectsBtnName);
        }

        private void Logout()
        {
            ShowMessage(_logoutBtnName);
        }

        private void OpenManageUsersPage()
        {
            ShowMessage(_manageUsersBtnName);
        }

        #endregion

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
