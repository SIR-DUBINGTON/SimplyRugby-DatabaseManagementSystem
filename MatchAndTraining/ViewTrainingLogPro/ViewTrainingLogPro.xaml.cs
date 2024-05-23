using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace SimplyRugby.MatchAndTraining.ViewTrainingLogPro
{
    /// <summary>
    /// Page for viewing training logs.
    /// </summary>
    public sealed partial class ViewTrainingLogPro : Page
    {
        ObservableCollection<Team.Team> teams = new ObservableCollection<Team.Team>();
        ObservableCollection<TrainingRecord.TrainingRecord> trainingDetails = new ObservableCollection<TrainingRecord.TrainingRecord>();
        public ViewTrainingLogPro()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Method to load the method for the teams for the logged-in coach when the page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTeamsAsync();
            await LoadTrainingDatesAsync(teams[0].TeamID);  // Load dates for the first team

        }
        /// <summary>
        /// Method to load the teams for the logged-in coach.
        /// </summary>
        /// <returns></returns>
        private async Task LoadTeamsAsync()
        {
            int coachStaffID = SessionManager.SessionManager.StaffID; // Assuming this holds the currently logged-in coach's ID
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT teamID, teamName FROM TeamInfo WHERE coachStaffID = @CoachStaffID ORDER BY teamName;";


            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CoachStaffID", coachStaffID);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            teams.Add(new Team.Team { TeamID = reader.GetInt32("teamID"), TeamName = reader.GetString("teamName") });
                        }
                    }
                }

                TeamComboBox.ItemsSource = teams;
                if (teams.Count > 0)
                    TeamComboBox.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Method to load the training dates for the selected team.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private async Task LoadTrainingDatesAsync(int teamId)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT DISTINCT trainingDate FROM TrainingRecordSheet WHERE teamID = @TeamID ORDER BY trainingDate DESC;";

            ObservableCollection<DateTime> trainingDates = new ObservableCollection<DateTime>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeamID", teamId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            trainingDates.Add(reader.GetDateTime("trainingDate"));
                        }
                    }
                }

                DateComboBox.ItemsSource = trainingDates;
                if (trainingDates.Count > 0)
                    DateComboBox.SelectedIndex = 0; // Automatically select the first date
                else
                    DateComboBox.ItemsSource = null; // Clear the date combo box if no dates are found
            }
        }
        /// <summary>
        /// Method to load the training details for the selected team and date.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="trainingDate"></param>
        /// <returns></returns>
        private async Task LoadTrainingDetailsAsync(int teamId, DateTime trainingDate)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT * FROM TrainingRecordSheet WHERE teamID = @TeamID AND DATE(trainingDate) = @TrainingDate;";


            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeamID", teamId);
                    command.Parameters.AddWithValue("@TrainingDate", trainingDate);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            trainingDetails.Add(new TrainingRecord.TrainingRecord
                            {
                                // Populate your TrainingRecord
                                TrainingDate = trainingDate,
                                TrainingLocation = reader.GetString("trainingLocation"),
                                TrainingTime = reader.GetTimeSpan("trainingTime"),
                                SkillsActivities = reader.GetString("skillsActivities"),
                                AccidentsInjuries = reader.GetString("accidentsInjuries")

                            });
                        }
                    }
                }

                TrainingDetailsListView.ItemsSource = trainingDetails;
            }
        }
        /// <summary>
        /// Method to search for the training details for the selected team and date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeamComboBox.SelectedValue == null || DateComboBox.SelectedValue == null)
            {
                ShowMessage("Please select both a team and a date.");
                return;
            }

            int selectedTeamId = (int)TeamComboBox.SelectedValue;
            DateTime selectedDate = (DateTime)DateComboBox.SelectedValue;

            await LoadTrainingDetailsAsync(selectedTeamId, selectedDate);  // Load training details for the selected date and team
        }
        /// <summary>
        /// Method to clear the training details when the date is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearTrainingDetails(); // Clear details when changing the date
        }
        /// <summary>
        /// Method to clear the training details when the team is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TeamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearTrainingDetails(); // Clear details when changing the team

            if (TeamComboBox.SelectedValue != null)
            {
                int selectedTeamId = (int)TeamComboBox.SelectedValue;
                await LoadTrainingDatesAsync(selectedTeamId);
            }
        }
        /// <summary>
        /// Method to clear the training details.
        /// </summary>
        private void ClearTrainingDetails()
        {
            trainingDetails.Clear();
            TrainingDetailsListView.ItemsSource = null; // This clears the ListView displaying training details
        }
        /// <summary>
        /// Method to show a message.
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Notification",
                Content = message,
                CloseButtonText = "OK"
            };
            dialog.ShowAsync();
        }
    }
}
