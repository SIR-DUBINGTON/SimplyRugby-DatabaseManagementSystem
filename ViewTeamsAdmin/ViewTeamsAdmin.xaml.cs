using System;
using System.Collections.Generic;
using System.IO;
using MySqlConnector;
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
using System.Collections.ObjectModel;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimplyRugby.ViewTeamsAdmin
{
    /// <summary>
    /// Page for viewing all teams in the system for an admin
    /// </summary>
    public sealed partial class ViewTeamsAdmin : Page
    {
        public ObservableCollection<Team.Team> Teams { get; set; } = new ObservableCollection<Team.Team>();
        public ViewTeamsAdmin()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
        }
        /// <summary>
        /// Method to load the method to load all teams from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await LoadTeamsAsync();
            TeamsListView.ItemsSource = Teams;
        }
        /// <summary>
        /// Method to load all teams from the database
        /// </summary>
        /// <returns></returns>
        private async Task LoadTeamsAsync()
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = @"
                SELECT TeamInfo.teamID, TeamInfo.teamName, TeamInfo.ageGroup, Staff.staffName AS coachName
                FROM TeamInfo
                JOIN Staff ON TeamInfo.coachStaffID = Staff.staffID;";

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
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
