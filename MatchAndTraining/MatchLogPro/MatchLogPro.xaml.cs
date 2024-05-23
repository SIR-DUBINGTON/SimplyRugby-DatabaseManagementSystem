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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimplyRugby.MatchAndTrainingAmateur.MatchLogPro
{
    /// <summary>
    /// A page for coaches to log match details.
    /// </summary>
    public sealed partial class MatchLogPro : Page
    {
        public MatchLogPro()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;

        }
        /// <summary>
        /// A method that is called when the page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCoachedTeamsAsync();
        }

        /// <summary>
        /// A method that loads the teams that the current coach is coaching.
        /// </summary>
        /// <returns></returns>
        private async Task LoadCoachedTeamsAsync()
        {
            int currentCoachId = SessionManager.SessionManager.StaffID; // Retrieve current coach's staff ID
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT teamID, teamName FROM TeamInfo WHERE coachStaffID = @CoachStaffID;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CoachStaffID", currentCoachId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var teams = new ObservableCollection<Team.Team>();
                            while (await reader.ReadAsync())
                            {
                                teams.Add(new Team.Team
                                {
                                    TeamID = reader.GetInt32("teamID"),
                                    TeamName = reader.GetString("teamName")
                                });
                            }

                            // Update the ComboBox on the UI thread
                            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                TeamComboBox.ItemsSource = teams;
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ShowErrorMessage(ex.Message);
            }
        }
        /// <summary>
        /// A method that is called when the SubmitScores button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitScores_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve form data
            string oppositionName = OppositionNameTextBox.Text;
            DateTime dateOfMatch = DateOfFixturePicker.Date.Date;
            TimeSpan kickoffTime = KickoffTimePicker.Time;
            string location = (LocationComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string result = (ResultComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string finalScore = FinalScoreTextBox.Text;
            string homeFirstHalfScoreText = HomeFirstHalfScoreTextBox.Text;
            string awayFirstHalfScoreText = AwayFirstHalfScoreTextBox.Text;
            string homeSecondHalfScoreText = HomeSecondHalfScoreTextBox.Text;
            string awaySecondHalfScoreText = AwaySecondHalfScoreTextBox.Text;
            string gameComments = GameCommentsTextBox.Text;
            // Initial validation for empty fields
            if (string.IsNullOrWhiteSpace(oppositionName) || string.IsNullOrWhiteSpace(location) ||
                string.IsNullOrWhiteSpace(result) || string.IsNullOrWhiteSpace(finalScore) ||
                string.IsNullOrWhiteSpace(homeFirstHalfScoreText) || string.IsNullOrWhiteSpace(awayFirstHalfScoreText) ||
                string.IsNullOrWhiteSpace(homeSecondHalfScoreText) || string.IsNullOrWhiteSpace(awaySecondHalfScoreText))
            {
                ShowErrorMessage("Please fill in all score fields.");
                return;
            }

            // Validate that score fields are integers
            if (!int.TryParse(homeFirstHalfScoreText, out int homeFirstHalfScore) ||
                !int.TryParse(awayFirstHalfScoreText, out int awayFirstHalfScore) ||
                !int.TryParse(homeSecondHalfScoreText, out int homeSecondHalfScore) ||
                !int.TryParse(awaySecondHalfScoreText, out int awaySecondHalfScore))
            {
                ShowErrorMessage("All score fields must contain valid numbers.");
                return;
            }

            // Assuming TeamComboBox is bound to a collection of Team objects
            if (TeamComboBox.SelectedItem is Team.Team selectedTeam)
            {
                int teamID = selectedTeam.TeamID; // Get the selected team's ID correctly

                string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
                string insertQuery = @"
            INSERT INTO GameRecordSheet (
                teamID, oppositionName, dateOfMatch, location, koTime, result, score, 
                firstHalfSimplyRugbyPoints, firstHalfOppositionPoints, 
                secondHalfSimplyRugbyPoints, secondHalfOppositionPoints, gameComments
            ) VALUES (
                @TeamID, @OppositionName, @DateOfMatch, @Location, @KoTime, @Result, @Score, 
                @FirstHalfSimplyRugbyPoints, @FirstHalfOppositionPoints, 
                @SecondHalfSimplyRugbyPoints, @SecondHalfOppositionPoints, @GameComments
            );";

                try
                {
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        using (var command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TeamID", teamID);
                            command.Parameters.AddWithValue("@OppositionName", oppositionName);
                            command.Parameters.AddWithValue("@DateOfMatch", dateOfMatch);
                            command.Parameters.AddWithValue("@Location", location);
                            command.Parameters.AddWithValue("@KoTime", kickoffTime);
                            command.Parameters.AddWithValue("@Result", result);
                            command.Parameters.AddWithValue("@Score", finalScore);
                            command.Parameters.AddWithValue("@FirstHalfSimplyRugbyPoints", homeFirstHalfScore);
                            command.Parameters.AddWithValue("@FirstHalfOppositionPoints", awayFirstHalfScore);
                            command.Parameters.AddWithValue("@SecondHalfSimplyRugbyPoints", homeSecondHalfScore);
                            command.Parameters.AddWithValue("@SecondHalfOppositionPoints", awaySecondHalfScore);
                            command.Parameters.AddWithValue("@GameComments", gameComments);

                            int rowsAffected = await command.ExecuteNonQueryAsync(); // Renamed variable

                            if (rowsAffected > 0)
                            {
                                ShowSuccessMessage();
                                Frame.Navigate(typeof(MatchAndTrainingPro));
                            }
                            else
                            {
                                ShowErrorMessage("No rows were inserted.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
            else
            {
                ShowErrorMessage("Please select a team.");
            }
        }

        /// <summary>
        /// A method that shows a success message dialog.
        /// </summary>
        private void ShowSuccessMessage()
        {
            var dialog = new ContentDialog
            {
                Title = "Success",
                Content = "The match log has been successfully submitted.",
                CloseButtonText = "OK"
            };

            dialog.ShowAsync();
        }

        /// <summary>
        /// A method that shows an error message dialog.
        /// </summary>
        /// <param name="message"></param>
        private void ShowErrorMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK"
            };

            dialog.ShowAsync();
        }
    }
}
