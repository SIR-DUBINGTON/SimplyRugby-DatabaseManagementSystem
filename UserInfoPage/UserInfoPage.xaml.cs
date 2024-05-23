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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimplyRugby.UserInfoPage
{
    /// <summary>
    /// Page for updating user information
    /// </summary>
    public sealed partial class UserInfoPage : Page
    {
        public UserInfoPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Method send user info to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve session or passed data for the username
            var username = SessionManager.SessionManager.Username; // Make sure to implement SessionManager to store session data
            var staffRole = SessionManager.SessionManager.UserRole; // Make sure to implement SessionManager to store session data

            var staffName = StaffNameTextBox.Text;
            var email = EmailTextBox.Text;
            var address = AddressTextBox.Text;
            var dob = DobDatePicker.Date.DateTime;
            var contactNo = ContactNumberTextBox.Text;
            var postcode = PostcodeTextBox.Text;

            if (string.IsNullOrEmpty(staffName) || string.IsNullOrEmpty(email)|| string.IsNullOrEmpty(address) || string.IsNullOrEmpty(contactNo) || string.IsNullOrEmpty(postcode))
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Missing Information",
                    Content = "Please fill in all fields before submitting.",
                    CloseButtonText = "Ok"
                };

                await errorDialog.ShowAsync();
                return;
            }


            if(!email.Contains("@") || !email.Contains("."))
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Invalid Email",
                    Content = "Please enter a valid email address.",
                    CloseButtonText = "Ok"
                };

                await errorDialog.ShowAsync();
                return;
            }

            // Validate age to be over 18
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

            if (contactNo.Length != 11)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Invalid Contact Number",
                    Content = "Please enter a valid contact number, 11 characters.",
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
                    var query = @"UPDATE Staff SET 
                                    staffName=@StaffName,
                                    email=@Email,
                                    Address=@Address, 
                                    dob=@Dob, 
                                    contactNo=@ContactNo, 
                                    postcode=@Postcode
                                  WHERE username=@Username;";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StaffName", staffName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Dob", dob);
                        command.Parameters.AddWithValue("@ContactNo", contactNo);
                        command.Parameters.AddWithValue("@Postcode", postcode);
                        command.Parameters.AddWithValue("@Username", username);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                // Show success message
                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Profile Updated",
                    Content = "Your profile information has been successfully updated.",
                    CloseButtonText = "Ok"
                };

                await successDialog.ShowAsync();

                // Optionally, navigate to another page here if needed
                NavigateBasedOnRole(staffRole);

            }
            catch (Exception ex)
            {
                // Handle exceptions
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Update Failed",
                    Content = "There was an error updating your profile. Please try again.",
                    CloseButtonText = "Ok"
                };

                await errorDialog.ShowAsync();
            }
        }
        /// <summary>
        /// method to navigate to the correct hub page based on the staff role
        /// </summary>
        /// <param name="staffRole"></param>
        private void NavigateBasedOnRole(string staffRole)
        {
            switch (staffRole)
            {
                case "Amateur Coach":
                    Frame.Navigate(typeof(AmateurCoachHub.AmateurCoachHub));
                    break;
                case "Pro Coach":
                    Frame.Navigate(typeof(ProCoachHub.ProCoachHub));
                    break;
                case "Admin Staff":
                    Frame.Navigate(typeof(AdminStaffHub.AdminStaffHub));
                    break;
                default:
                    // Unknown role or error
                    break;
            }
        }
    }
}
