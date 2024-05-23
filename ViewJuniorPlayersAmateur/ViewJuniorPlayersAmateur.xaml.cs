using MySqlConnector; // Make sure this using directive is at the top
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

namespace SimplyRugby.ViewJuniorPlayersAmateur
{
    /// <summary>
    /// Page to view junior players for an amateur coach
    /// </summary>
    public sealed partial class ViewJuniorPlayersAmateur : Page
    {
        public ObservableCollection<Player.Player> JuniorPlayers { get; } = new ObservableCollection<Player.Player>();
        public ViewJuniorPlayersAmateur()
        {
            this.InitializeComponent();

            this.Loaded += ViewJuniorPlayersAmateur_Loaded;
        }
        /// <summary>
        /// Method to load method to load junior players for an amateur coach
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ViewJuniorPlayersAmateur_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await LoadJuniorPlayersAsync();
        }
        /// <summary>
        /// Method to load junior players for an amateur coach
        /// </summary>
        /// <returns></returns>
        private async Task LoadJuniorPlayersAsync()
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            int coachStaffID = SessionManager.SessionManager.StaffID; // Assuming you have a session manager that holds the currently logged-in coach's ID
            string queryString = @"
            SELECT pi.sruNumber, pi.playerName, pi.dob, pi.address, pi.contactNo, pi.postcode, pi.email, pi.knownHealthIssues,
                   gi.guardianName, gi.phoneNumber, gi.relationship, gi.address AS guardianAddress,
                   ti.teamName, st.staffName AS CoachName
            FROM PlayerInfo pi
            JOIN PlayerGuardian pg ON pi.sruNumber = pg.sruNumber
            JOIN GuardianInfo gi ON pg.guardianID = gi.guardianID
            JOIN TeamInfo ti ON pi.teamID = ti.teamID
            JOIN Staff st ON pi.coachStaffID = st.staffID
            WHERE ti.ageGroup = 'Junior' AND pi.coachStaffID = @CoachStaffID;"; // Filter by coach's staff ID

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = new MySqlCommand(queryString, connection))
                    {
                        command.Parameters.AddWithValue("@CoachStaffID", coachStaffID); // Safely pass the coach's staff ID to the query
                        await connection.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                JuniorPlayers.Add(new Player.Player
                                {
                                    SruNumber = reader.GetInt32("sruNumber"),
                                    PlayerName = reader.GetString("playerName"),
                                    Dob = reader.GetDateTime("dob"),
                                    Address = reader.GetString("address"),
                                    ContactNo = reader.GetString("contactNo"),
                                    Postcode = reader.GetString("postcode"),
                                    Email = reader.GetString("email"),
                                    KnownHealthIssues = reader.GetString("knownHealthIssues"),
                                    GuardianName = reader.GetString("guardianName"),
                                    GuardianContactNo = reader.GetString("phoneNumber"),
                                    GuardianRelationship = reader.GetString("relationship"),
                                    GuardianAddress = reader.GetString("guardianAddress"),
                                    TeamName = reader.GetString("teamName"),
                                    CoachName = reader.GetString("CoachName")
                                    // Populate other fields as necessary
                                });
                            }
                        }
                    }
                    JuniorPlayersListView.ItemsSource = JuniorPlayers;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // Log the error or show an error message
            }
        }

    }
}
