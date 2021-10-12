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
            _ProgramVersion.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString()+".v";
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
            _NensOnBoardMenu NensOnBoardMenu = new _NensOnBoardMenu();
            App.OpenWindow(this, NensOnBoardMenu);
        }
        private void OpenSetupSoftwareMenu(object sender, RoutedEventArgs e)
        {
            _SetupSoftwareMenu SetupSoftwareMenu = new _SetupSoftwareMenu();
            App.OpenWindow(this, SetupSoftwareMenu);
        }

        private void OpenToolsMenu(object sender, RoutedEventArgs e)
        {
            _ToolsMenu ToolsMenu = new _ToolsMenu();
            App.OpenWindow(this, ToolsMenu);
        }

        private void OpenNavToolMenu(object sender, RoutedEventArgs e)
        {
            _NavToolMenu NavToolMenu = new _NavToolMenu();
            App.OpenWindow(this, NavToolMenu);
        }

        private void OpenTestMenu(object sender, RoutedEventArgs e)
        {
            _TestWindowMenu TestWindowMenu = new _TestWindowMenu();
            App.OpenWindow(this, TestWindowMenu);
        }

        private void OpenMacrosMenu(object sender, RoutedEventArgs e)
        {
            _MacrosMenu MacrosMenu = new _MacrosMenu();
            App.OpenWindow(this, MacrosMenu);
        }

        private void OpenLinksMenu(object sender, RoutedEventArgs e)
        {
            _LinksMenu LinksMenu = new _LinksMenu();
            App.OpenWindow(this, LinksMenu);
        }

        private void OpenHotkeysMenu(object sender, RoutedEventArgs e)
        {
            _HotkeysMenu HotkeysMenu = new _HotkeysMenu();
            App.OpenWindow(this, HotkeysMenu);
        }

        private void OpenOptionsMenu(object sender, RoutedEventArgs e)
        {
            _OptionsMenu OptionsMenu = new _OptionsMenu();
            App.OpenWindow(this, OptionsMenu);

        }
    } //    "Buttons"
    public partial class MainWindow
    {
        string UpdatePath = @"\\cklfsstorage.file.core.windows.net\remoteinstall\LAZYADMIN\Prod\";
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
                    int LocalCheck3 = Int32.Parse(LocalStringCheck[2]);
                    int LocalCheck4 = Int32.Parse(LocalStringCheck[3]);
                    int RemoteCheck1 = Int32.Parse(RemoteStringCheck[0]);
                    int RemoteCheck2 = Int32.Parse(RemoteStringCheck[1]);
                    int RemoteCheck3 = Int32.Parse(RemoteStringCheck[2]);
                    int RemoteCheck4 = Int32.Parse(RemoteStringCheck[3]);
                    if (LocalCheck1 < RemoteCheck1 || (LocalCheck2 < RemoteCheck2 && LocalCheck1 <= RemoteCheck1) || (LocalCheck3 < RemoteCheck3 && (LocalCheck2 <= RemoteCheck2 && LocalCheck1 <= RemoteCheck1)) || (LocalCheck4 < RemoteCheck4 && (LocalCheck3 <= RemoteCheck3 && LocalCheck2 <= RemoteCheck2 && LocalCheck1 <= RemoteCheck1)))
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
        private void Update()
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Lazy Admin has new version!\ndo you want update?", "Updater", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Process update = new Process();
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
