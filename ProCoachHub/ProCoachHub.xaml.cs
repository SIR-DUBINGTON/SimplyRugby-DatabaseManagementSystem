using SimplyRugby.MatchAndTrainingAmateur;
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

namespace SimplyRugby.ProCoachHub
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProCoachHub : Page
    {
        /// <summary>
        /// Welcome text to display on the page
        /// </summary>
        public string WelcomeText { get; set; }

        public ProCoachHub()
        {
            this.InitializeComponent();
            UsernameTextBlock.Text = $"Welcome, {SessionManager.SessionManager.Username}!";

            DataContext = this;

        }
        /// <summary>
        /// Method to navigate to the Player Development Centre page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerDevCentreButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(PlayerDevCentrePro.PlayerDevCentrePro));
        }
        /// <summary>
        /// Method to navigate to the Leagues & Results page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MatchAndTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Leagues & Results page
            Frame.Navigate(typeof(MatchAndTrainingPro));
        }
    }
}
