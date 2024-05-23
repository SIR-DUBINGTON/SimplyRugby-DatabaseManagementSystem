using SimplyRugby.Help.HelpPro;
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

namespace SimplyRugby.Navigation.UserNavProCoach
{
    /// <summary>
    /// Page that contains the navigation menu for the Pro Coach user.
    /// </summary>
    public sealed partial class UserNavProCoach : Page
    {
        public UserNavProCoach()
        {
            this.InitializeComponent();
            RenderTransform = new TranslateTransform { X = -Width };

        }

        /// <summary>
        /// Method to open navigation pane when hamburger is clicked.
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
                    rootFrame.Navigate(typeof(ProCoachHub.ProCoachHub));
                    break;
                case "PlayerDevListBoxItem":
                    rootFrame.Navigate(typeof(PlayerDevCentrePro.PlayerDevCentrePro));
                    break;
                case "MatchesListBoxItem":
                    rootFrame.Navigate(typeof(MatchAndTrainingPro));
                    break;
                case "TrainingListBoxItem":
                    rootFrame.Navigate(typeof(MatchAndTrainingPro));
                    break;
                case "SettingsListBoxItem":
                    rootFrame.Navigate(typeof(SettingsPro.SettingsPro));
                    break;
                case "HelpListBoxItem":
                    rootFrame.Navigate(typeof(HelpPro));
                    break;
                case "AboutListBoxItem":
                    rootFrame.Navigate(typeof(AboutPro.AboutPro));
                    break;
                case "LogoutListBoxItem":
                    SessionManager.SessionManager.ClearSession(); // Clear session data
                    rootFrame.Navigate(typeof(SimplyRugby.LoginOrCreate.LoginOrCreate));
                    break;
            }
        }
    }
}
