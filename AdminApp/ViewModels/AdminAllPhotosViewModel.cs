using AdminApp.Views;
using Newtonsoft.Json;
using Shared;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdminApp.ViewModels
{
    public class AdminAllPhotosViewModel : BaseViewModel
    {
        private long totalPageSize = 0;
        private int records = 0;

        public AdminAllPhotosViewModel()
        {
            GetAllPhotos();
        }
        Frame rootFrame = Window.Current.Content as Frame;

        #region Elements Properties
        private string _newPassword = "";
        private string _adminName = "";
        private string _policeStation = StaticContext.PoliceStation;
        private string _notificationMessage = string.Empty;
        private int _pageNo = 1;
        private string _passwordStrength = "";

        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set { _notificationMessage = value; OnPropertyChanged("NotificationMessage"); }
        }

        public int PageNo
        {
            get { return _pageNo; }
            set { _pageNo = value; OnPropertyChanged("PageNo"); }
        }

        public string AdminName
        {
            get { return _adminName; }
            set
            { _adminName = value; OnPropertyChanged("AdminName"); }
        }

        public string PasswordStrength
        {
            get { return _passwordStrength; }
            set
            {
                _passwordStrength = value; OnPropertyChanged("PasswordStrength");
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {

                var data = CheckingPasswordStrength(value);
                if (data == PasswordScore.Blank)
                    PasswordStrength = "Include a number (0-9)";
                if (data == PasswordScore.charactelength)
                    PasswordStrength = "Include at least 8 characters long";
                if (data == PasswordScore.VeryWeak)
                    PasswordStrength = "Include at least 1 upper case and lower case characters (A-Z)";
                if (data == PasswordScore.Weak)
                    PasswordStrength = "Include a number (0-9) and at least 1 special character";
                if (data == PasswordScore.Medium)
                    PasswordStrength = "";
                if (data == PasswordScore.Strong)
                    PasswordStrength = "Include at least 1 special character";
                if (data == PasswordScore.VeryStrong)
                    PasswordStrength = "";

                NewPasswordValidationMsg = Visibility.Visible;
                if (string.IsNullOrEmpty(value))
                {
                    PasswordStrength = "Required";
                }
                _newPassword = value; OnPropertyChanged("NewPassword");

                /* _newPassword = value; OnPropertyChanged("NewPassword");*/
            }
        }
        #endregion
        #region Visibility Properties

        private Visibility _logoutPopupGridVis = Visibility.Collapsed;
        private Visibility _updatePasswordProgressRingVis = Visibility.Collapsed;
        private Visibility _resetPopupGridVis = Visibility.Collapsed;
        private Visibility _updatePasswordGridVis = Visibility.Visible;
        private Visibility _notificationPopupGridVis = Visibility.Collapsed;
        private Visibility _loadingBackgroundGridVis = Visibility.Collapsed;
        private Visibility _newPasswordValidationMsg = Visibility.Collapsed;
        private Visibility _noDataValidationVis = Visibility.Collapsed;
        private Visibility _listBoxVis = Visibility.Visible;
        private Visibility _generateUserBtnVis = Visibility.Visible;
        private Visibility _generateUserBtnProgressRingVis = Visibility.Collapsed;
        private Visibility _photoBtnProgressVis = Visibility.Collapsed;
        private Visibility _photoBtnProgressRingVis = Visibility.Collapsed;
        private Visibility _deleteBtnProgressRingVis = Visibility.Collapsed;
        private Visibility _deleteBtnVis = Visibility.Collapsed;
        private Visibility _disabledBtnVis = Visibility.Collapsed;
        private Visibility _disabledbackBtnVis = Visibility.Collapsed;

        public Visibility DisabledBtnVis
        {
            get { return _disabledBtnVis; }
            set
            {
                _disabledBtnVis = value;
                OnPropertyChanged("DisabledBtnVis");
            }
        }
        public Visibility DisabledbackBtnVis
        {
            get { return _disabledbackBtnVis; }
            set
            {
                _disabledbackBtnVis = value;
                OnPropertyChanged("DisabledbackBtnVis");
            }
        }
        public Visibility PhotoBtnProgressRingVis
        {
            get { return _photoBtnProgressRingVis; }
            set
            {
                _photoBtnProgressRingVis = value;
                OnPropertyChanged("PhotoBtnProgressRingVis");
            }
        }
        public Visibility PhotoBtnProgressVis
        {
            get { return _photoBtnProgressVis; }
            set
            {
                _photoBtnProgressVis = value;
                OnPropertyChanged("PhotoBtnProgressVis");
            }
        }

        public Visibility DeleteBtnVis
        {
            get { return _deleteBtnVis; }
            set
            {
                _deleteBtnVis = value;
                OnPropertyChanged("DeleteBtnVis");
            }
        }
        public Visibility DeleteBtnProgressRingVis
        {
            get { return _deleteBtnProgressRingVis; }
            set
            {
                _deleteBtnProgressRingVis = value;
                OnPropertyChanged("DeleteBtnProgressRingVis");
            }
        }
        public Visibility GenerateUserBtnVis
        {
            get { return _generateUserBtnVis; }
            set
            {
                _generateUserBtnVis = value;
                OnPropertyChanged("GenerateUserBtnVis");
            }
        }
        public Visibility GenerateUserBtnProgressRingVis
        {
            get { return _generateUserBtnProgressRingVis; }
            set
            {
                _generateUserBtnProgressRingVis = value;
                OnPropertyChanged("GenerateUserBtnProgressRingVis");
            }
        }
        public Visibility ListBoxVis
        {
            get { return _listBoxVis; }
            set
            {
                _listBoxVis = value;
                OnPropertyChanged("ListBoxVis");
            }
        }
        public Visibility NoDataValidationVis
        {
            get { return _noDataValidationVis; }
            set
            {
                _noDataValidationVis = value;
                OnPropertyChanged("NoDataValidationVis");
            }
        }

        public Visibility NewPasswordValidationMsg
        {
            get { return _newPasswordValidationMsg; }
            set
            {
                _newPasswordValidationMsg = value;
                OnPropertyChanged("NewPasswordValidationMsg");
            }
        }

        public Visibility LoadingBackgroundGridVis
        {
            get { return _loadingBackgroundGridVis; }
            set
            {
                _loadingBackgroundGridVis = value;
                OnPropertyChanged("LoadingBackgroundGridVis");
            }
        }
        public Visibility LogoutPopupGridVis
        {
            get { return _logoutPopupGridVis; }
            set
            {
                _logoutPopupGridVis = value;
                OnPropertyChanged("LogoutPopupGrid");
            }
        }

        public Visibility UpdatePasswordProgressRingVis
        {
            get { return _updatePasswordProgressRingVis; }
            set
            {
                _updatePasswordProgressRingVis = value;
                OnPropertyChanged("UpdatePasswordProgressRingVis");
            }
        }

        public Visibility ResetPopupGridVis
        {
            get { return _resetPopupGridVis; }
            set
            {
                _resetPopupGridVis = value;
                OnPropertyChanged("ResetPopupGridVis");
            }
        }
        public Visibility UpdatePasswordGridVis
        {
            get { return _updatePasswordGridVis; }
            set
            {
                _updatePasswordGridVis = value;
                OnPropertyChanged("UpdatePasswordGridVis");
            }
        }
        public Visibility NotificationPopupGridVis
        {
            get { return _notificationPopupGridVis; }
            set
            {
                _notificationPopupGridVis = value;
                OnPropertyChanged("NotificationPopupGridVis");
            }
        }

        #endregion

        #region Commands
        public ICommand LogoutCommand
        {
            get { return new DelegateCommand(UserLogout); }
        }
        public ICommand SureCommand
        {
            get { return new DelegateCommand(SureLogout); }
        }
        public ICommand DontLogoutCommand
        {
            get { return new DelegateCommand(DontLogout); }
        }
        public ICommand UsersCommand
        {
            get { return new DelegateCommand(NavigateToUsersPage); }
        }
        public ICommand AllPhotosCommand
        {
            get { return new DelegateCommand(NavigateToAllPhotosPage); }
        }

        public ICommand CancelbtnCommand
        {
            get { return new DelegateCommand(CancelPopup); }
        }
        public ICommand DeleteCommand
        {
            get
            {
                return new CommadEventHandler<string>((Code) => DeleteAllPhotos(Code));
            }

            //get { return new DelegateCommand(DeleteAllPhotos); }
        }
        public ICommand UpdateUserPasswordCommand
        {
            get { return new DelegateCommand(ResetUserPassword); }
        }

        public ICommand PreviousCommand
        {
            get { return new DelegateCommand(PreviousButton); }
        }
        public ICommand NextCommand
        {
            get { return new DelegateCommand(NextButton); }
        }


        #endregion

        #region List

        private List<AllPhotosResponseModel> _allPhotosList;

        public List<AllPhotosResponseModel> AllPhotosList
        {
            get { return _allPhotosList; }
            set { _allPhotosList = value; OnPropertyChanged("AllPhotosList"); }
        }

        #endregion

        #region Popup Properties

        private bool _previousButtonEnable = false;
        private bool _nextButtonEnable = false;
        private bool _popupOpen = false;
        private bool _notificationPopup = false;
        private bool _logoutpopupOpen = false;

        public bool PreviousButtonEnable
        {
            get { return _previousButtonEnable; }
            set { _previousButtonEnable = value; OnPropertyChanged("PreviousButtonEnable"); }
        }
        public bool NextButtonEnable
        {
            get { return _nextButtonEnable; }
            set { _nextButtonEnable = value; OnPropertyChanged("NextButtonEnable"); }
        }
        public bool PopupOpen
        {
            get { return _popupOpen; }
            set { _popupOpen = value; OnPropertyChanged("PopupOpen"); }
        }
        public bool LogoutPopupOpen
        {
            get { return _logoutpopupOpen; }
            set { _logoutpopupOpen = value; OnPropertyChanged("LogoutPopupOpen"); }
        }
        public bool NotificationPopup
        {
            get { return _notificationPopup; }
            set { _notificationPopup = value; OnPropertyChanged("NotificationPopup"); }
        }


        #endregion

        #region Functions


        private PasswordScore CheckingPasswordStrength(string password)
        {
            int score = 1;
            if (password.Length < 8)
                return PasswordScore.charactelength;
            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;

            if (password.Length == 8)
                score++;
            if (password.Length > 12)
                score++;
            if (password.Length < 12)
                score++;
            if (Regex.IsMatch(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript))   //number only //"^\d+$" if you need to match more than one digit.
                score++;
            if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript)) //both, lower and upper case
                score++;
            if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript)) //^[A-Z]+$
                score++;
            return (PasswordScore)score;
        }



        private void CancelPopup()
        {
            LoadingBackgroundGridVis = Visibility.Collapsed;
            ResetPopupGridVis = Visibility.Visible;
            PopupOpen = false;
        }
        private async void PreviousButton()
        {

            AdminPagination page = new AdminPagination();
            NextButtonEnable = true;
            PageNo = PageNo - 1;
            records = records - page.PageSize;
            page = new AdminPagination()
            {
                PageNum = PageNo,
            };

            if (PageNo <= 1)
            {
                DisabledbackBtnVis = Visibility.Visible;
                PreviousButtonEnable = false;
            }


            if (PageNo >= 1)
            {
                ResponseBody response = new ResponseBody();
                DisabledBtnVis = Visibility.Collapsed;
                response = await StaticContext.FetchPhotosApiUrl.PostAsync<ResponseBody>(page);
                if (response.IsValidated)
                {
                    //List<AllPhotosResponseModel> list = new List<AllPhotosResponseModel>();
                    List<AllPhotosResponseModel> list = JsonConvert.DeserializeObject<List<AllPhotosResponseModel>>(response.Data);
                    AllPhotosList = list;
                }
                else
                {
                    OpenNotificationPopup();
                    NotificationMessage = response.Message;
                    CloseMessageTimer();
                }
            }
        }

        private async void NextButton()
        {
            ResponseBody response = new ResponseBody();
            AdminPagination page = new AdminPagination();
            if (totalPageSize > records)
            {
                PageNo = PageNo + 1;
                page = new AdminPagination()
                {
                    PageNum = PageNo,
                };

                records = PageNo * page.PageSize;

                if (records >= totalPageSize)
                {
                    DisabledBtnVis = Visibility.Visible;
                    NextButtonEnable = false;
                }


                PreviousButtonEnable = true;
                response = await StaticContext.FetchPhotosApiUrl.PostAsync<ResponseBody>(page);
                if (response.IsValidated)
                {
                    DisabledbackBtnVis = Visibility.Collapsed;
                    List<AllPhotosResponseModel> list = new List<AllPhotosResponseModel>();
                    list = JsonConvert.DeserializeObject<List<AllPhotosResponseModel>>(response.Data);
                    AllPhotosList = list;
                }
                else
                {
                    OpenNotificationPopup();
                    NotificationMessage = response.Message;
                    CloseMessageTimer();
                }
            }
        }

        private async void ResetUserPassword()
        {
            ResponseBody response = new ResponseBody();
            try
            {
                ResetPassword resetpassword = new ResetPassword
                {
                    UserPassword = NewPassword,
                    Code = StaticContext.UserId
                };

                if (resetpassword.UserPassword.Length >= 8 && !string.IsNullOrEmpty(resetpassword.Code) && !string.IsNullOrEmpty(resetpassword.UserPassword))
                {
                    UpdatePasswordGridVis = Visibility.Collapsed;
                    UpdatePasswordProgressRingVis = Visibility.Visible;
                    _ = Task.Run(async () =>
                    {
                        response = await StaticContext.ResetPasswordApiUrl.PostAsync<ResponseBody>(resetpassword);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            if (response.IsValidated)
                            {
                                NewPassword = "";
                                CancelPopup();
                                OpenNotificationPopup();
                                NotificationMessage = response.Detail;
                                CloseMessageTimer();
                                UpdatePasswordGridVis = Visibility.Visible;
                                UpdatePasswordProgressRingVis = Visibility.Collapsed;
                            }
                            else
                            {
                                OpenNotificationPopup();
                                NotificationMessage = response.Message;
                                CloseMessageTimer();
                            }
                        }).AsTask();
                    });
                }
                else
                {
                    if (string.IsNullOrEmpty(resetpassword.UserPassword))
                    {
                        NewPasswordValidationMsg = Visibility.Visible;
                        PasswordStrength = "Required";
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        private async void DeleteAllPhotos(string Code)
        {
            try
            {
                if (!string.IsNullOrEmpty(Code))
                {
                    //DeleteBtnVis = Visibility.Collapsed;
                    //DeleteBtnProgressRingVis = Visibility.Visible;
                    _ = Task.Run(async () =>
                  {
                      ResponseBody response = new ResponseBody();
                      var api = StaticContext.AdminDeletePhotoApiUrl + Code;
                      response = await api.GetAsyncOnLoad();
                      await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                      {
                          if (response.IsValidated)
                          {
                              OpenNotificationPopup();
                              NotificationMessage = response.Detail;
                              CloseMessageTimer();
                              GetAllPhotos();
                          }
                          else
                          {
                              OpenNotificationPopup();
                              NotificationMessage = response.Message;
                              CloseMessageTimer();
                          }
                      }).AsTask();
                  });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void SureLogout()
        {
            LogoutPopupGridVis = Visibility.Collapsed;
            LogoutPopupOpen = false;
            Logout();
        }

        private void DontLogout()
        {
            LogoutPopupGridVis = Visibility.Collapsed;
            LogoutPopupOpen = false;
        }

        private void UserLogout()
        {
            LogoutPopupGridVis = Visibility.Visible;
            LogoutPopupOpen = true;

        }

        private void Logout()
        {
            if (StaticContext.UserId != "")
            {
                Frame rootFrame = Window.Current.Content as Frame;

                LoginModel model = new LoginModel()
                {
                    AccessToken = "",
                    UserId = "",
                };

                RealmDb<LoginModel>.Update(model, d => d.UserId == StaticContext.UserId);
                RealmDb<LoginModel>.Delete(d => d.UserId == StaticContext.UserId);
                var userData1 = RealmDb<LoginModel>.Get(d => d.UserId == StaticContext.UserId);

                StaticContext.UserId = "";
                rootFrame.Navigate(typeof(AdminLoginPage));
            }
        }

        public void NavigateToUsersPage() => rootFrame.Navigate(typeof(UserPage));

        public void NavigateToAllPhotosPage() => rootFrame.Navigate(typeof(AdminAllPhotos));

        private async void GetAllPhotos()
        {            
            AdminPagination page = new AdminPagination();
            try
            {
                PhotoBtnProgressVis = Visibility.Collapsed;
                PhotoBtnProgressRingVis = Visibility.Visible;
                _ = Task.Run(async () =>
                {
                    ResponseBody response = new ResponseBody();
                    response = await StaticContext.FetchAdminPhotosApiUrl.PostAsync<ResponseBody>(page);
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                        if (response.IsValidated)
                        {
                            ListBoxVis = Visibility.Visible;
                            List<AllPhotosResponseModel> list = new List<AllPhotosResponseModel>();
                            list = JsonConvert.DeserializeObject<List<AllPhotosResponseModel>>(response.Data);

                            AllPhotosList = list;
                            PreviousButtonEnable = false;
                            PhotoBtnVis();
                            if (response.Total > page.PageSize)
                            {
                                totalPageSize = response.Total;
                                NextButtonEnable = true;
                                DisabledbackBtnVis = Visibility.Visible;
                                DisabledBtnVis = Visibility.Collapsed;
                            }
                            else
                            {
                                NextButtonEnable = false;
                                DisabledBtnVis = Visibility.Visible;
                                DisabledbackBtnVis = Visibility.Visible;
                            }
                        }
                        else
                        {
                            ListBoxVis = Visibility.Collapsed;
                            NoDataValidationVis = Visibility.Visible;
                            PhotoBtnVis();
                        }
                    }).AsTask();
                });
                AdminName = StaticContext.AdminName;
            }
            catch (Exception)
            {

                return;
            }

        }

        public void CloseMessageTimer()
        {
            DispatcherTimer responseTimer = new DispatcherTimer();
            responseTimer.Tick += (senders, args) =>
            {
                CollapseNotificationPopup();
                responseTimer.Stop();
            };
            responseTimer.Interval = TimeSpan.FromSeconds(3);
            responseTimer.Start();
        }

        public void OpenNotificationPopup()
        {
            NotificationPopupGridVis = Visibility.Visible;
            NotificationPopup = true;
        }

        public void CollapseNotificationPopup()
        {
            NotificationPopupGridVis = Visibility.Collapsed;
            NotificationMessage = string.Empty;
            NotificationPopup = false;
        }
        public void GenerateUserVis()
        {
            GenerateUserBtnVis = Visibility.Collapsed;
            GenerateUserBtnProgressRingVis = Visibility.Visible;
        }  
        public void PhotoBtnVis()
        {
            PhotoBtnProgressVis = Visibility.Visible;
            PhotoBtnProgressRingVis = Visibility.Collapsed;
        }
        #endregion
    }
}
