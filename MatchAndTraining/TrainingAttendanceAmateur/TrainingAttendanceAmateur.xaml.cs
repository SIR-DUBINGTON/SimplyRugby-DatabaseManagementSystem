using MySqlConnector;   
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace SimplyRugby.MatchAndTraining.TrainingAttendanceAmateur
{
    /// <summary>
    /// Page for amateur coaches to submit attendance for training sessions.
    /// </summary>
    public sealed partial class TrainingAttendanceAmateur : Page
    {
        ObservableCollection<TrainingRecord.TrainingRecord> trainingSessions = new ObservableCollection<TrainingRecord.TrainingRecord>();

        public TrainingAttendanceAmateur()
        {
            this.InitializeComponent();
            Loaded += Page_Loaded;
        }

        /// <summary>
        /// Method to load the teams for the logged-in coach when the page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTeamsAsync();
        }

        /// <summary>
        /// Method to load the teams for the logged-in coach.
        /// </summary>
        /// <returns></returns>
        private async Task LoadTeamsAsync()
        {
            // Assuming SessionManager.StaffID holds the logged-in coach's staff ID
            int coachStaffID = SessionManager.SessionManager.StaffID;
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT teamID, teamName FROM TeamInfo WHERE coachStaffID = @CoachStaffID ORDER BY teamName;";

            ObservableCollection<Team.Team> teams = new ObservableCollection<Team.Team>();

            try
            {
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
                                teams.Add(new Team.Team
                                {
                                    TeamID = reader.GetInt32("teamID"),
                                    TeamName = reader.GetString("teamName")
                                });
                            }
                        }
                    }

                    TeamComboBox.ItemsSource = teams;
                    if (teams.Count > 0)
                        TeamComboBox.SelectedIndex = 0; // Optionally select the first team by default
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load teams: " + ex.Message);
                // Optionally, handle exceptions such as displaying a message to the user
            }
        }
        /// <summary>
        /// Method to load the training sessions for the selected team.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private async Task LoadTrainingSessionsAsync(int teamId)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT trainingID, trainingDate, trainingTime, trainingLocation, skillsActivities, accidentsInjuries FROM TrainingRecordSheet WHERE teamID = @TeamID ORDER BY trainingDate DESC;";

            trainingSessions.Clear(); // Clear existing sessions before loading new ones

            try
            {
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
                                var session = new TrainingRecord.TrainingRecord
                                {
                                    TrainingID = reader.GetInt32("trainingID"),
                                    TrainingDate = reader.GetDateTime("trainingDate"),
                                    TrainingTime = reader.GetTimeSpan("trainingTime"),
                                    TrainingLocation = reader.GetString("trainingLocation"),
                                    SkillsActivities = reader.GetString("skillsActivities"),
                                    AccidentsInjuries = reader.GetString("accidentsInjuries")
                                };
                                trainingSessions.Add(session);
                            }
                        }
                    }

                    TrainingSessionsComboBox.ItemsSource = trainingSessions;
                    if (trainingSessions.Count > 0)
                        TrainingSessionsComboBox.SelectedIndex = 0; // Optionally select the first session by default
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load training sessions: " + ex.Message);
            }
        }
        /// <summary>
        /// UI update when a team is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TeamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TeamComboBox.SelectedValue is int selectedTeamId)
            {
                await LoadTrainingSessionsAsync(selectedTeamId);
                await LoadPlayersForTeamAsync(selectedTeamId);
            }
        }
        /// <summary>
        /// Method to load the players for the selected team.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private async Task LoadPlayersForTeamAsync(int teamId)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT sruNumber, playerName FROM PlayerInfo WHERE teamID = @TeamID;";

            var players = new ObservableCollection<Player.Player>();

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
                            players.Add(new Player.Player
                            {
                                SruNumber = reader.GetInt32("sruNumber"),
                                PlayerName = reader.GetString("playerName")
                            });
                        }
                    }
                }
            }

            PlayerComboBox.ItemsSource = players;
            if (players.Count > 0)
                PlayerComboBox.SelectedIndex = 0; // Optionally select the first player by default
        }
        /// <summary>
        /// Submit attendance for the selected player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitAttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeamComboBox.SelectedValue == null || TrainingSessionsComboBox.SelectedValue == null || PlayerComboBox.SelectedValue == null)
            {
                ShowMessage("Please select a team, a training session and a player.");
                return;
            }

            // Retrieve the selected training session details
            var selectedTrainingSession = (TrainingRecord.TrainingRecord)TrainingSessionsComboBox.SelectedItem;
            DateTime trainingDate = selectedTrainingSession.TrainingDate;

            // Validate that the training date is not in the future
            if (trainingDate.Date > DateTime.Today)
            {
                ShowMessage("The training date cannot be in the future.");
                return;
            }

            int selectedTrainingId = (int)TrainingSessionsComboBox.SelectedValue;
            int selectedPlayerId = (int)PlayerComboBox.SelectedValue;

            await SaveAttendanceAsync(selectedTrainingId, selectedPlayerId);

            ShowMessage("Attendance for the selected player has been successfully submitted.");
            Frame.Navigate(typeof(MatchAndTrainingAmateur.MatchAndTrainingAmateur));
        }
        /// <summary>
        /// Method to save the attendance for the selected player.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <param name="sruNumber"></param>
        /// <returns></returns>
        private async Task SaveAttendanceAsync(int trainingId, int sruNumber)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "INSERT INTO TrainingAttendance (trainingID, sruNumber) VALUES (@TrainingID, @SRUNumber);";

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TrainingID", trainingId);
                    command.Parameters.AddWithValue("@SRUNumber", sruNumber);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        /// <summary>
        /// Method to show a message to the user.
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
