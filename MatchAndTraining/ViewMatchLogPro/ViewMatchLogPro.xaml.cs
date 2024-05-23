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

namespace SimplyRugby.MatchAndTrainingAmateur.ViewMatchLogPro
{
    /// <summary>
    /// Page for viewing match logs for a pro coach's team.
    /// </summary>
    public sealed partial class ViewMatchLogPro : Page
    {
        public ObservableCollection<Team.Team> Teams { get; set; } = new ObservableCollection<Team.Team>();
        public ObservableCollection<GameOption.GameOption> Games { get; set; } = new ObservableCollection<GameOption.GameOption>();
        public ViewMatchLogPro()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;

        }
        /// <summary>
        /// Method to load the teams for the current coach when the page is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTeamsAsync();
        }

        /// <summary>
        /// Method to load the teams for the current coach.
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
        /// Method to load the game dates for the selected team.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TeamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TeamComboBox.SelectedValue is int teamId)
            {
                await LoadGameDatesAsync(teamId);
            }
            else
            {
                Debug.WriteLine("SelectedValue is not an integer or is null");
            }
        }
        /// <summary>
        /// Method to load the game dates for the selected team.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        private async Task LoadGameDatesAsync(int teamId)
        {
            int currentCoachId = SessionManager.SessionManager.StaffID;  // Assuming this retrieves the current coach's ID correctly
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = @"
            SELECT gr.gameID, gr.dateOfMatch, ti.teamName 
            FROM GameRecordSheet gr 
            JOIN TeamInfo ti ON gr.teamID = ti.teamID
            WHERE gr.teamID = @teamId 
            ORDER BY gr.dateOfMatch DESC;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@teamId", teamId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            Games.Clear(); // Clear existing items
                            while (await reader.ReadAsync())
                            {
                                Games.Add(new GameOption.GameOption
                                {
                                    GameID = reader.GetInt32("gameID"),
                                    GameDate = reader.GetDateTime("dateOfMatch"),
                                    TeamName = reader.GetString("teamName") // Store the team name from the query
                                });
                            }
                        }
                    }

                    // Update the UI on the main thread
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        DateComboBox.ItemsSource = Games;
                        if (Games.Count > 0)
                            DateComboBox.SelectedIndex = 0; // Optionally select the first date by default
                        else
                            DateComboBox.ItemsSource = null; // Handle no game dates available
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load game dates: " + ex.Message);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    // Optionally inform the user of the failure on UI thread
                });
            }
        }
        /// <summary>
        /// Method to load the game details for the selected game when search button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateComboBox.SelectedValue is int gameId)
            {
                await LoadGameDetailsAsync(gameId);
            }
            else
            {
                //ShowErrorMessage("Please select a date for the game.");
            }
        }
        /// <summary>
        /// Method to load the game details for the selected game.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        private async Task LoadGameDetailsAsync(int gameId)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = $@"
            SELECT gr.*, ti.teamName
            FROM GameRecordSheet gr
            JOIN TeamInfo ti ON gr.teamID = ti.teamID
            WHERE gr.gameID = @gameId;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@gameId", gameId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync()) // Assuming each game ID corresponds to a single record
                            {
                                // Assuming you have a data model to hold these values
                                UpdateGameDetailsUI(new MatchRecord.MatchRecord
                                {
                                    TeamName = reader.GetString("teamName"),
                                    DateOfMatch = reader.GetDateTime("dateOfMatch"),
                                    OppositionName = reader.GetString("oppositionName"),
                                    Location = reader.GetString("location"),
                                    KickoffTime = reader.GetTimeSpan("koTime").ToString(@"hh\:mm"),
                                    Result = reader.GetString("result"),
                                    Score = reader.GetString("score"),
                                    FirstHalfHomePoints = reader.GetInt32("firstHalfSimplyRugbyPoints"),
                                    FirstHalfAwayPoints = reader.GetInt32("firstHalfOppositionPoints"),
                                    SecondHalfHomePoints = reader.GetInt32("secondHalfSimplyRugbyPoints"),
                                    SecondHalfAwayPoints = reader.GetInt32("secondHalfOppositionPoints"),
                                    GameComments = reader.GetString("gameComments")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load game details: {ex.Message}");
                // Optionally handle UI updates to reflect errors
            }
        }
        /// <summary>
        /// Method to update the game details UI.
        /// </summary>
        /// <param name="details"></param>
        private void UpdateGameDetailsUI(MatchRecord.MatchRecord details)
        {
            TeamNameTextBlock.Text = details.TeamName;
            GameDateTextBlock.Text = details.DateOfMatch.ToString("yyyy-MM-dd");
            OppositionTeamTextBlock.Text = details.OppositionName;
            GameLocationTextBlock.Text = details.Location;
            GameTimeTextBlock.Text = details.KickoffTime;
            GameResultTextBlock.Text = details.Result;
            GameScoreTextBlock.Text = details.Score;
            firstHalfSimplyRugbyPointsTextBlock.Text = details.FirstHalfHomePoints.ToString();
            firstHalfOpponentPointsTextBlock.Text = details.FirstHalfAwayPoints.ToString();
            secondHalfSimplyRugbyPointsTextBlock.Text = details.SecondHalfHomePoints.ToString();
            secondHalfOpponentPointsTextBlock.Text = details.SecondHalfAwayPoints.ToString();
            GameCommentsTextBlock.Text = details.GameComments;
            // Update other UI elements as necessary
        }

    }
}
