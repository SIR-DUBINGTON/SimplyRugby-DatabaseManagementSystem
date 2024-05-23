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

namespace SimplyRugby.ViewTeamsPro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewTeamsPro : Page
    {
        public ObservableCollection<Team.Team> Teams { get; set; } = new ObservableCollection<Team.Team>();

        public ViewTeamsPro()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;

        }
        /// <summary>
        /// Method to load the method to load teams of the current staff member
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await LoadTeamsAsync();
            TeamsListView.ItemsSource = Teams;
        }
        /// <summary>
        /// Method to load the teams of the current staff member
        /// </summary>
        /// <returns></returns>
        private async Task LoadTeamsAsync()
        {
            // Retrieve the currently logged-in staff's StaffID from the SessionManager.
            int currentStaffId = SessionManager.SessionManager.StaffID;

            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = @"
            SELECT TeamInfo.teamID, TeamInfo.teamName, TeamInfo.ageGroup, Staff.staffName AS coachName
            FROM TeamInfo
            JOIN Staff ON TeamInfo.coachStaffID = Staff.staffID
            WHERE Staff.staffID = @currentStaffId;"; // Filter by the current staff's StaffID

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    // Add the current staff's StaffID as a parameter to the query
                    command.Parameters.AddWithValue("@currentStaffId", currentStaffId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Teams.Clear(); // Clear the existing teams (if refreshing the list)

                        while (await reader.ReadAsync())
                        {
                            Teams.Add(new Team.Team
                            {
                                TeamID = reader.GetInt32("teamID"), // Assuming the column name is "teamID"
                                TeamName = reader.GetString("teamName"),
                                AgeGroup = reader.GetString("ageGroup"),
                                CoachName = reader.GetString("coachName")
                            });
                        }
                    }
                }
            }
        }
    }
}
