using MySqlConnector; // Make sure this using directive is at the top
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SimplyRugby.ViewPlayersPro
{
    /// <summary>
    /// Page for viewing players for a professional coach
    /// </summary>
    public sealed partial class ViewPlayersPro : Page
    {
        public ObservableCollection<Player.Player> Players { get; } = new ObservableCollection<Player.Player>();
        public ViewPlayersPro()
        {
            this.InitializeComponent();
            this.Loaded += ViewPlayersPro_Loaded;

        }
        /// <summary>
        /// Method to load the players for the current professional coach
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ViewPlayersPro_Loaded(object sender, RoutedEventArgs e)
        {
            int currentStaffId = SessionManager.SessionManager.StaffID;
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string queryString = @"
            SELECT 
                PlayerInfo.*, 
                TeamInfo.teamName, 
                Staff.staffName AS CoachName
            FROM PlayerInfo
            JOIN TeamInfo ON PlayerInfo.teamID = TeamInfo.teamID
            JOIN Staff ON PlayerInfo.coachStaffID = Staff.staffID
            WHERE PlayerInfo.coachStaffID = @StaffID AND TeamInfo.ageGroup = 'Senior';"; // Only select senior players

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@StaffID", currentStaffId);
                    await connection.OpenAsync();
                    using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        Players.Clear(); // Clear existing entries to ensure the list is refreshed
                        while (await reader.ReadAsync())
                        {
                            Players.Add(new Player.Player
                            {
                                SruNumber = reader.GetInt32("sruNumber"),
                                PlayerName = reader.GetString("playerName"),
                                Address = reader.GetString("address"),
                                Dob = reader.GetDateTime("dob"),
                                ContactNo = reader.GetString("contactNo"),
                                Postcode = reader.GetString("postcode"),
                                Email = reader.GetString("email"),
                                KnownHealthIssues = reader.GetString("knownHealthIssues"),
                                NextOfKin = reader.IsDBNull(reader.GetOrdinal("NextOfKin")) ? null : reader.GetString("NextOfKin"),
                                NextOfKinContactNo = reader.IsDBNull(reader.GetOrdinal("NextOfKinContactNo")) ? null : reader.GetString("NextOfKinContactNo"),
                                TeamName = reader.GetString("teamName"),
                                CoachName = reader.GetString("CoachName") // Retrieve the coach's name
                            });
                        }
                    }
                }
            }

            PlayersListView.ItemsSource = Players;
        }

    }
}
