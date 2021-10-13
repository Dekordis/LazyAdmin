using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;


namespace LazyAdmin.Windows
{
    /// <summary>
    /// Interaction logic for _NavToolMenu.xaml
    /// </summary>
    public partial class _NavToolMenu : Window
    {
        public _NavToolMenu()
        {
            InitializeComponent();
            App.WindowSettings(_HeaderButtonGrid, this);
            App.Load(_DataGridFromAMT, _DataGridResult);
        }
        private void Upload(object sender, RoutedEventArgs e)
        {
            App.Upload(_DataGridFromAMT);
        }
        private void EnterText(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift && e.Key == Key.Enter)
            {
                string Search = _EnterText.Text;
                MessageBox.Show("You press Shift+Enter");
            }
            else if (e.Key == Key.Enter)
            {
                string[] EnteredTextCheking = _EnterText.Text.Split(',');
                if (EnteredTextCheking.Length > 1)
                {
                    foreach (string element in EnteredTextCheking)
                    {
                        if (element.Length == 10)
                        {
                            _EnterText.Text = element;
                        }
                    }
                }
                if (_EnterText.Text == "" || _EnterText.Text == null || _EnterText.Text == " ") _EnterText.Clear();
                else if (_SendingEquipment.IsChecked == false)
                {
                    App.Input(_EnterText.Text.TrimStart().ToString().ToUpper());
                    _EnterText.Clear();
                }
                else
                {
                    App.InputSending(_DataGridResult, _EnterText.Text.ToString().TrimStart().ToUpper());
                    _EnterText.Clear();
                }
            }
        }
        private void ClearTextInTextBox(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _EnterText.Clear();
            }
        }
        private void ButtonFinishSending(object sender, RoutedEventArgs e)
        {
            App.FinishSending();
        }

        private void ButtonClearAll(object sender, RoutedEventArgs e)
        {
            App.ClearAssets();
        }

        private void GetCiklumID(object sender, RoutedEventArgs e)
        {
            App.GetAllCiklumID();
        }
        private void GetSerialnumber(object sender, RoutedEventArgs e)
        {
            App.GetAllSerialNumber();
        }
        private void GetAll(object sender, RoutedEventArgs e)
        {
            App.GetAll();
        }

        private void ResultCopyColumn(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (_DataGridResult.CurrentColumn.Header.ToString() != null)
                {
                    App.GetColumn(_DataGridResult.CurrentColumn.Header.ToString() +" Result");
                }
            }
        }
        private void FromAMTCopyColumn(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (_DataGridFromAMT.CurrentColumn.Header.ToString() != null)
                {
                    App.GetColumn(_DataGridFromAMT.CurrentColumn.Header.ToString() + " From AMT");
                }
            }
        }
        private static void Highlight()
        {

        }
        private void NoSerialNumber(object sender, RoutedEventArgs e)
        {
            _EnterText.Text = "No Serial Number";
            App.Input(_EnterText.Text.TrimStart().ToString().ToUpper());
            _EnterText.Clear();
        }
    }

}
