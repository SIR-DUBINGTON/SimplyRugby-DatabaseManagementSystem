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

namespace SimplyRugby.AmateurCoachHub
{
    /// <summary>
    /// A page for the Amateur Coach Hub
    /// </summary>
    public sealed partial class AmateurCoachHub : Page
    {
        // Welcome text for the user
        public string WelcomeText { get; set; }

        //Constructor
        public AmateurCoachHub()
        {
            // Set the welcome text to include the username
            this.InitializeComponent();
            UsernameTextBlock.Text = $"Welcome, {SessionManager.SessionManager.Username}!";

            DataContext = this;
        }
        private void PlayerDevCentreButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(PlayerDevCentre.PlayerDevCentre));
        }

        private void MatchAndTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Match and Trainings page
            Frame.Navigate(typeof(MatchAndTrainingAmateur.MatchAndTrainingAmateur));
        }

    }
}
