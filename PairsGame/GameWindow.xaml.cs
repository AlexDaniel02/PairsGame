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
using System.Windows.Shapes;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace PairsGame
{
    public partial class GameWindow : Window
    {
        private List<Button> _buttonsList;
        private List<ImageSource> _imagesList;
        private ImageSource _grayImage;
        private int _pairsFound;
        private bool _firstClick;
        private Button _firstButton;
        private int _size;
        private DispatcherTimer _timer;
        private Label _timerLabel;
        private int _timeRemaining;
        private DateTime _startTime;
        private Grid _boardGrid;
        private string[] _imagePaths;
        
        public GameWindow(int size)
        {
            _timer = new DispatcherTimer();
            _firstClick = false;
            _pairsFound = 0;
            this._size = size;
            _imagesList = new List<ImageSource>();
            _buttonsList = new List<Button>();
            InitializeComponent();
            TimerSetup();
            BoardSetup();
            StartTimer();
            ImagesSetup();
            ButtonsSetup();
            Content = new Grid() { Children = { _timerLabel, _boardGrid } };
        }
        private void ButtonsSetup()
        {
            for (int row = 0; row < Math.Sqrt(_size); row++)
            {
                for (int col = 0; col < Math.Sqrt(_size); col++)
                {
                    Button button = new Button();
                    button.Name = $"button_{row}_{col}";
                    button.Content = new Image() { Source = new BitmapImage(new Uri(_imagePaths [0])) };
                    button.Click += Button_Click;
                    button.Width = 70;
                    button.Height = 70;
                    Console.WriteLine($"Adding button {button.Name} to grid");
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    _buttonsList.Add(button);
                    _boardGrid.Children.Add(button);
                }
            }
        }
        private void BoardSetup()
        {
            _boardGrid = new Grid();
            _boardGrid.HorizontalAlignment = HorizontalAlignment.Center;
            _boardGrid.VerticalAlignment = VerticalAlignment.Center;
            _boardGrid.ShowGridLines = true;
            for (int i = 0; i < Math.Sqrt(_size); i++)
            {
                _boardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                _boardGrid.RowDefinitions.Add(new RowDefinition());
            }
        }
        private void StartTimer()
        {
            _timeRemaining = 90;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _startTime = DateTime.Now;
            _timer.Start();
        }
        private void ImagesSetup()
        {
            string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CardsImages");
            _imagePaths = Directory.GetFiles(imagesFolder, "*.jpg");
            _grayImage = new BitmapImage(new Uri(_imagePaths[0]));
            for (int i = 0; i < _size / 2; i++)
            {
                _imagesList.Add(new BitmapImage(new Uri(_imagePaths[i + 1])));
                _imagesList.Add(new BitmapImage(new Uri(_imagePaths[i + 1])));
            }
            ShuffleImages();
        }
        private void TimerSetup()
        {
            _timerLabel = new Label();
            _timerLabel.FontSize = 24;
            _timerLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _timerLabel.VerticalAlignment = VerticalAlignment.Top;
            _timerLabel.Margin = new Thickness(0, 20, 0, 0);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeRemaining--;
            var elapsedTime = DateTime.Now - _startTime;
            _timerLabel.Content = $"Time: {_timeRemaining} seconds";
            if (_timeRemaining == 0)
            {
                _timer.Stop();
                MessageBox.Show("Time's up! You lose.");
                MainWindow.loggedUser.GamesPlayed++;
                UpdateStatistics();
                this.Close();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int index = _buttonsList.IndexOf(clickedButton);
            Image image = (Image)clickedButton.Content;
            image.Source = _imagesList[index];
            if (_firstClick == false)
            {
                clickedButton.IsEnabled = false;
                _firstClick = true;
                _firstButton = clickedButton;
            }
            else
            {
                _firstClick = false;
                if (((Image)_firstButton.Content).Source.ToString() == ((Image)clickedButton.Content).Source.ToString())
                {
                    _pairsFound++;
                    clickedButton.IsEnabled = false;
                    _firstButton.IsEnabled = false;
                    if (_pairsFound == _size / 2)
                    {
                        _timer.Stop();
                        MessageBox.Show("Congratulations, you won!");
                        MainWindow.loggedUser.GamesPlayed++;
                        MainWindow.loggedUser.GamesWon++;
                        UpdateStatistics();
                        this.Close();
                    }
                }
                else
                {
                    DispatcherTimer delay = new DispatcherTimer();
                    delay.Interval = TimeSpan.FromSeconds(0.5);
                    delay.Tick += (s, args) =>
                    {
                        clickedButton.IsEnabled = true;
                        _firstButton.IsEnabled = true;
                        image.Source = _grayImage;
                        ((Image)_firstButton.Content).Source = _grayImage;
                        delay.Stop();
                    };
                    delay.Start();
                }
            }
        }
        private void UpdateStatistics()
        {
            string[] lines = File.ReadAllLines("users.csv");
            File.WriteAllLines("users.csv", lines.Where(line => !line.StartsWith(MainWindow.loggedUser.Username)));
            using (StreamWriter writer = new StreamWriter("users.csv", true))
            {
                writer.WriteLine($"{MainWindow.loggedUser.Username},{MainWindow.loggedUser.ProfilePicture},{MainWindow.loggedUser.GamesPlayed},{MainWindow.loggedUser.GamesWon}");
            }
        }
        private void ShuffleImages()
        {
            Random random = new Random();
            _imagesList = _imagesList.OrderBy(x => random.Next()).ToList();
        }
    }
}
