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

namespace SimplyRugby.Help.HelpPro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpPro : Page
    {
        public HelpPro()
        {
            this.InitializeComponent();
        }
        private void LoggingInButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(LoggingInPro));
        }

        private void CreatingAccountsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(AccountCreationPro));
        }

        private void ViewingTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(ViewingTeamsPro));
        }

        private void NavHelpAdminButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(NavHelpPro));
        }

        private void RegisteringPlayersButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(RegisteringPlayersPro));
        }

        private void ViewingPlayersButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(ViewingPlayersPro));
        }

        private void RegisterPlayerStatsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(RegisterPlayerStatsPro));
        }

        private void ViewingPlayerStatsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(ViewingPlayerStatsPro));
        }

        private void RegisterJuniorPlayerStatsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(JuniorPlayersRegisterPro));
        }

        private void ViewingJuniorPlayerStatsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(JuniorPlayersViewPro));
        }

        private void MatchLogAmateurButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(MatchLogRegisterPro));
        }

        private void ViewingMatchLogAmateurButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(MatchLogViewPro));
        }

        private void WriteTrainingLogsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(TrainingLogRegisterPro));
        }

        private void ViewTrainingLogsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(TrainingLogViewPro));
        }

        private void LogAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(AttendanceLogRegisterPro));
        }

        private void ViewAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(AttendanceLogViewPro));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(LogoutPro));
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(FullScreenPro));
        }
    }
}
