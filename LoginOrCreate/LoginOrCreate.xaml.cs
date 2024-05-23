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

namespace SimplyRugby.LoginOrCreate
{
    /// <summary>
    /// A page that allows the user to choose to either login or create an account
    /// </summary>
    public sealed partial class LoginOrCreate : Page
    {
        /// <summary>
        /// Constructor for the LoginOrCreate page
        /// </summary>
        public LoginOrCreate()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Method to navigate to the login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Login page
            Frame.Navigate(typeof(LoginPage.LoginPage)); // Assume LoginPage is the name of your login page
        }
        /// <summary>
        /// Method to navigate to the create account page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to EntryPage for account creation
            Frame.Navigate(typeof(CreatePage.CreatePage));
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Help.HelpMainPage.HelpMainPage));
        }
    }
}
