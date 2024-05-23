using MySqlConnector;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimplyRugby.CreatePage
{
    /// <summary>
    /// A page that allows the user to create a new account and password
    /// </summary>
    public sealed partial class CreatePage : Page
    {
        // Constructor
        public CreatePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// A method that navigates to the UserInfo page after the account has been created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text; // Get the username from the text box
            var password = PasswordBox.Password; // Get the password from the password box
            var staffRole = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(); // Get the role from the combo box

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(staffRole))
            {

                // Provide user feedback about the need to fill all fields
                await ShowFailureDialog();
                return;
            }
            // Check if the password length is less than 8 characters
            if (password.Length < 8)
            {
                ContentDialog failureDialog = new ContentDialog
                {
                    Title = "Account Not Created",
                    Content = "Password must be at least 8 characters long.",
                    CloseButtonText = "Ok"
                };

                await failureDialog.ShowAsync();
                return;
            }

            var hashedPassword = HashPassword(password);  // Hash the password
            var connectionString = "server=localhost;user=root;password=;database=simplyrugby;"; // Connection string to the database

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO Staff (username, passwordHash, StaffRole) VALUES (@Username, @Password, @StaffRole);";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.Parameters.AddWithValue("@StaffRole", staffRole);

                        await command.ExecuteNonQueryAsync();

                        SessionManager.SessionManager.Username = username;
                        SessionManager.SessionManager.UserRole = staffRole;
                        long lastId = command.LastInsertedId;
                        SessionManager.SessionManager.StaffID = (int)lastId;
                        // Show success message
                        await ShowSuccessDialog();
                        // Navigate to the user info page
                        Frame.Navigate(typeof(UserInfoPage.UserInfoPage));
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception and provide user feedback
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
        /// A method that shows a dialog when the account creation is successful
        /// </summary>
        /// <returns></returns>
        private async Task ShowSuccessDialog()
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "Account Created",
                Content = "The account has been successfully created, please complete your profile",
                CloseButtonText = "Ok"
            };

            await successDialog.ShowAsync();
        }

        /// <summary>
        /// A method that shows a dialog when the account creation fails
        /// </summary>
        /// <returns></returns>
        private async Task ShowFailureDialog()
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "Account Not Created",
                Content = "Please enter a valid username, password and choose a role.",
                CloseButtonText = "Ok"
            };

            await successDialog.ShowAsync();
        }

        /// <summary>
        /// A method that navigates back to the previous page
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

        /// <summary>
        /// A method that hashes the password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string HashPassword(string password)
        {
            using (var deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(password, saltSize: 8, iterations: 10000))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] buffer2 = deriveBytes.GetBytes(20);  // 20 bytes for SHA-1

                byte[] dst = new byte[salt.Length + buffer2.Length];
                Buffer.BlockCopy(salt, 0, dst, 0, salt.Length);
                Buffer.BlockCopy(buffer2, 0, dst, salt.Length, buffer2.Length);
                return Convert.ToBase64String(dst);
            }
        }

    }
}
