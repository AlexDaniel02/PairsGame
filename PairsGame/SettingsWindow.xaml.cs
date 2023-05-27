using System;
using System.Collections.Generic;
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

namespace PairsGame
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (fourByFourRadioButton.IsChecked == true)
            {
                GameWindow gameWindow = new GameWindow(16);
                this.Close();
                gameWindow.ShowDialog();
            }
            else if (sixBySixRadioButton.IsChecked == true)
            {
                GameWindow gameWindow = new GameWindow(36);
                this.Close();
                gameWindow.ShowDialog();
            }
            else if (eightByEightRadioButton.IsChecked == true)
            {
                GameWindow gameWindow = new GameWindow(64);
                this.Close();
                gameWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a board size!");
            }
            this.Close();
        }
    }
}
