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

namespace SimplyRugby.AdminStaffHub
{
    /// <summary>
    /// All elements on the Admin Staff Hub page are defined here.
    /// </summary>
    /// 
    public sealed partial class AdminStaffHub : Page
    {
        // Welcome text for the user
        public string WelcomeText { get; set; }

        // Constructor
        public AdminStaffHub()
        {
            // Set the welcome text to include the username of the current user
            this.InitializeComponent();
            UsernameTextBlock.Text = $"Welcome, {SessionManager.SessionManager.Username}!";

            DataContext = this;
        }

        private void PlayerDevCentreButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Player Development Centre page
            Frame.Navigate(typeof(PlayerDevCentreAdmin.PlayerDevCentreAdmin));
        }

    }
}
