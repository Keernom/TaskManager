using Entities.DTOs;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManagerWPF.Models;
using TaskManagerWPF.Services;
using TaskManagerWPF.Views;

namespace TaskManagerWPF.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private string _cachePath = Path.GetTempPath() + "userTaskManager.txt";
        private UserRequestService _userRequestService;
        private CommonViewService _commonViewService;
        private Window _currentWindow;

        #region COMMANDS

        public DelegateCommand<object> GetUserFromDBCommand { get; private set; }
        public DelegateCommand<object> LoginFromCacheCommand { get; private set; }

        #endregion

        public LoginViewModel()
        {
            GetUserFromDBCommand = new DelegateCommand<object>(GetUserFromDB);
            LoginFromCacheCommand = new DelegateCommand<object>(LoginFromCache);

            CurrentUserCache = GetUserCache();
            _userRequestService = new UserRequestService();
            _commonViewService = new CommonViewService();
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

        private UserCache _currentUserCache;

        public UserCache CurrentUserCache
        {
            get => _currentUserCache;
            set 
            { 
                _currentUserCache = value; 
                RaisePropertyChanged(nameof(CurrentUserCache));
            }
        }

        #endregion

        #region METHODS

        private void GetUserFromDB(object parameter)
        {
            var passbox = parameter as PasswordBox;
            UserPassword = passbox.Password;
            _currentWindow = Window.GetWindow(passbox);

            bool isNewUser = false;
            if (UserLogin != CurrentUserCache?.Login || UserPassword != CurrentUserCache?.Password)
            {
                isNewUser = true;
            }

            var res = _userRequestService.GetToken(UserLogin, UserPassword);

            if (res.code == System.Net.HttpStatusCode.OK)
            {
                AuthToken = res.token;
                UserDTO userDTO = _userRequestService.GetUser(AuthToken).user;

                if (isNewUser)
                {
                    var saveUserCacheMessage = MessageBox.Show("Хотите сохранить логин и пароль?", "Сохранение данных", MessageBoxButton.YesNo);

                    if (saveUserCacheMessage == MessageBoxResult.Yes)
                    {
                        UserCache userCache = new UserCache()
                        {
                            Login = UserLogin,
                            Password = UserPassword,
                        };

                        CreateUserCache(userCache);
                    }
                }

                CurrentUser = userDTO;
                OpenMainWindow();
            }
        }

        private void CreateUserCache(UserCache userCache)
        {
            string jsonUser = JsonConvert.SerializeObject(userCache);

            using (StreamWriter sw = new StreamWriter(_cachePath, false, Encoding.Default))
            {
                sw.Write(jsonUser);
                _commonViewService.ShowMessage("Успех!");
            }
        }

        private UserCache GetUserCache()
        {
            bool isCacheExist = File.Exists(_cachePath);
            if (!isCacheExist) return null;

            string text = File.ReadAllText(_cachePath);
            if (text.Length == 0 ) return null;

            return JsonConvert.DeserializeObject<UserCache>(text);
        }

        private void LoginFromCache(object wnd)
        {
            _currentWindow = wnd as Window;

            UserLogin = CurrentUserCache.Login;
            UserPassword = CurrentUserCache.Password;

            var res = _userRequestService.GetToken(UserLogin, UserPassword);

            if (res.code == System.Net.HttpStatusCode.OK)
            {
                AuthToken = res.token;
                UserDTO userDTO = _userRequestService.GetUser(AuthToken).user;
                CurrentUser = userDTO;

                OpenMainWindow();
            }
        }

        private void OpenMainWindow()
        {
            MainWindow window = new MainWindow();
            window.DataContext = new MainWindowViewModel(AuthToken, CurrentUser, window);
            window.Show();

            _currentWindow.Close();
        }
        #endregion
    }
}
