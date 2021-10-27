using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Input;

namespace LazyAdmin.Windows
{
    /// <summary>
    /// Interaction logic for _NavToolMenu.xaml
    /// </summary>
    public partial class _NavToolMenu : Window
    {
        private void Upload(object sender, RoutedEventArgs e)
        {
            App.Upload(_DataGridFromAMT);
        }
        private void EnterText(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift && e.Key == Key.Enter)
            {
                MessageBox.Show("!");
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
                    App.GetColumn(_DataGridResult.CurrentColumn.Header.ToString() + " Result");
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
        private void NoSerialNumber(object sender, RoutedEventArgs e)
        {
            _EnterText.Text = "No Serial Number";
            App.Input(_EnterText.Text.TrimStart().ToString().ToUpper());
            _EnterText.Clear();
        }

        private void StartFixing(object sender, RoutedEventArgs e)
        {
            if (_Start.Content == "Start")
            {
                App.UploadToFixing(_DataGridFixing);
                _Start.Content = "Cancel";
                _Finish.Visibility = Visibility.Visible;
                _DataGridFixing.Visibility = Visibility.Visible;
                _DataGridFromAMT.Visibility = Visibility.Hidden;
                _DataGridResult.Visibility = Visibility.Hidden;
            }
            else
            {
                _Start.Content = "Start";
                _Finish.Visibility = Visibility.Hidden;
                _DataGridFixing.Visibility = Visibility.Hidden;
                _DataGridFromAMT.Visibility = Visibility.Visible;
                _DataGridResult.Visibility = Visibility.Visible;
            }
        }
        private void FinishFixing(object sender, RoutedEventArgs e)
        {
            App.test();
        }
        
    } //buttons
    public partial class _NavToolMenu : Window //main functional
    {
        public _NavToolMenu()
        {
            InitializeComponent();
            App.WindowSettings(_HeaderButtonGrid, this);
            App.Load(_DataGridFromAMT, _DataGridResult);
        }
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ZIP files|*.zip";
            saveFileDialog.DefaultExt = "zip";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\DataBase");
                File.Copy($@"{Environment.CurrentDirectory}\GridOfAssetsFromAMT.json", $@"{Environment.CurrentDirectory}\DataBase\GridOfAssetsFromAMT.json", true);
                File.Copy($@"{Environment.CurrentDirectory}\GridOfAssetsResult.json", $@"{Environment.CurrentDirectory}\DataBase\GridOfAssetsResult.json", true);
                ZipFile.CreateFromDirectory($@"{Environment.CurrentDirectory}\DataBase", saveFileDialog.FileName);
                File.Delete($@"{Environment.CurrentDirectory}\DataBase\GridOfAssetsFromAMT.json");
                File.Delete($@"{Environment.CurrentDirectory}\DataBase\GridOfAssetsResult.json");
                Directory.Delete($@"{Environment.CurrentDirectory}\DataBase");
            }
        }
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ZIP files|*.zip";
            if (openFileDialog.ShowDialog() == true)
            {
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\OldDataBase");
                File.Move($@"{Environment.CurrentDirectory}\GridOfAssetsFromAMT.json", $@"{Environment.CurrentDirectory}\OldDataBase\GridOfAssetsFromAMT.json");
                File.Move($@"{Environment.CurrentDirectory}\GridOfAssetsResult.json", $@"{Environment.CurrentDirectory}\OldDataBase\GridOfAssetsResult.json");
                try
                {
                    ZipFile.ExtractToDirectory(openFileDialog.FileName, $@"{Environment.CurrentDirectory}");
                }
                catch
                {
                    File.Move($@"{Environment.CurrentDirectory}\OldDataBase\GridOfAssetsFromAMT.json", $@"{Environment.CurrentDirectory}\GridOfAssetsFromAMT.json");
                    File.Move($@"{Environment.CurrentDirectory}\OldDataBase\GridOfAssetsResult.json", $@"{Environment.CurrentDirectory}\GridOfAssetsResult.json");
                }
                File.Delete($@"{Environment.CurrentDirectory}\OldDataBase\GridOfAssetsFromAMT.json");
                File.Delete($@"{Environment.CurrentDirectory}\OldDataBase\GridOfAssetsResult.json");
                Directory.Delete($@"{Environment.CurrentDirectory}\OldDataBase");
            }
            App.Load(_DataGridFromAMT, _DataGridResult);
        }
    }
}