using System;
using System.Windows;
using LazyAdmin.Windows;
using System.Reflection;
using System.Diagnostics;

namespace LazyAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            App.WindowSettings(_HeaderButtonGrid, this);
            _ProgramVersion.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(Assembly.GetExecutingAssembly().GetName().Version.ToString().Length - 4) + ".v";
            ChekUpdates();
        }

    }
    public partial class MainWindow : Window
    {
        private void CipboardPlus(object sender, RoutedEventArgs e)
        {
            App.ChangeClipboard("Add comment");
        }

        private void NoComment(object sender, RoutedEventArgs e)
        {
            App.ChangeClipboard("No comment");
        }
        private void OpenNensOnBoardMenu(object sender, RoutedEventArgs e)
        {
            _NensOnBoardMenu NensOnBoardMenu = new();
            App.OpenWindow(this, NensOnBoardMenu);
        }

        private void OpenToolsMenu(object sender, RoutedEventArgs e)
        {
            _ToolsMenu ToolsMenu = new();
            App.OpenWindow(this, ToolsMenu);
        }

        private void OpenNavToolMenu(object sender, RoutedEventArgs e)
        {
            _NavToolMenu NavToolMenu = new();
            App.OpenWindow(this, NavToolMenu);
        }

        private void OpenMacrosMenu(object sender, RoutedEventArgs e)
        {
            _MacrosMenu MacrosMenu = new();
            App.OpenWindow(this, MacrosMenu);
        }

        private void OpenLinksMenu(object sender, RoutedEventArgs e)
        {
            _LinksMenu LinksMenu = new();
            App.OpenWindow(this, LinksMenu);
        }
        private void OpenOptionsMenu(object sender, RoutedEventArgs e)
        {
            _OptionsMenu OptionsMenu = new();
            App.OpenWindow(this, OptionsMenu);

        }
        private void HideAndMinimize(object sender, RoutedEventArgs e) //Minimize in small window with ClipBoard instrument
        {
            if (_NensOnBoard.Visibility == Visibility.Visible)
            {
                _NensOnBoard.Visibility = Visibility.Hidden;
                _NavTool.Visibility = Visibility.Hidden;
                _Links.Visibility = Visibility.Hidden;
                _Macros.Visibility = Visibility.Hidden;
                _Tools.Visibility = Visibility.Hidden;
                ClipBoardWithComment.HorizontalAlignment = HorizontalAlignment.Left;
                ClipBoardWithoutComment.HorizontalAlignment = HorizontalAlignment.Left;
                ClipBoardWithComment.Margin = new Thickness(85, 0, 0, 0);
                ClipBoardWithoutComment.Margin = new Thickness(340, 0, 0, 0);//grid h 60 main h 40
                ClipBoardWithComment.Height = 40;
                ClipBoardWithoutComment.Height = 40;
                _MainGrid.Height = 40;
                _MainGrid.Width = 625;
                _MainWindow.Height = 40;
                _MainWindow.Topmost = true;
            }
            else
            {
                _NensOnBoard.Visibility = Visibility.Visible;
                _NavTool.Visibility = Visibility.Visible;
                _Links.Visibility = Visibility.Visible;
                _Macros.Visibility = Visibility.Visible;
                _Tools.Visibility = Visibility.Visible;
                ClipBoardWithComment.HorizontalAlignment = HorizontalAlignment.Center;
                ClipBoardWithoutComment.HorizontalAlignment = HorizontalAlignment.Center;
                ClipBoardWithComment.Margin = new Thickness(0, 150, 0, 0);
                ClipBoardWithoutComment.Margin = new Thickness(0, 214, 0, 0);
                ClipBoardWithComment.Height = 64;
                ClipBoardWithoutComment.Height = 64;
                _MainGrid.Height = 434.04;
                _MainGrid.Width = 800;
                _MainWindow.Height = 250;
                _MainWindow.Topmost = false;
            }
        }

    } //    "Buttons"
    public partial class MainWindow
    {
        readonly string UpdatePath = @"\\cklfsstorage.file.core.windows.net\remoteinstall\LAZYADMIN\Prod\LazyAdmin\";
        string CheckVersion;
        private void ChekUpdates()
        {
            try
            {
                App.AzureConnection("Connect");
                FileVersionInfo ProdVersion = FileVersionInfo.GetVersionInfo($@"{UpdatePath}" + "LazyAdmin.exe");
                CheckVersion = ProdVersion.FileVersion;
                if (Assembly.GetExecutingAssembly().GetName().Version.ToString() == CheckVersion)
                {
                }
                else if (Assembly.GetExecutingAssembly().GetName().Version.ToString() != CheckVersion)
                {
                    string[] LocalStringCheck = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
                    string[] RemoteStringCheck = CheckVersion.Split('.');
                    int LocalCheck1 = Int32.Parse(LocalStringCheck[0]);
                    int LocalCheck2 = Int32.Parse(LocalStringCheck[1]);
                    int RemoteCheck1 = Int32.Parse(RemoteStringCheck[0]);
                    int RemoteCheck2 = Int32.Parse(RemoteStringCheck[1]);
                    if (LocalCheck1 < RemoteCheck1 || (LocalCheck2 < RemoteCheck2 && LocalCheck1 <= RemoteCheck1))
                    {
                        Update();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private static void Update()
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Lazy Admin has new version!\ndo you want update?", "Updater", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Process update = new();
                    update.StartInfo.FileName = $"Update.bat";
                    update.Start();
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

    }
}
