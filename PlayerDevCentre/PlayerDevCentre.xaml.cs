using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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

namespace SimplyRugby.PlayerDevCentre
{
    /// <summary>
    /// Page for the Player Development Centre
    /// </summary>
    public sealed partial class PlayerDevCentre : Page
    {
        public PlayerDevCentre()
        {
            this.InitializeComponent();
            this.ViewTeamsCommand = new RelayCommand(_ => ViewTeams());
            this.RegisterPlayerCommand = new RelayCommand(_ => RegisterNewPlayer());
            this.ViewMyPlayersCommand = new RelayCommand(_ => ViewMyPlayers());
            this.RegisterMyPlayerStatsCommand = new RelayCommand(_ => RegisterMyPlayerStats());
            this.ViewMyPlayerStatsCommand = new RelayCommand(_ => ViewMyPlayerStats());
            this.RegisterJuniorPlayerCommand = new RelayCommand(_ => RegisterJuniorPlayer());
            this.ViewJuniorPlayersCommand = new RelayCommand(_ => ViewJuniorPlayers());
        }
        /// <summary>
        /// Method to navigate to the View Teams page
        /// </summary>
        public ICommand ViewTeamsCommand { get; }
        /// <summary>
        /// Method to navigate to the Registration page
        /// </summary>
        public ICommand RegisterPlayerCommand { get; }
        /// <summary>
        /// Method to navigate to the Player Profiles page
        /// </summary>
        public ICommand ViewMyPlayersCommand { get; }
        /// <summary>
        /// Method to navigate to the Player Stats page
        /// </summary>
        public ICommand RegisterMyPlayerStatsCommand { get; }
        /// <summary>
        /// Method to navigate to the View Stats page
        /// </summary>
        public ICommand ViewMyPlayerStatsCommand { get; }
        /// <summary>
        /// Method to navigate to the Junior Player Registration page
        /// </summary>
        public ICommand RegisterJuniorPlayerCommand { get; }
        /// <summary>
        /// Method to navigate to the View Junior Players page
        /// </summary>
        public ICommand ViewJuniorPlayersCommand { get; }

        /// <summary>
        /// Method to navigate to the View Teams page
        /// </summary>
        private void ViewTeams()
        {
            // Implementation to navigate to the View Teams page or logic
            Frame.Navigate(typeof(ViewTeamsAmateur.ViewTeamsAmateur));
        }
        /// <summary>
        /// Method to navigate to the Registration page
        /// </summary>
        private void RegisterNewPlayer()
        {
            // Implementation to navigate to the Registration page or logic
            Frame.Navigate(typeof(PlayerRegisterAmateurCoach.PlayerRegisterAmateurCoach));
        }

        /// <summary>
        /// Method to navigate to the Player Profiles page
        /// </summary>
        private void ViewMyPlayers()
        {
            // Implementation to navigate to the Player Profiles page or logic
            Frame.Navigate(typeof(ViewPlayersAmateur.ViewPlayersAmateur));
        }
        /// <summary>
        /// Method to navigate to the Player Stats page
        /// </summary>
        private void RegisterMyPlayerStats()
        {
            // Implementation to navigate to the Player Stats page or logic
            Frame.Navigate(typeof(PlayerStatsAmateur.PlayerStatsAmateur));
        }
        /// <summary>
        /// Method to navigate to the View Stats page
        /// </summary>
        private void ViewMyPlayerStats()
        {
            // Implementation to navigate to the View Stats page or logic
            Frame.Navigate(typeof(ViewStatsAmateur.ViewStatsAmateur));
        }
        /// <summary>
        /// Method to navigate to the Junior Player Registration page
        /// </summary>
        private void RegisterJuniorPlayer()
        {
            // Implementation to navigate to the Junior Player Registration page or logic
            Frame.Navigate(typeof(JuniorPlayerRegisterAmateurCoach.JuniorPlayerRegisterAmateurCoach));
        }
        /// <summary>
        /// Method to navigate to the View Junior Players page
        /// </summary>
        private void ViewJuniorPlayers()
        {
            // Implementation to navigate to the View Junior Players page or logic
            Frame.Navigate(typeof(ViewJuniorPlayersAmateur.ViewJuniorPlayersAmateur));
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> execute;

        public RelayCommand(Action<object> execute)
        {
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}

