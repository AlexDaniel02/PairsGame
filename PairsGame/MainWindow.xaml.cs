using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PairsGame
{
    public partial class MainWindow : Window
    {
        private List<User> _users;
        public static User loggedUser;
        public MainWindow()
        {
            InitializeComponent();
            _users = new List<User>();
            if (!File.Exists("users.csv"))
            {
                File.Create("users.csv");
            }
            using (StreamReader reader = new StreamReader("users.csv"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(',');
                    if (parts.Length == 4)
                    {
                        User user = new User();
                        user.Username = parts[0];
                        user.ProfilePicture = parts[1];
                        user.GamesPlayed = int.Parse(parts[2]);
                        user.GamesWon = int.Parse(parts[3]);
                        _users.Add(user);
                    }
                }
            }
            foreach (var user in _users)
            {
                accountsComboBox.Items.Add(user.Username);
            }
        }
        private void newUserButton_Click(object sender, RoutedEventArgs e)
        {
            NewUserWindow newUserWindow = new NewUserWindow();
            newUserWindow.ShowDialog();
            User newUser = newUserWindow.User;
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i].Username == newUser.Username)
                {
                    MessageBox.Show("This username already exists!");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(newUser.Username) && !string.IsNullOrEmpty(newUser.ProfilePicture))
            {
                accountsComboBox.Items.Add(newUser.Username);
                profileImage.Source = new BitmapImage(new Uri(newUser.ProfilePicture));
                _users.Add(newUser);
                accountsComboBox.SelectedItem = newUser.Username;
                using (StreamWriter writer = new StreamWriter("users.csv", true))
                {
                    writer.WriteLine($"{newUser.Username},{newUser.ProfilePicture},{newUser.GamesPlayed},{newUser.GamesWon}");
                }
            }
            else
            {
                MessageBox.Show("Please enter an username!");
            }
        }
        private void accountsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (accountsComboBox.SelectedItem != null)
            {
                string selectedUserName = accountsComboBox.SelectedItem.ToString();
                User selectedUser = _users.Find(user => user.Username == selectedUserName);

                if (selectedUser != null)
                {
                    profileImage.Source = new BitmapImage(new Uri(selectedUser.ProfilePicture));
                }
            }
        }
        private void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (accountsComboBox.SelectedItem != null)
            {
                string selectedUserName = accountsComboBox.SelectedItem.ToString();
                User selectedUser = _users.Find(user => user.Username == selectedUserName);

                if (selectedUser != null)
                {
                    _users.Remove(selectedUser);
                    accountsComboBox.Items.Remove(selectedUserName);
                    string[] lines = File.ReadAllLines("users.csv");
                    File.WriteAllLines("users.csv", lines.Where(line => !line.StartsWith(selectedUser.Username)));
                    profileImage.Source = null;
                    MessageBox.Show("User deleted!");
                }
            }
            else
            {
                MessageBox.Show("Please select an account to delete!");
            }
        }
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (accountsComboBox.SelectedItem != null)
            {
                string selectedUserName = accountsComboBox.SelectedItem.ToString();
                loggedUser = _users.Find(user => user.Username == selectedUserName);
                GameMenuWindow gameMenuWindow = new GameMenuWindow();
                gameMenuWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select an account to play!");
            }
        }
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {

            System.Environment.Exit(0);
        }
    }
}
