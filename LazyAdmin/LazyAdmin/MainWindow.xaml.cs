﻿using System;
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
        private void TestButton(object sender, RoutedEventArgs e)
        {
            _TestWindow TestWindow = new _TestWindow();
            App.OpenWindow(this, TestWindow);
        }

        private void _SetupSoftware_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
