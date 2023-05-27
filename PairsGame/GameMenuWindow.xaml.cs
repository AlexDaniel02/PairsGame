using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PairsGame
{
    public partial class GameMenuWindow : Window
    {
        public GameMenuWindow()
        {
            InitializeComponent();
            usernameLabel.Content = MainWindow.loggedUser.Username;
            profileImage.Source = new BitmapImage(new Uri(MainWindow.loggedUser.ProfilePicture));
        }
        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }
        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void statisticsButton_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow statisticsWindow = new StatisticsWindow();
            statisticsWindow.ShowDialog();
        }
    }
}
