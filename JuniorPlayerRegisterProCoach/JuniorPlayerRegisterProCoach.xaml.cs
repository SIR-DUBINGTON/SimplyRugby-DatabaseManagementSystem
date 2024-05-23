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

namespace SimplyRugby.JuniorPlayerRegisterProCoach
{
    /// <summary>
    /// Page for registering a junior player by a professional coach.
    /// </summary>
    public sealed partial class JuniorPlayerRegisterProCoach : Page
    {
        public ObservableCollection<Player.Player> Player { get; set; } = new ObservableCollection<Player.Player>();
        public ObservableCollection<Guardian.Guardian> GuardianInfos { get; set; } = new ObservableCollection<Guardian.Guardian>();

        public ObservableCollection<Team.Team> Teams { get; set; } = new ObservableCollection<Team.Team>();
        public JuniorPlayerRegisterProCoach()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;

        }
        /// <summary>
        /// Asynchronously loads the teams for the current coach.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTeamsAsync();
        }
        /// <summary>
        /// A method to navigate to the next page in the flip view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegistrationFlipView.SelectedIndex < RegistrationFlipView.Items.Count - 1)
            {
                RegistrationFlipView.SelectedIndex += 1;
            }
        }
        /// <summary>
        /// A method to navigate to the previous page in the flip view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegistrationFlipView.SelectedIndex > 0)
            {
                RegistrationFlipView.SelectedIndex -= 1;
            }
        }

        /// <summary>
        /// A method to load the teams for the current coach.
        /// </summary>
        /// <returns></returns>
        private async Task LoadTeamsAsync()
        {
            int currentCoachId = SessionManager.SessionManager.StaffID; // Retrieve current coach's staff ID
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";

            // Adjust the query to include the filter for 'Junior' teams and the current coach
            string query = "SELECT teamID, teamName FROM TeamInfo WHERE coachStaffID = @CoachStaffID AND ageGroup = 'Junior';";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CoachStaffID", currentCoachId); // Use the current coach's ID to filter teams

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
                        TeamComboBox.DisplayMemberPath = "TeamName";
                        TeamComboBox.SelectedValuePath = "TeamID";
                        if (Teams.Count > 0)
                            TeamComboBox.SelectedIndex = 0; // Optionally select the first team by default
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load teams: " + ex.ToString()); // Log the exception
            }
        }
        /// <summary>
        /// A method to register a player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RegisterPlayer_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve form data
            string playerName = NameTextBox.Text;
            string email = EmailTextBox.Text;
            string address = AddressTextBox.Text;
            var dob = DobDatePicker.Date.DateTime;
            string contactNo = ContactNumberTextBox.Text;
            string postcode = PostcodeTextBox.Text;
            string healthConditions = HealthConditionsTextBox.Text;
            string guardianName = GuardianTextBox.Text;
            string guardianContactNo = GuardianContactNoTextBox.Text;
            string guardianRelationship = GuardianRelationshipTextBox.Text;
            string guardianAddress = GuardianAddressTextBox.Text;
            int? teamId = TeamComboBox.SelectedValue as int?;
            int coachStaffID = SessionManager.SessionManager.StaffID; // Retrieve coach's staff ID


            if (string.IsNullOrWhiteSpace(playerName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(contactNo) || string.IsNullOrWhiteSpace(postcode) || string.IsNullOrWhiteSpace(healthConditions) || string.IsNullOrWhiteSpace(guardianName) || string.IsNullOrWhiteSpace(guardianContactNo) || string.IsNullOrWhiteSpace(guardianRelationship) || string.IsNullOrWhiteSpace(guardianAddress) || !teamId.HasValue)
            {
                var emptyFieldsDialog = new ContentDialog
                {
                    Title = "Registration Failed",
                    Content = "Please fill in all fields.",
                    CloseButtonText = "OK"
                };
                await emptyFieldsDialog.ShowAsync();
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
            if ((DateTime.Now - dob).TotalDays / 365.25 >= 18)
            {
                var ageFailureDialog = new ContentDialog
                {
                    Title = "Registration Failed",
                    Content = "Player must be under 18 years of age.",
                    CloseButtonText = "OK"
                };
                await ageFailureDialog.ShowAsync();
                return;
            }

            var connectionString = "server=localhost;user=root;password=;database=simplyrugby;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        // Insert into PlayerInfo
                        string playerInfoQuery = @"
                    INSERT INTO PlayerInfo (
                        playerName, email, address, dob, contactNo, postcode, knownHealthIssues,  teamID, coachStaffID
                    ) VALUES (
                        @PlayerName, @Email, @Address, @Dob, @ContactNo, @Postcode, @HealthConditions,  @TeamID, @CoachStaffID
                    );";

                        long sruNumber;
                        using (var playerInfoCommand = new MySqlCommand(playerInfoQuery, connection, transaction))
                        {
                            // Add parameters for PlayerInfo
                            playerInfoCommand.Parameters.AddWithValue("@PlayerName", playerName);
                            playerInfoCommand.Parameters.AddWithValue("@Email", email);
                            playerInfoCommand.Parameters.AddWithValue("@Address", address);
                            playerInfoCommand.Parameters.AddWithValue("@Dob", dob);
                            playerInfoCommand.Parameters.AddWithValue("@ContactNo", contactNo);
                            playerInfoCommand.Parameters.AddWithValue("@Postcode", postcode);
                            playerInfoCommand.Parameters.AddWithValue("@HealthConditions", healthConditions);
                            playerInfoCommand.Parameters.AddWithValue("@TeamID", teamId.HasValue ? (object)teamId.Value : DBNull.Value);
                            playerInfoCommand.Parameters.AddWithValue("@CoachStaffID", coachStaffID);

                            await playerInfoCommand.ExecuteNonQueryAsync();
                            sruNumber = playerInfoCommand.LastInsertedId;
                        }

                        // Insert into GuardianInfo
                        string guardianInfoQuery = @"INSERT INTO GuardianInfo (guardianName, relationship, address, phoneNumber) VALUES (@GuardianName, @Relationship, @Address, @PhoneNumber);";

                        long guardianID;
                        using (var guardianInfoCommand = new MySqlCommand(guardianInfoQuery, connection, transaction))
                        {
                            // Add parameters for GuardianInfo
                            guardianInfoCommand.Parameters.AddWithValue("@GuardianName", guardianName);
                            guardianInfoCommand.Parameters.AddWithValue("@Relationship", guardianRelationship);
                            guardianInfoCommand.Parameters.AddWithValue("@Address", guardianAddress);
                            guardianInfoCommand.Parameters.AddWithValue("@PhoneNumber", guardianContactNo);
                            await guardianInfoCommand.ExecuteNonQueryAsync();
                            guardianID = guardianInfoCommand.LastInsertedId;
                        }

                        // Link player with guardian in PlayerGuardian
                        string playerGuardianQuery = @"
                        INSERT INTO PlayerGuardian (sruNumber, guardianID) VALUES (@SruNumber, @GuardianID);";
                        using (var playerGuardianCommand = new MySqlCommand(playerGuardianQuery, connection, transaction))
                        {
                            playerGuardianCommand.Parameters.AddWithValue("@SruNumber", sruNumber);
                            playerGuardianCommand.Parameters.AddWithValue("@GuardianID", guardianID);
                            await playerGuardianCommand.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                }

                // Show success message
                var successDialog = new ContentDialog
                {
                    Title = "Registration Successful",
                    Content = "The player has been registered successfully.",
                    CloseButtonText = "OK"
                };

                await successDialog.ShowAsync();

                // Optionally, clear the form or navigate away
                Frame.Navigate(typeof(PlayerDevCentre.PlayerDevCentre));

            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("SQL Error: " + ex.Message);
                var sqlErrorDialog = new ContentDialog
                {
                    Title = "Database Error",
                    Content = "A database error occurred. Please try again.",
                    CloseButtonText = "OK"
                };

                // You could log the error or perform additional actions based on specific error numbers
                if (ex.Number == 1045) // Access denied
                {
                    sqlErrorDialog.Content = "Access to the database is denied. Please check your credentials.";
                }
                else if (ex.Number == 1062) // Duplicate entry
                {
                    sqlErrorDialog.Content = "This data already exists in the database. Please check your input for duplicates.";
                }

                await sqlErrorDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Non-SQL Error: " + ex.Message);
                var generalErrorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "An error occurred. Please try again later.",
                    CloseButtonText = "OK"
                };
                await generalErrorDialog.ShowAsync();
            }
        }

    }
}
