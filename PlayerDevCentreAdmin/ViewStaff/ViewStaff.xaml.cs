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

namespace SimplyRugby.PlayerDevCentreAdmin.ViewStaff
{
    /// <summary>
    /// Page to view staff members
    /// </summary>
    public sealed partial class ViewStaff : Page
    {
        public ViewStaff()
        {
            this.InitializeComponent();
            Loaded += Page_Loaded;
        }
        /// <summary>
        /// Load method to load staff details from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadStaffDetailsAsync();
        }
        /// <summary>
        /// Method to load staff details from the database
        /// </summary>
        /// <returns></returns>
        private async Task LoadStaffDetailsAsync()
        {
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string query = "SELECT * FROM Staff ORDER BY StaffName;";

            ObservableCollection<StaffMember.StaffMember> staffMembers = new ObservableCollection<StaffMember.StaffMember>();

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            staffMembers.Add(new StaffMember.StaffMember
                            {
                                StaffID = reader.GetInt32("staffID"),
                                StaffName = reader.GetString("staffName"),
                                StaffRole = reader.IsDBNull(reader.GetOrdinal("StaffRole")) ? null : reader.GetString("StaffRole"),
                                Email = reader.GetString("email"),
                                ContactNo = reader.GetString("contactNo"),
                                DOB = reader.GetDateTime("dob"),
                                Address = reader.GetString("Address"),
                                Postcode = reader.GetString("postcode"),
                                username = reader.GetString("username"),
                                // Populate other fields as necessary
                            });
                        }
                    }
                }
            }

            StaffListView.ItemsSource = staffMembers;
        }
        /// <summary>
        /// Method to navigate to the add staff page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteStaffMember_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int staffId = Convert.ToInt32(button.CommandParameter);

                // Ask for confirmation before deleting
                var confirmDialog = new ContentDialog
                {
                    Title = "Confirm Deletion",
                    Content = "Are you sure you want to delete this staff member?",
                    PrimaryButtonText = "Delete",
                    CloseButtonText = "Cancel"
                };

                var result = await confirmDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
                    string queryDeleteStaff = "DELETE FROM Staff WHERE staffID=@StaffID;";
                    string queryFindTeams = "SELECT teamID FROM TeamInfo WHERE coachStaffID=@StaffID;";
                    string queryUpdateTeams = "UPDATE TeamInfo SET coachStaffID=NULL WHERE coachStaffID=@StaffID;";

                    try
                    {
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            await connection.OpenAsync();

                            // Find teams coached by the staff member
                            List<int> teamIds = new List<int>();
                            using (var command = new MySqlCommand(queryFindTeams, connection))
                            {
                                command.Parameters.AddWithValue("@StaffID", staffId);
                                using (var reader = await command.ExecuteReaderAsync())
                                {
                                    while (await reader.ReadAsync())
                                    {
                                        teamIds.Add(reader.GetInt32("teamID"));
                                    }
                                }
                            }

                            // Update teams to set their coachStaffID to NULL
                            using (var command = new MySqlCommand(queryUpdateTeams, connection))
                            {
                                command.Parameters.AddWithValue("@StaffID", staffId);
                                await command.ExecuteNonQueryAsync();
                            }

                            // Delete the staff member
                            using (var command = new MySqlCommand(queryDeleteStaff, connection))
                            {
                                command.Parameters.AddWithValue("@StaffID", staffId);
                                int rowsAffected = await command.ExecuteNonQueryAsync();

                                if (rowsAffected > 0)
                                {
                                    // Refresh the list to reflect the deletion
                                    await LoadStaffDetailsAsync();

                                    // Show success message
                                    await ShowDialog("Successful", "The staff member has been successfully deleted.");
                                }
                                else
                                {
                                    await ShowDialog("Error", "No staff member was found with the specified ID.");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        await ShowDialog("Error deleting staff member", $"An error occurred: {ex.Message}");
                    }
                }
            }
        }

        private async Task ShowDialog(string title, string content)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "Ok"
            };

            await errorDialog.ShowAsync();
        }
    }
}