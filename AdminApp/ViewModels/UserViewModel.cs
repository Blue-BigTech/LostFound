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
using static Shared.UWPExtension;

namespace AdminApp.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        Frame rootFrame = Window.Current.Content as Frame;
        private long totalPageSize = 0;
        private int records = 0;
        //private int prevRecords = 0;

        public UserViewModel()
        {
            FetchUserDetail();
        }

        #region Visibility Properties

        private Visibility _generateUserBtnVis = Visibility.Visible;
        private Visibility _generateUserBtnProgressRingVis = Visibility.Collapsed;
        private Visibility _resetPopupGridVis = Visibility.Collapsed;
        private Visibility _updatePasswordProgressRingVis = Visibility.Collapsed;
        private Visibility _updatePasswordGridVis = Visibility.Visible;
        private Visibility _userNameValidationMsg = Visibility.Collapsed;
        private Visibility _userPasswordValidationMsg = Visibility.Collapsed;
        private Visibility _distCorpValidationMsg = Visibility.Collapsed;
        private Visibility _policestationValidationMsg = Visibility.Collapsed;
        private Visibility _notificationPopupGridVis = Visibility.Collapsed;
        private Visibility _logoutPopupGridVis = Visibility.Collapsed;
        private Visibility _newPasswordValidationMsg = Visibility.Collapsed;
        private Visibility _loadingBackgroundGridVis = Visibility.Collapsed;
        private Visibility _disabledBtnVis = Visibility.Collapsed;
        private Visibility _disabledbackBtnVis = Visibility.Collapsed;

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
        public Visibility ResetPopupGridVis
        {
            get { return _resetPopupGridVis; }
            set
            {
                _resetPopupGridVis = value;
                OnPropertyChanged("ResetPopupGridVis");
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
        public Visibility UpdatePasswordGridVis
        {
            get { return _updatePasswordGridVis; }
            set
            {
                _updatePasswordGridVis = value;
                OnPropertyChanged("UpdatePasswordGridVis");
            }
        }
        public Visibility UserNameValidationMsg
        {
            get { return _userNameValidationMsg; }
            set
            {
                _userNameValidationMsg = value;
                OnPropertyChanged("UserNameValidationMsg");
            }
        }
        public Visibility UserPasswordValidationMsg
        {
            get { return _userPasswordValidationMsg; }
            set
            {
                _userPasswordValidationMsg = value;
                OnPropertyChanged("UserPasswordValidationMsg");
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

        public Visibility DistCorpValidationMsg
        {
            get { return _distCorpValidationMsg; }
            set
            {
                _distCorpValidationMsg = value;
                OnPropertyChanged("DistCorpValidationMsg");
            }
        }

        public Visibility PoliceStatValidationMsg
        {
            get { return _policestationValidationMsg; }
            set
            {
                _policestationValidationMsg = value;
                OnPropertyChanged("PoliceStatValidationMsg");
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

        public Visibility LogoutPopupGridVis
        {
            get { return _logoutPopupGridVis; }
            set
            {
                _logoutPopupGridVis = value;
                OnPropertyChanged("LogoutPopupGrid");
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

        #region Elements Properties

        private string _userName = "";
        private string _newPassword = "";
        private string _userPassword = "";
        private string _districtCorp = "";
        private string _policeStation = "";
        private string _adminName = "";
        private string _itemCode = "";
        private string _notificationMessage = string.Empty;
        private int _pageNo = 1;
        private string _passwordStrength = "";

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    UserNameValidationMsg = Visibility.Visible;
                else
                    UserNameValidationMsg = Visibility.Collapsed;
                _userName = value; OnPropertyChanged("UserName");
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
        public string UserPassword
        {
            get { return _userPassword; }
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

                UserPasswordValidationMsg = Visibility.Visible;
                if (string.IsNullOrEmpty(value))
                {
                    PasswordStrength = "Required!";
                }
                _userPassword = value; OnPropertyChanged("UserPassword");

            }
        }
        public string PasswordStrength
        {
            get { return _passwordStrength; }
            set
            {
                _passwordStrength = value; OnPropertyChanged("PasswordStrength");
            }
        }
        public string DistrictCorp
        {
            get { return _districtCorp; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    DistCorpValidationMsg = Visibility.Visible;
                else
                    DistCorpValidationMsg = Visibility.Collapsed;
                _districtCorp = value; OnPropertyChanged("DistrictCorp");
            }
        }
        public string PoliceStation
        {
            get { return _policeStation; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    PoliceStatValidationMsg = Visibility.Visible;
                else
                    PoliceStatValidationMsg = Visibility.Collapsed;

                _policeStation = value; OnPropertyChanged("PoliceStation");
            }
        }
        public string AdminName
        {
            get { return _adminName; }
            set
            { _adminName = value; OnPropertyChanged("AdminName"); }
        }
        public string ItemCode
        {
            get { return _itemCode; }
            set
            { _itemCode = value; OnPropertyChanged("ItemCode"); }
        }
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
        #endregion

        #region Commands

        public ICommand HoverPointerEntered
        {
            get
            {
                return new DelegateCommand(ListOnHoverPointerEntered);
            }
        }
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
        public ICommand GenerateUserCommand
        {
            get { return new DelegateCommand(GenerateUser); }

        }
        public ICommand UpdateUserPasswordCommand
        {
            get { return new DelegateCommand(ResetUserPassword); }
        }
        public ICommand UsersCommand
        {
            get { return new DelegateCommand(NavigateUserPage); }
        }
        public ICommand AllPhotosCommand
        {
            get { return new DelegateCommand(NavigateAllPhotosPage); }
        }
        public ICommand GenerateRandomPassword
        {
            get { return new DelegateCommand(CreatePassword); }

        }
        public ICommand GenerateRandomName
        {
            get { return new DelegateCommand(GenerateName); }
        }
        public ICommand PreviousCommand
        {
            get { return new DelegateCommand(PreviousButton); }
        }
        public ICommand NextCommand
        {
            get { return new DelegateCommand(NextButton); }
        }
        public ICommand CancelbtnCommand
        {
            get { return new DelegateCommand(CancelPopup); }
        }

        //public ICommand ResetCommand
        //{
        //    get
        //    {
        //        return new CommadEventHandler<string>((Code) => GridResetPassword(Code));
        //    }

        //    //get { return new DelegateCommand(GridResetPassword); }
        //}

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

        private async void PreviousButton()
        {
            Shared.Model.Page page = new Shared.Model.Page();
            NextButtonEnable = true;
            PageNo = PageNo - 1;
            records = records - page.PageSize;
            page = new Shared.Model.Page()
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
                DisabledBtnVis = Visibility.Collapsed;
                ResponseBody response = new ResponseBody();
                response = await StaticContext.FetchUserApiUrl.PostAsync<ResponseBody>(page);
                if (response.IsValidated)
                {
                    List<UserResponseModel> list = JsonConvert.DeserializeObject<List<UserResponseModel>>(response.Data);
                    UsersList = list;

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
            Shared.Model.Page page = new Shared.Model.Page();
            if (totalPageSize > records)
            {
                PageNo = PageNo + 1;
                page = new Shared.Model.Page()
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
                ResponseBody response = new ResponseBody();
                response = await StaticContext.FetchUserApiUrl.PostAsync<ResponseBody>(page);
                if (response.IsValidated)
                {
                    DisabledbackBtnVis = Visibility.Collapsed;
                    List<UserResponseModel> list = JsonConvert.DeserializeObject<List<UserResponseModel>>(response.Data);
                    UsersList = list;
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
            try
            {
                ResetPassword resetpassword = new ResetPassword
                {
                    UserPassword = NewPassword,
                    Code = (StaticContext.IsNavbarPopup) == true ? StaticContext.UserId : ItemCode
                };

                if (resetpassword.UserPassword.Length >= 8 && !string.IsNullOrEmpty(resetpassword.Code) && !string.IsNullOrEmpty(resetpassword.UserPassword))
                {
                    UpdatePasswordVis();
                    _ = Task.Run(async () =>
                    {
                        ResponseBody response = new ResponseBody();
                        response = await StaticContext.ResetPasswordApiUrl.PostAsync<ResponseBody>(resetpassword);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            if (response.IsValidated)
                            {
                                UserDetail();
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
                UpdatePasswordVis();
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

        private void NavigateUserPage() => rootFrame.Navigate(typeof(UserPage));

        private void NavigateAllPhotosPage() => rootFrame.Navigate(typeof(AdminAllPhotos));

        public void FetchUserDetail() => UserDetail();

        public async void UserDetail()
        {
            Shared.Model.Page page = new Shared.Model.Page();
            try
            {
                ResponseBody response = new ResponseBody();
                response = await StaticContext.FetchUserApiUrl.PostAsync<ResponseBody>(page);

                if (response.IsValidated)
                {
                    List<UserResponseModel> list = new List<UserResponseModel>();
                    list = JsonConvert.DeserializeObject<List<UserResponseModel>>(response.Data);
                    UsersList = list;

                    PreviousButtonEnable = false;
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
                    OpenNotificationPopup();
                    NotificationMessage = response.Message;
                    CloseMessageTimer();
                }
                AdminName = StaticContext.AdminName;
            }
            catch (Exception)
            {

                return;
            }
        }

        public async void GenerateUser()
        {
            try
            {

                UserModel user = new UserModel
                {
                    UserName = UserName,
                    UserPassword = UserPassword,
                    DistrictCorporation = DistrictCorp,
                    PoliceStation = PoliceStation

                };
                if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.UserPassword) && !string.IsNullOrEmpty(user.DistrictCorporation) && !string.IsNullOrEmpty(user.PoliceStation))
                {
                    GenerateUserVis();
                    _ = Task.Run(async () =>
                    {
                        ResponseBody response = new ResponseBody();
                        response = await StaticContext.SaveUserApiUrl.PostAsync<ResponseBody>(user);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            if (response.IsValidated)
                            {
                                ClearField();
                                UserDetail();
                                OpenNotificationPopup();
                                NotificationMessage = response.Detail;
                                CloseMessageTimer();
                                UserValidation();
                                GenerateUserBtnVis = Visibility.Visible;
                                GenerateUserBtnProgressRingVis = Visibility.Collapsed;
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
                    if (string.IsNullOrEmpty(user.UserName))
                        UserNameValidationMsg = Visibility.Visible;
                    if (string.IsNullOrEmpty(user.UserPassword))
                    {
                        UserPasswordValidationMsg = Visibility.Visible;
                        PasswordStrength = "Required";
                    }
                    if (string.IsNullOrEmpty(user.DistrictCorporation))
                        DistCorpValidationMsg = Visibility.Visible;
                    if (string.IsNullOrEmpty(user.PoliceStation))
                        PoliceStatValidationMsg = Visibility.Visible;
                    GenerateUserBtnVis = Visibility.Visible;
                    GenerateUserBtnProgressRingVis = Visibility.Collapsed;
                    return;
                }

            }
            catch (Exception ex)
            {
                GenerateUserVis();
                return;
                throw ex;
            }
        }

        //private async void GridResetPassword(string code)
        //{
        //    try
        //    {

        //        ResetPassword resetpassword = new ResetPassword
        //        {
        //            UserPassword = NewPassword,
        //            //Code = (StaticContext.IsNavbarPopup) == true ? StaticContext.UserId : ItemCode
        //            Code = code
        //        };

        //        if (!string.IsNullOrEmpty(resetpassword.UserPassword))
        //        {
        //            UpdatePasswordVis();
        //            _ = Task.Run(async () =>
        //            {
        //                ResponseBody response = new ResponseBody();
        //                response = await StaticContext.ResetPasswordApiUrl.PostAsync<ResponseBody>(resetpassword);
        //                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
        //                {
        //                    if (response.IsValidated)
        //                    {
        //                        UserDetail();
        //                        NewPassword = "";
        //                        CancelPopup();
        //                        //ResetPopupGridVis = Visibility.Visible;
        //                        //PopupOpen = true;
        //                        //ResetPopupGridVis = Visibility.Collapsed;
        //                        //PopupOpen = false;
        //                        OpenNotificationPopup();
        //                        NotificationMessage = response.Message;
        //                        CloseMessageTimer();
        //                        UpdatePasswordGridVis = Visibility.Visible;
        //                        UpdatePasswordProgressRingVis = Visibility.Collapsed;
        //                    }
        //                    else
        //                    {

        //                        OpenNotificationPopup();
        //                        NotificationMessage = response.Message;
        //                        CloseMessageTimer();
        //                    }
        //                }).AsTask();
        //            });
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(resetpassword.UserPassword))
        //            {
        //                NewPasswordValidationMsg = Visibility.Visible;
        //                PasswordStrength = "Required";
        //            }
        //        }
        //        //UpdatePasswordVis();

        //    }
        //    catch (Exception ex)
        //    {
        //        UpdatePasswordVis();
        //        throw ex;
        //    }
        //}


        public void GenerateName()
        {
            UserName = RandomName();
        }
        public void CreatePassword()
        {
            UserPassword = GeneratePassword();
        }

        private void ClearField()
        {
            UserName = "";
            UserPassword = "";
            DistrictCorp = "";
            PoliceStation = "";
        }

        private void CancelPopup()
        {
            LoadingBackgroundGridVis = Visibility.Collapsed;
            ResetPopupGridVis = Visibility.Visible;
            PopupOpen = false;
        }

        public void UpdatePasswordVis()
        {
            UpdatePasswordGridVis = Visibility.Collapsed;
            UpdatePasswordProgressRingVis = Visibility.Visible;
        }
        public void GenerateUserVis()
        {
            GenerateUserBtnVis = Visibility.Collapsed;
            GenerateUserBtnProgressRingVis = Visibility.Visible;
        }
        public void UserValidation()
        {
            UserNameValidationMsg = Visibility.Collapsed;
            UserPasswordValidationMsg = Visibility.Collapsed;
            DistCorpValidationMsg = Visibility.Collapsed;
            PoliceStatValidationMsg = Visibility.Collapsed;
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
        private void ListOnHoverPointerEntered()
        {

        }
        #endregion

        #region List

        private List<UserResponseModel> _usersList;

        public List<UserResponseModel> UsersList
        {
            get { return _usersList; }
            set
            {
                _usersList = value; OnPropertyChanged("UsersList");
            }
        }

        #endregion
    }
}
