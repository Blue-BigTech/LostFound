using ClientApp.ViewModels;
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

namespace ClientApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ClientDashboard : Page
    {
        public ClientDashboard()
        {
            this.InitializeComponent();
      
        }
        private void ComboBoxLoaded(object sender, RoutedEventArgs e)
        {
            Combobox1.Items.Add(PersonType.Select);
            Combobox1.Items.Add(PersonType.Lost);
            Combobox1.Items.Add(PersonType.Found);

            Combobox1.SelectedIndex = Convert.ToInt32(PersonType.Select);
        }
        private void ChangePasswordBtnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = (ClientDashboardViewModel)DataContext;
            //LoadingBackgroundGrid.Visibility = Visibility.Visible;
            viewModel.LoadingBackgroundGridVis = Visibility.Visible;
            viewModel.ResetPopupGridVis = Visibility.Visible;
            viewModel.PopupOpen = true;
        }

        private void NamePointerEntered(object sender, PointerRoutedEventArgs e)
        {
            NavbarHoverGrid.Visibility = Visibility.Visible;
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            NavbarHoverGrid.Visibility = Visibility.Collapsed;
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                }
            
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ClientDashboardViewModel)DataContext;
            viewModel.FilePersonReport();
            if (viewModel.selectedItem == null)
            {
                Combobox1.SelectedIndex = Convert.ToInt32(PersonType.Select);
            }
        }

        private void CPPhoneNo_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            int pos = sender.SelectionStart;
            sender.Text = new String(sender.Text.Where(char.IsDigit).ToArray());
            sender.SelectionStart = pos;
        }
        private void OfficerPhoneNo_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            int pos = sender.SelectionStart;
            sender.Text = new String(sender.Text.Where(char.IsDigit).ToArray());
            sender.SelectionStart = pos;
        }
    }
}
