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
    /// Interaction logic for _TestWindow.xaml
    /// </summary>
    public partial class _TestWindow : Window
    {
        public _TestWindow()
        {
            InitializeComponent();
            App.HeaderRender(_HeaderButtonGrid, this);
        }
        private void TestButton(object sender, RoutedEventArgs e)
        {
            string Path = @"C:\test";
            string TypeOfRun = "Run several scripts";
            App.RunScript(Path, TypeOfRun);
        }

    }
}
