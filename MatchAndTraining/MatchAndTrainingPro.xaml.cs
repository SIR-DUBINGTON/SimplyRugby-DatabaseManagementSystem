using SimplyRugby.MatchAndTraining.TrainingAttendanceAmateur;
using SimplyRugby.MatchAndTraining.TrainingAttendancePro;
using SimplyRugby.MatchAndTraining.TrainingLogPro;
using SimplyRugby.MatchAndTraining.ViewAttendanceAmateur;
using SimplyRugby.MatchAndTraining.ViewAttendancePro;
using SimplyRugby.MatchAndTraining.ViewTrainingLogAmateur;
using SimplyRugby.MatchAndTraining.ViewTrainingLogPro;
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

namespace SimplyRugby.MatchAndTrainingAmateur
{
    /// <summary>
    /// Page for Match and Training for Pro players
    /// </summary>
    public sealed partial class MatchAndTrainingPro : Page
    {
        public string WelcomeText { get; set; }

        public MatchAndTrainingPro()
        {
            this.InitializeComponent();

            DataContext = this;
        }
        /// <summary>
        /// Method to navigate to Match and Training page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MatchButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Match and Training page
            Frame.Navigate(typeof(MatchLogPro.MatchLogPro));
        }
        /// <summary>
        /// Method to navigate to Training page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainingButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Training page
            Frame.Navigate(typeof(TrainingLogPro));
        }
        /// <summary>
        /// Method to navigate to View Match Log page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewMatchButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to View Match Log page
            Frame.Navigate(typeof(ViewMatchLogPro.ViewMatchLogPro));
        }
        /// <summary>
        /// Method to navigate to View Training Log page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to View Training Log page
            Frame.Navigate(typeof(ViewTrainingLogPro));
        }
        /// <summary>
        /// Method to navigate to Training Attendance page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainingAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Training Attendance page
            Frame.Navigate(typeof(TrainingAttendancePro));
        }
        /// <summary>
        /// Method to navigate to Attendance Log page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AttendanceLogButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to View Attendance page
            Frame.Navigate(typeof(ViewAttendancePro));
        }
    }
}
