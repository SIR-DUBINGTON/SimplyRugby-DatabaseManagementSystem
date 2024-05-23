using MySqlConnector;
using MySqlX.XDevAPI.Common;
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

namespace SimplyRugby.PlayerStatsAmateur
{
    /// <summary>
    /// Page for registering player stats for amateur coaches.
    /// </summary>
    public sealed partial class PlayerStatsAmateur : Page
    {
        public ObservableCollection<Player.Player> Players { get; } = new ObservableCollection<Player.Player>();
        public ObservableCollection<SkillEntry.SkillEntry> SkillEntries { get; } = new ObservableCollection<SkillEntry.SkillEntry>();
        public PlayerStatsAmateur()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;
            SkillsListView.ItemsSource = SkillEntries;

        }
        /// <summary>
        /// Method for adding a new skill entry to the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            SkillEntries.Add(new SkillEntry.SkillEntry());
        }
        /// <summary>
        /// Method for sending skill entry to database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int currentStaffId = SessionManager.SessionManager.StaffID;
            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";
            string queryString = @"
            SELECT sruNumber, playerName 
            FROM PlayerInfo 
            WHERE coachStaffID = @StaffID;";

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@StaffID", currentStaffId);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Players.Add(new Player.Player
                            {
                                SruNumber = reader.GetInt32("sruNumber"),
                                PlayerName = reader.GetString("playerName")
                            });
                        }
                        PlayerComboBox.ItemsSource = Players;
                    }
                }
            }
        }

        /// <summary>
        /// Method for submitting stats to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitStatsButton_Click(object sender, RoutedEventArgs e)
        {

            if (PlayerComboBox.SelectedValue == null || PositionComboBox.SelectedItem == null)
            {
                await ShowDialog("Error", "Please select a player and a position.");
                return;
            }

            int sruNumber = (int)PlayerComboBox.SelectedValue;
            string position = (PositionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Check if any of the skill entries are incomplete
            if (SkillEntries.Any(skillEntry => string.IsNullOrEmpty(skillEntry.SkillCategory) ||
                                               string.IsNullOrEmpty(skillEntry.Skill) ||
                                               skillEntry.SkillLevel == 0 ||
                                               string.IsNullOrEmpty(skillEntry.SkillComments)))
            {
                await ShowDialog("Error", "Please fill in all fields for all skill entries: Skill Category, Skill, Skill Level, and Comments.");
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=simplyrugby;";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    foreach (var skillEntry in SkillEntries)
                    {
                        var insertQuery = @"
                            INSERT INTO PlayerProfileSheet (sruNumber, skillCategory, skill, skillLevel, skillComments, position)
                            VALUES (@SruNumber, @SkillCategory, @Skill, @SkillLevel, @SkillComments, @Position);";
                        using (var command = new MySqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@SruNumber", sruNumber); // Ensure this is set from your context
                            command.Parameters.AddWithValue("@SkillCategory", skillEntry.SkillCategory);
                            command.Parameters.AddWithValue("@Skill", skillEntry.Skill);
                            command.Parameters.AddWithValue("@SkillLevel", skillEntry.SkillLevel);
                            command.Parameters.AddWithValue("@SkillComments", skillEntry.SkillComments);
                            command.Parameters.AddWithValue("@Position", position); // Ensure this is set

                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    {
                        ContentDialog successDialog = new ContentDialog
                        {
                            Title = "Stats Registered",
                            Content = "The stats have been successfully registered.",
                            CloseButtonText = "Ok"
                        };

                        await successDialog.ShowAsync();
                        SkillEntries.Clear(); // Clear the list for new input
                        Frame.Navigate(typeof(PlayerDevCentre.PlayerDevCentre));
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowDialog("Error", $"An error occurred: {ex.Message}");
            }
        }
        private async Task ShowDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "Ok"
            };
            await dialog.ShowAsync();
        }
    }
}
