using System;
using System.Linq;
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
using LazyAdmin.Windows;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace LazyAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string DesignParametr = "None";
        public MainWindow()
        {
            InitializeComponent();
            App.WindowSettings(_HeaderButtonGrid, this);
        }

        private void CipboardPlus(object sender, RoutedEventArgs e)
        {
            App.ChangeClipboard("Add comment");
        }

        private void NoComment(object sender, RoutedEventArgs e)
        {
            App.ChangeClipboard("No comment");
        }
    }
    public partial class MainWindow : Window
    {
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

        private void OpenInDevMenu(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ha-ha");
            MessageBox.Show("Program will be shutdowned after 5 sec");
            MessageBox.Show("Joke\nHa-ha");
            Close();
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
}
