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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimplyRugby.MatchAndTraining.TrainingLogPro
{
    /// <summary>
    /// Page for professional coaches to log training records
    /// </summary>
    public sealed partial class TrainingLogPro : Page
    {
        public ObservableCollection<Team.Team> Teams { get; set; } = new ObservableCollection<Team.Team>();

        public TrainingLogPro()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;

        }
        /// <summary>
        /// Method to load teams for the current coach when the page is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTeamsAsync();
        }
        /// <summary>
        /// Method to load teams for the current coach
        /// </summary>
        /// <returns></returns>
        private async Task LoadTeamsAsync()
        {
            int currentCoachId = SessionManager.SessionManager.StaffID;  // Assuming this retrieves the current coach's ID correctly
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT teamID, teamName FROM TeamInfo WHERE coachStaffID = @CoachStaffID ORDER BY teamName ASC;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CoachStaffID", currentCoachId);  // Use the current coach's ID to filter teams

                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            Teams.Clear(); // Clear existing items
                            while (await reader.ReadAsync())
                            {
                                var team = new Team.Team
                                {
                                    TeamID = reader.GetInt32("teamID"),
                                    TeamName = reader.IsDBNull(reader.GetOrdinal("teamName")) ? "Unnamed Team" : reader.GetString("teamName")
                                };
                                Teams.Add(team);
                            }
                        }
                    }

                    // Update the UI on the main thread
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        TeamComboBox.ItemsSource = Teams;
                        if (Teams.Count > 0)
                            TeamComboBox.SelectedIndex = 0; // Optionally select the first team by default
                    });
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log or show to user)
                Debug.WriteLine("Failed to load teams: " + ex.Message);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    // Optionally inform the user of the failure on UI thread
                });
            }
        }

        /// <summary>
        /// Method to insert a training record into the database
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private async Task InsertTrainingRecordAsync(TrainingRecord.TrainingRecord record)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = @"
        INSERT INTO TrainingRecordSheet (
            coachStaffID, teamID, trainingDate, trainingLocation, trainingTime, skillsActivities, accidentsInjuries
        ) VALUES (
            @CoachStaffID, @teamID, @TrainingDate, @TrainingLocation, @TrainingTime, @SkillsActivities, @AccidentsInjuries
        );";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CoachStaffID", record.CoachStaffID);
                        command.Parameters.AddWithValue("@TrainingDate", record.TrainingDate);
                        command.Parameters.AddWithValue("@TrainingLocation", record.TrainingLocation);
                        command.Parameters.AddWithValue("@TrainingTime", record.TrainingTime);
                        command.Parameters.AddWithValue("@SkillsActivities", record.SkillsActivities);
                        command.Parameters.AddWithValue("@AccidentsInjuries", record.AccidentsInjuries);
                        command.Parameters.AddWithValue("@TeamID", record.TeamID); // Make sure to capture this from the ComboBox
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to insert training record: " + ex.Message);
                // Optionally, handle exceptions such as displaying a message to the user
            }
        }

        /// <summary>
        /// Method to handle the submission of a training record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var trainingRecord = new TrainingRecord.TrainingRecord
            {
                CoachStaffID = SessionManager.SessionManager.StaffID,  // Assuming you have a SessionManager to fetch current user's details
                TrainingDate = TrainingDateDatePicker.Date.DateTime,
                TrainingLocation = TrainingLocationTextBox.Text,
                TrainingTime = TrainingTimePicker.Time,
                SkillsActivities = SkillsActivitiesTextBox.Text,
                AccidentsInjuries = AccidentsInjuriesTextBox.Text,
                TeamID = (int)TeamComboBox.SelectedValue
            };
            if (TrainingDateDatePicker.Date == null || TrainingTimePicker.Time == null || TrainingLocationTextBox.Text == "" || SkillsActivitiesTextBox.Text == "" || AccidentsInjuriesTextBox.Text == "" || TeamComboBox.SelectedValue == null)
            {
                ContentDialog missingFieldsDialog = new ContentDialog
                {
                    Title = "Missing Fields",
                    Content = "Please fill in all fields before submitting the training record.",
                    CloseButtonText = "OK"
                };
                await missingFieldsDialog.ShowAsync();
                return;
            }


            // Validate that the training date is not in the future
            if (trainingRecord.TrainingDate.Date > DateTime.Today)
            {
                ContentDialog futureDateDialog = new ContentDialog
                {
                    Title = "Invalid Date",
                    Content = "The training date cannot be in the future. Please select a valid date.",
                    CloseButtonText = "OK"
                };
                await futureDateDialog.ShowAsync();
                return;
            }

            await InsertTrainingRecordAsync(trainingRecord);

            // Optionally, clear the form fields after submission or navigate away
            ClearForm();
            // Display success message or handle any post-submission logic
            ShowSuccessMessage("Training record submitted successfully.");
            // Display success message or handle any post-submission logic
            Frame.Navigate(typeof(MatchAndTrainingAmateur.MatchAndTrainingPro));

        }

        /// <summary>
        /// Method to clear the form fields
        /// </summary>
        private void ClearForm()
        {
            TrainingLocationTextBox.Text = string.Empty;
            SkillsActivitiesTextBox.Text = string.Empty;
            AccidentsInjuriesTextBox.Text = string.Empty;
            TrainingDateDatePicker.Date = DateTimeOffset.Now;
            TrainingTimePicker.Time = new TimeSpan(0);
        }
        private void ShowSuccessMessage(string message)
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "Success",
                Content = message,
                CloseButtonText = "OK"
            };
            successDialog.ShowAsync();
        }
    }
}
