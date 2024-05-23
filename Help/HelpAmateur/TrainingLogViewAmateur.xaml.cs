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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SimplyRugby.Help.HelpAmateur
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrainingLogViewAmateur : Page
    {
        public TrainingLogViewAmateur()
        {
            this.InitializeComponent();
            MyVideo.Source = new Uri("ms-appx:///Assets/ViewTrainingLog.mkv"); // Replace "MyVideo.mp4" with the name of your video file
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MyVideo.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            MyVideo.Pause();
        }

        private void SkipBackwardButton_Click(object sender, RoutedEventArgs e)
        {
            MyVideo.Position -= TimeSpan.FromSeconds(10); // Skip backward by 10 seconds
        }

        private void SkipForwardButton_Click(object sender, RoutedEventArgs e)
        {
            MyVideo.Position += TimeSpan.FromSeconds(10); // Skip forward by 10 seconds
        }
    }
}
