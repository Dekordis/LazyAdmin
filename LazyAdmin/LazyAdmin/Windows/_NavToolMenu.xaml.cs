using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using LazyAdmin.DataBase;

namespace LazyAdmin.Windows
{
    /// <summary>
    /// Interaction logic for _NavToolMenu.xaml
    /// </summary>
    public partial class _NavToolMenu : Window
    {
        int i = 0;
        public _NavToolMenu()
        {
            InitializeComponent();
            App.WindowSettings(_HeaderButtonGrid, this);
            App.Load();
        }
        private void Upload(object sender, RoutedEventArgs e)
        {
            App.Upload(_DataGridFromAMT);
        }
        private void EnterText(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (_EnterText.Text == "" || _EnterText.Text == null || _EnterText.Text == " ") _EnterText.Clear();
                else
                {
                    App.Input(_DataGridResult, _EnterText.Text.TrimStart().ToUpper());
                    _EnterText.Clear();
                }
            }
        }
        private void _EnterText_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _EnterText.Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void _Get_Click(object sender, RoutedEventArgs e)
        {
        }
    }

}
