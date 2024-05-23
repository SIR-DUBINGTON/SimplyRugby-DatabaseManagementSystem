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

namespace SimplyRugby.PlayerDevCentreAdmin
{
    /// <summary>
    /// Page for the Admin to view teams, create teams and view staff
    /// </summary>
    public sealed partial class PlayerDevCentreAdmin : Page
    {
        public PlayerDevCentreAdmin()
        {
            this.InitializeComponent();
            this.ViewTeamsCommand = new RelayCommand(_ => ViewTeams());
            this.CreateTeamsCommand = new RelayCommand(_ => CreateTeams());
            this.ViewStaffCommand = new RelayCommand(_ => ViewStaff());

        }
        /// <summary>
        /// Method to navigate to the View Teams page
        /// </summary>
        public ICommand ViewTeamsCommand { get; }
        /// <summary>
        /// Method to navigate to the Create Teams page
        /// </summary>
        public ICommand CreateTeamsCommand { get; }
        /// <summary>
        /// Method to navigate to the View Staff page
        /// </summary>
        public ICommand ViewStaffCommand { get; }

        /// <summary>
        /// Method to navigate to the View Teams page
        /// </summary>
        private void ViewTeams()
        {
            // Implementation to navigate to the View Teams page or logic
            Frame.Navigate(typeof(ViewTeamsAdmin.ViewTeamsAdmin));
        }
        /// <summary>
        /// Method to navigate to the Create Teams page
        /// </summary>
        private void CreateTeams()
        {
            // Implementation to navigate to the Create Teams page or logic
            Frame.Navigate(typeof(CreateTeams.CreateTeams));
        }
        /// <summary>
        /// Method to navigate to the View Staff page
        /// </summary>
        private void ViewStaff()
        {
            // Implementation to navigate to the View Staff page or logic
            Frame.Navigate(typeof(ViewStaff.ViewStaff));
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
}
