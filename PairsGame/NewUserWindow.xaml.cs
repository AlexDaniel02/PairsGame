using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

namespace PairsGame
{
    public partial class NewUserWindow : Window
    {
        public User User { get; set; }
        private string[] _imagePaths;
        private int _currentPhoto;   
        public NewUserWindow()
        {
            User = new User();
            InitializeComponent();
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            _imagePaths = Directory.GetFiles(imagesFolder, "*.jpg");
            profilePicture.Source = new BitmapImage(new Uri(_imagePaths[0]));
            User.ProfilePicture = profilePicture.Source.ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User.Username = usernameTextBox.Text;
            Close();
        }
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            Image selectedImage = (Image)sender;
            if (selectedImage != null)
            {
                User.ProfilePicture = selectedImage.Source.ToString();
            }
        }
        private void leftArrow_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPhoto > 0)
            {
                _currentPhoto--;
                profilePicture.Source = new BitmapImage(new Uri(_imagePaths[_currentPhoto]));
            }
            else
            {
                _currentPhoto = _imagePaths.Length - 1;
                profilePicture.Source = new BitmapImage(new Uri(_imagePaths[_currentPhoto]));
            }
        }
        private void rightArrow_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPhoto < _imagePaths.Length - 1)
            {
                _currentPhoto++;
                profilePicture.Source = new BitmapImage(new Uri(_imagePaths[_currentPhoto]));
                User.ProfilePicture = profilePicture.Source.ToString();
            }
            else
            {
                _currentPhoto = 0;
                profilePicture.Source = new BitmapImage(new Uri(_imagePaths[_currentPhoto]));
                User.ProfilePicture = profilePicture.Source.ToString();
            }
        }
    }
}
