using AdminApp.ViewModels;
using ClientApp.Views;
using Shared;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ClientApp.ViewModels
{
    public class ClientDashboardViewModel : BaseViewModel
    {
        public string selectedItem;
        Frame rootFrame = Window.Current.Content as Frame;
        byte[] byteData = null;
        string fileName = null;
        public ClientDashboardViewModel()
        {

        }

        #region Elements Properties

        private string _newPassword = "";
        private string _personName = "";
        private string _contactPersonName = "";
        private string _typeofPerson = "";
        private string _contactPersonPhoneNo = "";
        private string _caseNo = "";
        private string _officerName = "";
        private string _officerBPNo = "";
        private string _officerPhoneNo = "";
        private string _remarks = "";
        private string _policeStation = StaticContext.PoliceStation;
        private string _notificationMessage = string.Empty;
        private string _passwordStrength = "";

        public string TypeofPerson
        {
            get { return _typeofPerson; }
            set { _typeofPerson = value; OnPropertyChanged("TypeofPerson"); }
        }
        public string PersonName
        {
            get { return _personName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value))
                    NameValidationVis = Visibility.Collapsed;
                else
                    NameValidationVis = Visibility.Visible;
                _personName = value; OnPropertyChanged("PersonName");
            }
        }
        public string ContactPersonName
        {
            get { return _contactPersonName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value))
                    CPNameValidationVis = Visibility.Collapsed;
                else
                    CPNameValidationVis = Visibility.Visible;
                _contactPersonName = value; OnPropertyChanged("ContactPersonName");
            }
        }
        public string ContactPersonPhoneNo
        {
            get { return _contactPersonPhoneNo; }
            set
            {

                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value) && value.Length < 15)
                {
                    CPPhoneNoValidationVis = Visibility.Collapsed;
                    PhoneNoValidationVis = Visibility.Visible;
                }
                else if (string.IsNullOrWhiteSpace(value) && string.IsNullOrEmpty(value))
                {
                    CPPhoneNoValidationVis = Visibility.Visible;
                    PhoneNoValidationVis = Visibility.Collapsed;
                }
                else
                {
                    PhoneNoValidationVis = Visibility.Collapsed;
                    CPPhoneNoValidationVis = Visibility.Collapsed;
                }
                _contactPersonPhoneNo = value; OnPropertyChanged("ContactPersonPhoneNo");
            }
        }
        public string CaseNo
        {
            get { return _caseNo; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value))
                    GDNoValidationVis = Visibility.Collapsed;
                else
                    GDNoValidationVis = Visibility.Visible;
                _caseNo = value; OnPropertyChanged("CaseNo");
            }
        }
        public string OfficerName
        {
            get { return _officerName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value))
                    OfficerNameValidationVis = Visibility.Collapsed;
                else
                    OfficerNameValidationVis = Visibility.Visible;
                _officerName = value; OnPropertyChanged("OfficerName");
            }
        }
        public string OfficerBPNo
        {
            get { return _officerBPNo; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value))
                    OfficerBPNoValidationVis = Visibility.Collapsed;
                else
                    OfficerBPNoValidationVis = Visibility.Visible;
                _officerBPNo = value; OnPropertyChanged("OfficerBPNo");
            }
        }
        public string OfficerPhoneNo
        {
            get { return _officerPhoneNo; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value) && value.Length < 15)
                {
                    OfficerPhoneNoValidationVis = Visibility.Collapsed;
                    OfficerNoValidationVis = Visibility.Visible;
                }
                else if (string.IsNullOrWhiteSpace(value) && string.IsNullOrEmpty(value))
                {
                    OfficerPhoneNoValidationVis = Visibility.Visible;
                    OfficerNoValidationVis = Visibility.Collapsed;
                }
                else
                {
                    OfficerNoValidationVis = Visibility.Collapsed;
                    OfficerPhoneNoValidationVis = Visibility.Collapsed;
                }
                _officerPhoneNo = value; OnPropertyChanged("OfficerPhoneNo");
            }
        }
        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; OnPropertyChanged("Remarks"); }
        }
        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set { _notificationMessage = value; OnPropertyChanged("NotificationMessage"); }
        }

        public string PoliceStation
        {
            get { return _policeStation; }
            set
            { _policeStation = value; OnPropertyChanged("PoliceStation"); }
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

        private Visibility _choosePhotoVis = Visibility.Visible;
        private Visibility _choosePhotoProgressRingVis = Visibility.Collapsed;
        private Visibility _saveProfileBtnGridVis = Visibility.Visible;
        private Visibility _saveProfileBtnProgressRingVis = Visibility.Collapsed;
        private Visibility _notificationPopupGridVis = Visibility.Collapsed;
        private Visibility _updatePasswordProgressRingVis = Visibility.Collapsed;
        private Visibility _updatePasswordGridVis = Visibility.Visible;
        private Visibility _resetPopupGridVis = Visibility.Collapsed;
        private Visibility _logoutPopupGridVis = Visibility.Collapsed;
        private Visibility _displayImageVis = Visibility.Collapsed;
        private Visibility _avatarImageVis = Visibility.Visible;
        private Visibility _nameValidationVis = Visibility.Collapsed;
        private Visibility _cPNameValidationVis = Visibility.Collapsed;
        private Visibility _cPPhoneNoValidationVis = Visibility.Collapsed;
        private Visibility _gDNoValidationVis = Visibility.Collapsed;
        private Visibility _officerNameValidationVis = Visibility.Collapsed;
        private Visibility _officerBPNoValidationVis = Visibility.Collapsed;
        private Visibility _officerPhoneNoValidationVis = Visibility.Collapsed;
        private Visibility _personTypeValidationMsg = Visibility.Collapsed;
        private Visibility _phoneNoValidationVis = Visibility.Collapsed;
        private Visibility _officerNoValidationVis = Visibility.Collapsed;
        private Visibility _loadingBackgroundGridVis = Visibility.Collapsed;
        private Visibility _newPasswordValidationMsg = Visibility.Collapsed;

        public Visibility NewPasswordValidationMsg
        {
            get { return _newPasswordValidationMsg; }
            set
            {
                _newPasswordValidationMsg = value;
                OnPropertyChanged("NewPasswordValidationMsg");
            }
        }
        public Visibility OfficerNoValidationVis
        {
            get { return _officerNoValidationVis; }
            set
            {
                _officerNoValidationVis = value;
                OnPropertyChanged("OfficerNoValidationVis");
            }
        }
        public Visibility PhoneNoValidationVis
        {
            get { return _phoneNoValidationVis; }
            set
            {
                _phoneNoValidationVis = value;
                OnPropertyChanged("PhoneNoValidationVis");
            }
        }
        public Visibility PersonTypeValidationMsg
        {
            get { return _personTypeValidationMsg; }
            set
            {
                _personTypeValidationMsg = value;
                OnPropertyChanged("PersonTypeValidationMsg");
            }
        }
        public Visibility OfficerPhoneNoValidationVis
        {
            get { return _officerPhoneNoValidationVis; }
            set
            {
                _officerPhoneNoValidationVis = value;
                OnPropertyChanged("OfficerPhoneNoValidationVis");
            }
        }
        public Visibility OfficerBPNoValidationVis
        {
            get { return _officerBPNoValidationVis; }
            set
            {
                _officerBPNoValidationVis = value;
                OnPropertyChanged("OfficerBPNoValidationVis");
            }
        }
        public Visibility OfficerNameValidationVis
        {
            get { return _officerNameValidationVis; }
            set
            {
                _officerNameValidationVis = value;
                OnPropertyChanged("OfficerNameValidationVis");
            }
        }
        public Visibility GDNoValidationVis
        {
            get { return _gDNoValidationVis; }
            set
            {
                _gDNoValidationVis = value;
                OnPropertyChanged("GDNoValidationVis");
            }
        }
        public Visibility CPPhoneNoValidationVis
        {
            get { return _cPPhoneNoValidationVis; }
            set
            {
                _cPPhoneNoValidationVis = value;
                OnPropertyChanged("CPPhoneNoValidationVis");
            }
        }
        public Visibility CPNameValidationVis
        {
            get { return _cPNameValidationVis; }
            set
            {
                _cPNameValidationVis = value;
                OnPropertyChanged("CPNameValidationVis");
            }
        }
        public Visibility NameValidationVis
        {
            get { return _nameValidationVis; }
            set
            {
                _nameValidationVis = value;
                OnPropertyChanged("NameValidationVis");
            }
        }
        public Visibility ChoosePhotoVis
        {
            get { return _choosePhotoVis; }
            set
            {
                _choosePhotoVis = value;
                OnPropertyChanged("ChoosePhotoVis");
            }
        }
        public Visibility SaveProfileBtnGridVis
        {
            get { return _saveProfileBtnGridVis; }
            set
            {
                _saveProfileBtnGridVis = value;
                OnPropertyChanged("SaveProfileBtnGridVis");
            }
        }
        public Visibility SaveProfileBtnProgressRingVis
        {
            get { return _saveProfileBtnProgressRingVis; }
            set
            {
                _saveProfileBtnProgressRingVis = value;
                OnPropertyChanged("SaveProfileBtnProgressRingVis");
            }
        }
        public Visibility ChoosePhotoProgressRingVis
        {
            get { return _choosePhotoProgressRingVis; }
            set
            {
                _choosePhotoProgressRingVis = value;
                OnPropertyChanged("ChoosePhotoProgressRingVis");
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
        public Visibility ResetPopupGridVis
        {
            get { return _resetPopupGridVis; }
            set
            {
                _resetPopupGridVis = value;
                OnPropertyChanged("ResetPopupGridVis");
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
        public Visibility DisplayImageVis
        {
            get { return _displayImageVis; }
            set
            {
                _displayImageVis = value;
                OnPropertyChanged("DisplayImageVis");
            }
        }
        public Visibility AvatarImageVis
        {
            get { return _avatarImageVis; }
            set
            {
                _avatarImageVis = value;
                OnPropertyChanged("AvatarImageVis");
            }
        }
        #endregion

        public object ComboBoxSelectionChanged
        {
            set
            {
                if (!ReferenceEquals(value, null))
                {
                    if (value.ToString()!= PersonType.Select.ToString())
                    {
                        selectedItem = value.ToString();
                        PersonTypeValidationMsg = Visibility.Collapsed;
                    }
                   
                }
            }
        }

        #region Popup Properties

        private bool _popupOpen = false;
        private bool _notificationPopup = false;
        private bool _logoutpopupOpen = false;

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

        #region List Properties

        private List<ComboBoxItem> _typeofPersonsList;

        public List<ComboBoxItem> TypeofPersonsList
        {
            get { return _typeofPersonsList; }
            set { _typeofPersonsList = value; OnPropertyChanged("TypeofPersonsList"); }
        }

        #endregion

        #region Events

        #endregion

        #region Image

        private BitmapImage _DisplayImage;

        public BitmapImage DisplayImage
        {
            get { return _DisplayImage; }
            set { _DisplayImage = value; OnPropertyChanged("DisplayImage"); }
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

        public ICommand UploadCommand
        {

            get { return new DelegateCommand(NavigateToUploadPage); }
        }
        public ICommand AllPhotosCommand
        {
            get { return new DelegateCommand(NavigateToAllPhotosPage); }
        }

        public ICommand LostPersonPhoto
        {
            get { return new DelegateCommand(SelectLostPersonPhoto); }
        }
        public ICommand FileReportCommand
        {
            get { return new DelegateCommand(FilePersonReport); }
        }
        public ICommand UpdateUserPasswordCommand
        {
            get { return new DelegateCommand(ResetUserPassword); }
        }
        public ICommand CancelbtnCommand
        {
            get { return new DelegateCommand(CancelPopup); }
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
                rootFrame.Navigate(typeof(ClientLoginPage));
            }
        }


        private async void ResetUserPassword()
        {
            try
            {              
                ResetPassword resetpassword = new ResetPassword
                {
                    UserPassword = NewPassword,
                    Code = StaticContext.UserId
                };

                if (resetpassword.UserPassword.Length >= 8 && !string.IsNullOrEmpty(resetpassword.Code) && !string.IsNullOrEmpty(resetpassword.UserPassword))
                {

                    ClientDashboardPasswordVis();
                    _ = Task.Run(async () =>
                    {
                        ResponseBody response = new ResponseBody();
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

        public void NavigateToUploadPage()
        {
            rootFrame.Navigate(typeof(ClientDashboard));
        }
        public void NavigateToAllPhotosPage()
        {
            rootFrame.Navigate(typeof(ClientAllPhotos));
        }
        public async Task<BitmapImage> GetBitmapImage(StorageFile file)
        {
            BitmapImage image = new BitmapImage();
            using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                image = bitmapImage;

            }
            return image;
        }

        public async Task<byte[]> GetByteArray(StorageFile file)
        {
            var stream = await file.OpenStreamForReadAsync();
            byte[] bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public async void SelectLostPersonPhoto()
        {
            ChoosePhotoVis = Visibility.Collapsed;
            ChoosePhotoProgressRingVis = Visibility.Visible;

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                AvatarImageVis = Visibility.Collapsed;
                DisplayImageVis = Visibility.Visible;
                DisplayImage = await GetBitmapImage(file);
                byteData = await GetByteArray(file);
                fileName = file.Name;
            }
            ChoosePhotoVis = Visibility.Visible;
            ChoosePhotoProgressRingVis = Visibility.Collapsed;

        }

        public async void FilePersonReport()
        {

            try
            {
                DasboardModel model = new DasboardModel()
                {
                    IsLost = selectedItem == null ? false : selectedItem.ToString() == "Lost" ? false : true,
                    Name = PersonName,
                    CPName = ContactPersonName,
                    CPPhoneNo = ContactPersonPhoneNo,
                    GDCaseNo = CaseNo,
                    OfficerName = OfficerName,
                    OfficerBPNo = OfficerBPNo,
                    OfficerPhoneNo = OfficerPhoneNo,
                    AspNetUserId = StaticContext.UserId,
                    Remarks = Remarks,
                    FileName = fileName,
                    Image = byteData
                };
                if (!ReferenceEquals(selectedItem, null) && !string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.CPName)
                          && !string.IsNullOrEmpty(model.CPPhoneNo) && model.CPPhoneNo.Length >= 15 && !string.IsNullOrEmpty(model.GDCaseNo) && !string.IsNullOrEmpty(model.OfficerName)
                          && !string.IsNullOrEmpty(model.OfficerBPNo) && !string.IsNullOrEmpty(model.OfficerPhoneNo) && model.OfficerPhoneNo.Length >= 15)
                {
                    SaveProfileBtnGridVis = Visibility.Collapsed;
                    SaveProfileBtnProgressRingVis = Visibility.Visible;
                    selectedItem = null;        
                    _ = Task.Run(async () =>
                    {
                        ResponseBody response = new ResponseBody();
                        response = await StaticContext.AdminDashboardApiUrl.PostAsync<UserResponseModel>(model);
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            if (response.IsValidated)
                            {
                                OpenNotificationPopup();
                                NotificationMessage = response.Message;
                                CloseMessageTimer();
                                ClearField();
                                SaveProfileBtnGridVis = Visibility.Visible;
                                SaveProfileBtnProgressRingVis = Visibility.Collapsed;
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
                    if (ReferenceEquals(selectedItem, null))
                        PersonTypeValidationMsg = Visibility.Visible;
                    if (string.IsNullOrEmpty(model.Name))
                        NameValidationVis = Visibility.Visible;
                    if (string.IsNullOrEmpty(model.CPName))
                        CPNameValidationVis = Visibility.Visible;
                    if (string.IsNullOrEmpty(model.CPPhoneNo))
                    {
                        CPPhoneNoValidationVis = Visibility.Visible;
                        PhoneNoValidationVis = Visibility.Collapsed;
                    }
                    else if (model.CPPhoneNo.Length < 15)
                    {
                        PhoneNoValidationVis = Visibility.Visible;
                        CPPhoneNoValidationVis = Visibility.Collapsed;
                    }
                    if (string.IsNullOrEmpty(model.GDCaseNo))
                        GDNoValidationVis = Visibility.Visible;
                    if (string.IsNullOrEmpty(model.OfficerName))
                        OfficerNameValidationVis = Visibility.Visible;
                    if (string.IsNullOrEmpty(model.OfficerBPNo))
                        OfficerBPNoValidationVis = Visibility.Visible;
                    if (string.IsNullOrEmpty(model.OfficerPhoneNo))
                    {
                        OfficerPhoneNoValidationVis = Visibility.Visible;
                        OfficerNoValidationVis = Visibility.Collapsed;
                    }
                    else if (model.OfficerPhoneNo.Length < 15)
                    {
                        OfficerNoValidationVis = Visibility.Visible;
                        OfficerPhoneNoValidationVis = Visibility.Collapsed;
                    }
                    //OpenNotificationPopup();
                    //NotificationMessage = "Value cannot be empty!";
                    //CloseMessageTimer();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void ClearField()
        {
            selectedItem = null;
            PersonName = "";
            ContactPersonName = "";
            ContactPersonPhoneNo = "";
            CaseNo = "";
            OfficerName = "";
            OfficerBPNo = "";
            OfficerPhoneNo = "";
            Remarks = "";
            AvatarImageVis = Visibility.Visible;
            DisplayImageVis = Visibility.Collapsed;
            DisplayImage = new BitmapImage();

            NameValidationVis = Visibility.Collapsed;

            CPNameValidationVis = Visibility.Collapsed;

            CPPhoneNoValidationVis = Visibility.Collapsed;

            GDNoValidationVis = Visibility.Collapsed;

            OfficerNameValidationVis = Visibility.Collapsed;

            OfficerBPNoValidationVis = Visibility.Collapsed;

            OfficerPhoneNoValidationVis = Visibility.Collapsed;
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
        public void ClientDashboardPasswordVis()
        {
            UpdatePasswordGridVis = Visibility.Collapsed;
            UpdatePasswordProgressRingVis = Visibility.Visible;
        }
        #endregion

    }
}
