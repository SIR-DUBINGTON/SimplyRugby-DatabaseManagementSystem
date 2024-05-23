using System;
using SimplyRugby.MatchAndTraining;
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
using SimplyRugby.Help.HelpAmateur;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimplyRugby.Navigation.UserNavAmateurCoach
{
    /// <summary>
    /// Page that contains the navigation pane for the amateur coach user.
    /// </summary>
    public sealed partial class UserNavAmateurCoach : Page
    {
        public UserNavAmateurCoach()
        {
            this.InitializeComponent();
            RenderTransform = new TranslateTransform { X = -Width };
        }

        /// <summary>
        /// Event handler for the HamburgerButton click.
        /// Toggles the visibility of the navigation pane.
        /// </summary>
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
                    rootFrame.Navigate(typeof(AmateurCoachHub.AmateurCoachHub));
                    break;
                case "PlayerDevListBoxItem":
                    rootFrame.Navigate(typeof(PlayerDevCentre.PlayerDevCentre));
                    break;
                case "MatchesListBoxItem":
                    rootFrame.Navigate(typeof(MatchAndTrainingAmateur.MatchAndTrainingAmateur));
                    break;
                case "TrainingListBoxItem":
                    rootFrame.Navigate(typeof(MatchAndTrainingAmateur.MatchAndTrainingAmateur));
                    break;
                case "SettingsListBoxItem":
                    rootFrame.Navigate(typeof(SettingsAmateur.SettingsAmateur));
                    break;
                case "HelpListBoxItem":
                    rootFrame.Navigate(typeof(HelpAmateur));
                    break;
                case "AboutListBoxItem":
                    rootFrame.Navigate(typeof(AboutAmateur.AboutAmateur));
                    break;
                case "LogoutListBoxItem":
                    SessionManager.SessionManager.ClearSession(); // Clear session data
                    rootFrame.Navigate(typeof(SimplyRugby.LoginOrCreate.LoginOrCreate));
                    break;
            }
        }
    }
}
