using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SimplyRugby.LoginPage
{
    /// <summary>
    /// Page for logging in to the application.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Event handler for the login button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                var emptyFieldsDialog = new ContentDialog
                {
                    Title = "Empty Fields",
                    Content = "Please enter a username and password.",
                    CloseButtonText = "OK"
                };
                await emptyFieldsDialog.ShowAsync();
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var query = "SELECT StaffID, StaffRole, passwordHash FROM Staff WHERE username=@Username;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        var reader = await command.ExecuteReaderAsync();
                        if (reader.Read())
                        {
                            string storedHash = reader.GetString("passwordHash");
                            if (VerifyPasswordHash(password, storedHash))
                            {
                                int staffID = reader.GetInt32("StaffID");
                                string staffRole = reader.GetString("StaffRole");

                                SessionManager.SessionManager.Username = username;
                                SessionManager.SessionManager.UserRole = staffRole;
                                SessionManager.SessionManager.StaffID = staffID;

                                NavigateBasedOnRole(staffRole);
                            }
                            else
                            {
                                await ShowFailureDialog();
                            }
                        }
                        else
                        {
                            await ShowFailureDialog();
                        }
                    }
                }
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
        /// <summary>
        /// A method to verify the password hash.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[8];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, 8);
            byte[] storedSubkey = new byte[20];
            Buffer.BlockCopy(hashBytes, 8, storedSubkey, 0, 20);

            using (var deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] testSubkey = deriveBytes.GetBytes(20);  // Must be same size as the hash part of the stored password
                return testSubkey.SequenceEqual(storedSubkey);
            }
        }

        /// <summary>
        /// A method to navigate to the appropriate hub page based on the staff role.
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

        /// <summary>
        /// A method to show a failure dialog.
        /// </summary>
        /// <returns></returns>
        private async Task ShowFailureDialog()
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "Login Failed",
                Content = "The account info entered is incorrect.",
                CloseButtonText = "Ok"
            };

            await successDialog.ShowAsync();
        }
        /// <summary>
        /// A method to navigate back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

    }
}
