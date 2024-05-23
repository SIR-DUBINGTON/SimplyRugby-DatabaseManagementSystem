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

namespace SimplyRugby.Help.HelpAdmin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpAdmin : Page
    {
        public HelpAdmin()
        {
            this.InitializeComponent();
        }
        private void LoggingInButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(LoggingInAdmin));
        }

        private void CreatingAccountsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(AccountCreationAdmin));
        }

        private void ViewingTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(ViewingTeamsAdmin));
        }

        private void CreatingTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(CreatingTeamsAdmin));
        }

        private void ViewStaffButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(ViewStaffAdmin));
        }

        private void DeleteStaffButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(DeleteStaffAdmin));
        }

        private void NavHelpAdminButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(NavHelpAdmin));
        }

        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(FullScreenAdmin));
        }

        private void LoggingOutButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(LogoutAdmin));
        }
    }
}
