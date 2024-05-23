using SimplyRugby.Help.HelpAdmin;
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

namespace SimplyRugby.Help.HelpAmateur
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpAmateur : Page
    {
        public HelpAmateur()
        {
            this.InitializeComponent();
        }
        private void LoggingInButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(LoggingInAmateur));
        }

        private void CreatingAccountsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(AccountCreationAmateur));
        }

        private void ViewingTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(ViewingTeamsAmateur));
        }

        private void NavHelpAdminButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(NavHelpAmateur));
        }

        private void RegisteringPlayersButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(RegisteringPlayersAmateur));
        }

        private void ViewingPlayersButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(ViewingPlayersAmateur));
        }

        private void RegisterPlayerStatsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(RegisterPlayerStatsAmateur));
        }

        private void ViewingPlayerStatsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(ViewingPlayerStatsAmateur));
        }

        private void RegisterJuniorPlayerStatsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(JuniorPlayersRegisterAmateur));
        }

        private void ViewingJuniorPlayerStatsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(JuniorPlayersViewAmateur));
        }

        private void MatchLogAmateurButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(MatchLogRegisterAmateur));
        }

        private void ViewingMatchLogAmateurButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(MatchLogViewAmateur));
        }

        private void WriteTrainingLogsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(TrainingLogRegisterAmateur));
        }

        private void ViewTrainingLogsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(TrainingLogViewAmateur));
        }

        private void LogAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(AttendanceLogRegisterAmateur));
        }

        private void ViewAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(AttendanceLogViewAmateur));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(LogoutAmateur));
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(FullScreenAmateur));
        }
    }
}
