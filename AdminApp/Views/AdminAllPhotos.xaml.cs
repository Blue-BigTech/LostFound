using AdminApp.ViewModels;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AdminApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminAllPhotos : Page
    {
        public AdminAllPhotos()
        {
            this.InitializeComponent();
          
        }

        private void ChangePasswordBtnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = (AdminAllPhotosViewModel)DataContext;
            //LoadingBackgroundGrid.Visibility = Visibility.Visible;
            viewModel.LoadingBackgroundGridVis = Visibility.Visible;
            viewModel.ResetPopupGridVis = Visibility.Visible;
            viewModel.PopupOpen = true;
        }
        private void AdminNamePointerEntered(object sender, PointerRoutedEventArgs e)
        {
            NavbarHoverGrid.Visibility = Visibility.Visible;
        }

        private void DeletePhotosCommand(object sender, RoutedEventArgs e)
        {
            StaticContext.ItemCode = (sender as Button).CommandParameter.ToString();
            var viewModel = (AdminAllPhotosViewModel)DataContext;
            viewModel.DeleteCommand.Execute(StaticContext.ItemCode);
        }
    }
}
