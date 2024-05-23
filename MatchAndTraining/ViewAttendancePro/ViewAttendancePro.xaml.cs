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

namespace SimplyRugby.MatchAndTraining.ViewAttendancePro
{
    /// <summary>
    /// Page for viewing attendance of players in a training session.
    /// </summary>
    public sealed partial class ViewAttendancePro : Page
    {
        ObservableCollection<TrainingRecord.TrainingRecord> trainingSessions = new ObservableCollection<TrainingRecord.TrainingRecord>();
        ObservableCollection<Player.Player> attendanceList = new ObservableCollection<Player.Player>();
        public ViewAttendancePro()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;

        }
        /// <summary>
        /// Method to load the method teams for the logged-in coach.
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
        /// UI UPDATE: Method to load the training sessions for the selected team.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TeamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TeamComboBox.SelectedValue is int teamId)
            {
                await LoadTrainingSessionsAsync(teamId);

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
        /// Method to load the attendance of players for the selected training session.
        /// </summary>
        /// <param name="trainingId"></param>
        /// <returns></returns>
        private async Task LoadAttendanceAsync(int trainingId)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = @"
            SELECT p.playerName, p.sruNumber
            FROM TrainingAttendance ta
            JOIN PlayerInfo p ON ta.sruNumber = p.sruNumber
            WHERE ta.trainingID = @TrainingID;";


            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TrainingID", trainingId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                attendanceList.Add(new Player.Player
                                {
                                    PlayerName = reader.GetString("playerName"),
                                    SruNumber = reader.GetInt32("sruNumber")
                                });
                            }
                        }
                    }
                }

                // Assuming AttendanceListView is your ListView in XAML
                AttendanceListView.ItemsSource = attendanceList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load attendance data: " + ex.Message);
                // Optionally, handle exceptions such as displaying a message to the user
            }
        }
        /// <summary>
        /// Method to search for the attendance of players for the selected training session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeamComboBox.SelectedValue is int teamId)
            {
                if (TrainingSessionsComboBox.SelectedValue is int trainingId)
                {
                    // Clears previous data to avoid duplication
                    attendanceList.Clear();

                    // Load attendance data for the selected training session
                    await LoadAttendanceAsync(trainingId);
                }
                else
                {
                    Debug.WriteLine("No training session selected.");
                    ShowMessage("Please select a training session.");
                }
            }
            else
            {
                Debug.WriteLine("No team selected.");
                ShowMessage("Please select a team.");
            }
        }

        /// <summary>
        /// Method to display a message to the user.
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
