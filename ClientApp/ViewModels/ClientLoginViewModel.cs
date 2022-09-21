﻿using AdminApp.ViewModels;
using ClientApp.Views;
using Newtonsoft.Json;
using Shared;
using Shared.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ClientApp.ViewModels
{
    public class ClientLoginViewModel : BaseViewModel
    {
        Frame rootFrame = Window.Current.Content as Frame;
        public ClientLoginViewModel()
        {

        }

        #region Visibility Properties

        private Visibility _loginBtnGridVis = Visibility.Visible;
        private Visibility _loginProgressRingVis = Visibility.Collapsed;
        private Visibility _loginErrorMessageVis = Visibility.Collapsed;

        public Visibility LoginBtnGridVis
        {
            get { return _loginBtnGridVis; }
            set
            {
                _loginBtnGridVis = value;
                OnPropertyChanged("LoginBtnGridVis");
            }
        }
        public Visibility LoginProgressRingVis
        {
            get { return _loginProgressRingVis; }
            set
            {
                _loginProgressRingVis = value;
                OnPropertyChanged("LoginProgressRingVis");
            }
        }
        public Visibility LoginErrorMessageVis
        {
            get { return _loginErrorMessageVis; }
            set
            {
                _loginErrorMessageVis = value;
                OnPropertyChanged("LoginErrorMessageVis");
            }
        }
        #endregion

        #region Elements Properties

        private string _userName = "";
        private string _userPassword = "";
        private string _loginErrorMessage = "";
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value; OnPropertyChanged("UserName");
            }
        }
        public string UserPassword
        {
            get { return _userPassword; }
            set
            { _userPassword = value; OnPropertyChanged("UserPassword"); }
        }
        public string LoginErrorMessage
        {
            get { return _loginErrorMessage; }
            set
            { _loginErrorMessage = value; OnPropertyChanged("LoginErrorMessage"); }
        }
        #endregion

        #region Commands

        public ICommand LoginCommand
        {
            get { return new DelegateCommand(DoLogin); }
        }

        #endregion

        #region Functions
        public void DoLogin()
        {
            Login();
        }

        public async void Login()
        {
            LoginModel login = new LoginModel();
            try
            {

                AdminLoginModel model = new AdminLoginModel()
                {
                    UserName = UserName,
                    UserPassword = UserPassword,
                };
                if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.UserPassword))
                {
                    ClientLoginBtnVis();
                    _ = Task.Run(async () =>
                    {
                        ResponseBody response = new ResponseBody();
                        response = await StaticContext.SignInApiUrl.PostAsync<ResponseBody>(model);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            if (response.IsValidated)
                            {
                                var Userlogin = JsonConvert.DeserializeObject<UserModal>(response.Data);
                                login.AccessToken = Userlogin.Token;
                                login.UserId = Userlogin.UserId;
                                StaticContext.PoliceStation = Userlogin.PoliceStation;

                                login.LoginTime = DateTime.Now;
                                StaticContext.UserId = login.UserId;
                                RealmDb<LoginModel>.Add(login);
                                ClientLoginBtnVis();
                                rootFrame.Navigate(typeof(ClientDashboard));


                            }
                            else
                            {
                                ClientLoginBtnVis();
                                ValidationMsg();
                                LoginErrorMessage = response.Message;
                                CloseMessageTimer();
                                return;
                            }
                        }).AsTask();
                    });
                }
                else
                {
                    ValidationMsg();
                    LoginErrorMessage = "Value cannot be empty!";
                    CloseMessageTimer();
                    return;
                }

            }
            catch (Exception ex)
            {
                ClientLoginPasswordVis();
                LoginErrorMessage = ex.Message;
                CloseMessageTimer();
                return;
            }
        }

        private void ValidationMsg()
        {
            LoginProgressRingVis = Visibility.Collapsed;
            LoginBtnGridVis = Visibility.Visible;
            LoginErrorMessageVis = Visibility.Visible;
        }
        private void CloseMessageTimer()
        {
            DispatcherTimer responseTimer = new DispatcherTimer();
            responseTimer.Tick += (senders, args) =>
            {
                LoginErrorMessageVis = Visibility.Collapsed;
                LoginErrorMessage = "";
                responseTimer.Stop();
            };
            responseTimer.Interval = TimeSpan.FromSeconds(5);
            responseTimer.Start();
        }

        private void ClientLoginPasswordVis()
        {
            LoginProgressRingVis = Visibility.Collapsed;
            LoginBtnGridVis = Visibility.Visible;
            LoginErrorMessageVis = Visibility.Visible;
        }
        private void ClientLoginBtnVis()
        {
            LoginBtnGridVis = Visibility.Collapsed;
            LoginProgressRingVis = Visibility.Visible;
        }

        #endregion
    }
}
