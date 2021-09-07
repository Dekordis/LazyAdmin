using System;
using System.Windows;
using LazyAdmin.Winodws;


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
        }
        private void TestButton(object sender, RoutedEventArgs e)
        {
            _TestWindow Test = new _TestWindow();
            Test.Show();
        }
    }
}
