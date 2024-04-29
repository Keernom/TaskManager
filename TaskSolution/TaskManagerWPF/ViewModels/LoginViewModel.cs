using Entities.DTOs;
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
using TaskManagerWPF.Services;

namespace TaskManagerWPF.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        UserRequestService _userRequestService;

        #region COMMANDS

        public DelegateCommand<object> GetUserFromDBCommand { get; private set; }

        #endregion

        public LoginViewModel()
        {
            GetUserFromDBCommand = new DelegateCommand<object>(GetUserFromDB);
            _userRequestService = new UserRequestService();
        }

        #region PROPERTIES

        public string UserLogin { get; set; }
        public string UserPassword { get; private set; }

        private UserDTO _currentUser;
        public UserDTO CurrentUser
        {
            get => _currentUser;
            set 
            { 
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private AuthToken _authToken;
        public AuthToken AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                RaisePropertyChanged(nameof(AuthToken));
            }
        }

        #endregion

        #region METHODS

        private void GetUserFromDB(object parameter)
        {
            var passbox = parameter as PasswordBox;
            UserPassword = passbox.Password;

            var res = _userRequestService.GetToken(UserLogin, UserPassword);

            if (res.code == System.Net.HttpStatusCode.OK)
            {
                AuthToken = res.token;
                UserDTO userDTO = _userRequestService.GetUser(AuthToken).user;
                MessageBox.Show(userDTO.FirstName);
            }
            
        }
        #endregion
    }
}
