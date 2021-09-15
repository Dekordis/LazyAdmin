﻿using System;
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
        }

        private void Upload(object sender, RoutedEventArgs e)
        {
            App.Upload(_DataGridFromAMT);
        }
    }
}
