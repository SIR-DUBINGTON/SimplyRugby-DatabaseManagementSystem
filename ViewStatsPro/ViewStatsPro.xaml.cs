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

namespace SimplyRugby.ViewStatsPro
{
    /// <summary>
    /// Page for viewing player stats for a pro coach
    /// </summary>
    public sealed partial class ViewStatsPro : Page
    {
        public ObservableCollection<PlayerStats.PlayerStats> PlayerStatsList { get; } = new ObservableCollection<PlayerStats.PlayerStats>();
        public ObservableCollection<Player.Player> Players { get; } = new ObservableCollection<Player.Player>();
        public ViewStatsPro()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// method to update the combobox with the players of the coach
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PopulatePlayersAsync(SessionManager.SessionManager.StaffID).ConfigureAwait(false);// Assuming SessionManager is a static class, if not adjust accordingly
            PlayerStatsList.Clear(); // Clear any existing stats to ensure the list starts empty
        }
        /// <summary>
        /// method to populate the combobox with the players of the coach
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        private async Task PopulatePlayersAsync(int staffId)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string queryString = @"
            SELECT sruNumber, playerName 
            FROM PlayerInfo 
            WHERE coachStaffID = @StaffID;";

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@StaffID", staffId);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Players.Add(new Player.Player
                            {
                                SruNumber = reader.GetInt32("sruNumber"),
                                PlayerName = reader.GetString("playerName")
                            });
                        }
                        PlayerSelectionComboBox.ItemsSource = Players;
                    }
                }
            }
        }
        /// <summary>
        /// Method to update the ui combobox the stats of the selected player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void ViewStatsButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerSelectionComboBox.SelectedValue != null)
            {
                int selectedPlayerId = (int)PlayerSelectionComboBox.SelectedValue;
                await LoadPlayerStatsAsync(selectedPlayerId);
            }
            else
            {
                // Notify user to select a player
            }
        }
        /// <summary>
        /// Method to load the stats of the selected player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        private async Task LoadPlayerStatsAsync(int playerId)
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;Allow User Variables=true";
            string queryString = @"
            SELECT pi.playerName, p.skillCategory, p.skill, p.skillLevel, p.skillComments, p.position
            FROM PlayerProfileSheet AS p
            INNER JOIN PlayerInfo AS pi ON p.sruNumber = pi.sruNumber
            WHERE p.sruNumber = @PlayerID;";  // Correct // Correctly identifying the field to match against

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@PlayerID", playerId);  // Correcting the parameter name
                    await connection.OpenAsync();
                    using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        PlayerStatsList.Clear();  // Clear existing entries to avoid duplication
                        while (await reader.ReadAsync())
                        {
                            PlayerStatsList.Add(new PlayerStats.PlayerStats
                            {
                                // Assuming that the reader's order of fields is correctly matched here:
                                PlayerName = reader.GetString(0),
                                SkillCategory = reader.GetString(1),
                                Skill = reader.GetString(2),
                                SkillLevel = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                SkillComments = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                Position = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            StatsListView.ItemsSource = PlayerStatsList;  // This ensures the ListView is updated after loading new data
        }
    }
}
