using System;
using System.Collections.Generic;
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
using MySqlConnector;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using System.Diagnostics;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimplyRugby.PlayerRegisterProCoach
{
    /// <summary>
    /// Page for registering a player by a professional coach
    /// </summary>
    public sealed partial class PlayerRegisterProCoach : Page
    {
        public ObservableCollection<Team.Team> Teams { get; set; } = new ObservableCollection<Team.Team>();

        public PlayerRegisterProCoach()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;

        }

        /// <summary>
        /// Method to load method to load teams for the current coach
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await LoadTeamsAsync();
            // The Dispatcher call ensures we're on the UI thread
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                TeamComboBox.ItemsSource = Teams;
                TeamComboBox.DisplayMemberPath = "TeamName";
                TeamComboBox.SelectedValuePath = "TeamID";
            });
            //Teams.Add(new Team.Team { TeamID = 1, TeamName = "Test Team 1" });
        }
        /// <summary>
        /// Method to load teams for the current coach
        /// </summary>
        /// <returns></returns>
        private async Task LoadTeamsAsync()
        {
            int currentCoachId = SessionManager.SessionManager.StaffID; // Retrieve current coach's staff ID
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";

            // Query only for teams where the current coach is assigned
            string query = "SELECT teamID, teamName FROM TeamInfo WHERE coachStaffID = @CoachStaffID AND ageGroup = 'Senior' ORDER BY teamName ASC;";
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
                            Teams.Clear(); // Clear existing items
                            while (await reader.ReadAsync())
                            {
                                Teams.Add(new Team.Team
                                {
                                    TeamID = reader.GetInt32("teamID"),
                                    TeamName = reader.GetString("teamName")
                                });
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
                Debug.WriteLine("Failed to load teams: " + ex.ToString()); // Log the exception
                                                                           // Optionally display a message to the user indicating that team loading failed
            }
        }
        /// <summary>
        /// Method to register a player then navigate to the player development centre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RegisterPlayer_Click(object sender, RoutedEventArgs e)
        {
            // Form data retrieval
            string name = NameTextBox.Text;
            string email = EmailTextBox.Text;
            string postcode = PostcodeTextBox.Text;
            string address = AddressTextBox.Text;
            var dob = DobDatePicker.Date.DateTime;
            string contactNo = ContactNumberTextBox.Text;
            string doctorName = DoctorNameTextBox.Text;
            string doctorContact = DoctorContactNoTextBox.Text;
            string doctorAddress = DoctorAddressTextBox.Text;
            string nextOfKin = NextOfKinTextBox.Text;
            string nextOfKinContact = NextOfKinContactNumberTextBox.Text;
            string healthConditions = HealthConditionsTextBox.Text;


            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(postcode) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(contactNo) || string.IsNullOrWhiteSpace(doctorName) || string.IsNullOrWhiteSpace(doctorContact) || string.IsNullOrWhiteSpace(doctorAddress) || string.IsNullOrWhiteSpace(nextOfKin) || string.IsNullOrWhiteSpace(nextOfKinContact) || string.IsNullOrWhiteSpace(healthConditions))
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please fill in all fields.",
                    CloseButtonText = "Ok"
                };

                await errorDialog.ShowAsync();
                return;
            }

            // Validate email to contain "@"
            if (!email.Contains("@"))
            {
                var emailFailureDialog = new ContentDialog
                {
                    Title = "Registration Failed",
                    Content = "Please enter a valid email address.",
                    CloseButtonText = "OK"
                };
                await emailFailureDialog.ShowAsync();
                return;
            }

            // Validate age to be under 18
            if ((DateTime.Now - dob).TotalDays / 365.25 < 18)
            {
                var ageFailureDialog = new ContentDialog
                {
                    Title = "Registration Failed",
                    Content = "Player must be over 18 years of age.",
                    CloseButtonText = "OK"
                };
                await ageFailureDialog.ShowAsync();
                return;
            }

            if (TeamComboBox.SelectedValue == null)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please select a team.",
                    CloseButtonText = "Ok"
                };

                await errorDialog.ShowAsync();
                return;
            }

            if (contactNo.Length != 11)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Invalid Contact Number",
                    Content = "Please enter a valid contact number.",
                    CloseButtonText = "Ok"
                };

                await errorDialog.ShowAsync();
                return;
            }

            var connectionString = "server=localhost;user=root;password=;database=simplyrugby;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Begin a transaction for inserting into multiple tables
                    using (var transaction = connection.BeginTransaction())
                    {
                        var selectedTeamId = TeamComboBox.SelectedValue as int?;

                        // Insert into PlayerInfo
                        var playerInfoQuery = @"INSERT INTO PlayerInfo (
                                            playerName, address, dob, contactNo, postcode, email,
                                            knownHealthIssues, NextOfKin, NextOfKinContactNo, teamID, coachStaffID
                                        ) VALUES (
                                            @Name, @Address, @DOB, @ContactNo, @Postcode, @Email,
                                            @HealthConditions, @NextOfKin, @NextOfKinContactNo, @TeamID, @coachStaffID
                                        );";
                        long sruNumber;
                        using (var command = new MySqlCommand(playerInfoQuery, connection, transaction))
                        {
                            // Add parameters and execute the command
                            // ...
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@Address", address);
                            command.Parameters.AddWithValue("@DOB", dob);
                            command.Parameters.AddWithValue("@ContactNo", contactNo);
                            command.Parameters.AddWithValue("@Postcode", postcode);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@HealthConditions", healthConditions);
                            command.Parameters.AddWithValue("@NextOfKin", nextOfKin);
                            command.Parameters.AddWithValue("@NextOfKinContactNo", nextOfKinContact);
                            command.Parameters.AddWithValue("@TeamID", selectedTeamId.Value);
                            command.Parameters.AddWithValue("@coachStaffID", SessionManager.SessionManager.StaffID);
                            await command.ExecuteNonQueryAsync();
                            sruNumber = command.LastInsertedId; // Capture the auto-generated sruNumber
                        }

                        // Insert into DoctorInfo
                        var doctorInfoQuery = @"INSERT INTO DoctorInfo (
                                            doctorName, phoneNumber, address
                                        ) VALUES (
                                            @DoctorName, @DoctorContactNo, @DoctorAddress
                                        );";
                        long doctorID;
                        using (var command = new MySqlCommand(doctorInfoQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@DoctorName", doctorName);
                            command.Parameters.AddWithValue("@DoctorContactNo", doctorContact);
                            command.Parameters.AddWithValue("@DoctorAddress", doctorAddress); // Modify as needed

                            await command.ExecuteNonQueryAsync();
                            doctorID = command.LastInsertedId;
                        }

                        // Now, associate the player with the guardian and doctor using their IDs
                        var playerDoctorAssociationQuery = @"INSERT INTO PlayerDoctor (sruNumber, doctorID) VALUES (@SruNumber, @DoctorID);";

                        // Associate Player with Doctor
                        using (var command = new MySqlCommand(playerDoctorAssociationQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@SruNumber", sruNumber);
                            command.Parameters.AddWithValue("@DoctorID", doctorID);
                            await command.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction after all inserts
                        transaction.Commit();
                    }
                }
                // Show success message
                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Profile Registered",
                    Content = "This profile has successfully been registered.",
                    CloseButtonText = "Ok"
                };

                await successDialog.ShowAsync();
                Frame.Navigate(typeof(PlayerDevCentrePro.PlayerDevCentrePro));
            }
            catch (MySqlException ex)
            {
                await ShowDialog("Database Error", "A database error occurred: " + ex.Message);
            }
            catch (Exception ex)
            {
                await ShowDialog("Error", "An unexpected error occurred: " + ex.Message);
            }
        }
        private async Task ShowDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK"
            };
            await dialog.ShowAsync();
        }
    }
}
