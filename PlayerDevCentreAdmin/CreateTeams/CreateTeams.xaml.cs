using MySqlConnector;
using Org.BouncyCastle.Crypto.Macs;
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

namespace SimplyRugby.PlayerDevCentreAdmin.CreateTeams
{
    /// <summary>
    /// Page for creating a new team
    /// </summary>
    public sealed partial class CreateTeams : Page
    {
        public ObservableCollection<Coach.Coach> Coaches { get; set; } = new ObservableCollection<Coach.Coach>();
        public CreateTeams()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
        }
        /// <summary>
        /// Method to load the coaches into the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await LoadCoachesAsync();
            CoachComboBox.ItemsSource = Coaches;
        }
        /// <summary>
        /// Method to load the coaches from the database
        /// </summary>
        /// <returns></returns>
        private async Task LoadCoachesAsync()
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT staffID, staffName FROM Staff WHERE StaffRole IN ('Amateur Coach', 'Pro Coach');";

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int staffId = reader.GetInt32("staffID");
                            string staffName = reader.GetString("staffName");

                            // Assuming you have a collection to add to, representing the UI element (e.g., a ComboBox)
                            Coaches.Add(new Coach.Coach
                            {
                                ID = staffId,
                                Name = staffName
                            });
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Method to create a new team
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateTeam_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string teamName = TeamNameTextBox.Text;
            var ageGroupItem = AgeGroupComboBox.SelectedItem as ComboBoxItem;
            string ageGroup = ageGroupItem != null ? ageGroupItem.Content.ToString() : null;
            var coachIdObject = CoachComboBox.SelectedValue;

            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(teamName) || ageGroup == null || coachIdObject == null)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Please fill in all fields: Team Name, Age Group, and select a Coach.",
                    CloseButtonText = "Ok"
                };

                await errorDialog.ShowAsync();
                return;
            }

            int coachId = (int)coachIdObject;
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "INSERT INTO TeamInfo (teamName, ageGroup, coachStaffID) VALUES (@teamName, @ageGroup, @coachStaffID);";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@teamName", teamName);
                        command.Parameters.AddWithValue("@ageGroup", ageGroup);
                        command.Parameters.AddWithValue("@coachStaffID", coachId);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                // Show success message
                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Team Registered",
                    Content = "This team has successfully been registered.",
                    CloseButtonText = "Ok"
                };

                await successDialog.ShowAsync();
                Frame.Navigate(typeof(PlayerDevCentreAdmin)); // Adjust navigation as necessary
            }
            catch (MySqlException ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Database Error",
                    Content = $"Failed to register the team: {ex.Message}",
                    CloseButtonText = "Ok"
                };

                await errorDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = $"An unexpected error occurred: {ex.Message}",
                    CloseButtonText = "Ok"
                };

                await errorDialog.ShowAsync();
            }
        }

    }

}

