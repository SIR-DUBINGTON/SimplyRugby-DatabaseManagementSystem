using SimplyRugby.Help.HelpAdmin;
using SimplyRugby.PlayerDevCentreAdmin.ViewStaff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimplyRugby.Navigation.UserNavAdminStaff
{
    /// <summary>
    /// Page that contains the navigation menu for the Admin and Coaches users.
    /// </summary>
    public sealed partial class UserNavAdminStaff : Page
    {
        public UserNavAdminStaff()
        {
            this.InitializeComponent();
            RenderTransform = new TranslateTransform { X = -Width };
        }
        /// <summary>
        /// Hamburger button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        /// <summary>
        /// Event handler for the IconsListBox selection change.
        /// Navigates to the selected page based on the ListBoxItem's name.
        /// </summary>
        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            switch (((ListBoxItem)IconsListBox.SelectedItem).Name)
            {
                case "HomeListBoxItem":
                    rootFrame.Navigate(typeof(AdminStaffHub.AdminStaffHub));
                    break;
                case "StaffAdminAndTeams":
                    rootFrame.Navigate(typeof(PlayerDevCentreAdmin.PlayerDevCentreAdmin));
                    break;
                case "SettingsListBoxItem":
                    rootFrame.Navigate(typeof(SettingsAdmin.SettingsAdmin));
                    break;
                case "HelpListBoxItem":
                    rootFrame.Navigate(typeof(HelpAdmin));
                    break;
                case "AboutListBoxItem":
                    rootFrame.Navigate(typeof(AboutAdmin.AboutAdmin));
                    break;
                case "LogoutListBoxItem":
                    SessionManager.SessionManager.ClearSession(); // Clear session data
                    rootFrame.Navigate(typeof(SimplyRugby.LoginOrCreate.LoginOrCreate));
                    break;
            }
        }
    }
}
