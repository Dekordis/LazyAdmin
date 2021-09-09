﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LazyAdmin
{
    partial class App
    {
        static public void WindowSettings(Grid HeaderGrid, Window CurrentWindow)
        {
            WindowDesign(CurrentWindow);
            Button _CloseWindow = new Button();
            _CloseWindow.Height = 25;
            _CloseWindow.Width = 35;
            _CloseWindow.Name = "_CloseWindow";
            _CloseWindow.Background = Brushes.Transparent;
            _CloseWindow.BorderBrush = Brushes.Transparent;
            _CloseWindow.Click += (s, ee) => {CloseThisWindow(CurrentWindow); };
            _CloseWindow.VerticalAlignment = VerticalAlignment.Top;
            _CloseWindow.HorizontalAlignment = HorizontalAlignment.Left;
            Label _CloseButtonText = new Label();
            _CloseButtonText.Content = "X";
            _CloseButtonText.FontFamily = new FontFamily("Century Gothic");
            _CloseButtonText.FontSize = 20;
            _CloseButtonText.FontWeight = FontWeights.UltraBold;
            _CloseButtonText.Margin = new Thickness(0, -5, 0, -5);
            _CloseButtonText.Foreground = Brushes.White;
            _CloseWindow.Content = _CloseButtonText;
            HeaderGrid.Children.Add(_CloseWindow);

            Button _MinimizeWindow = new Button();
            _MinimizeWindow.Margin = new Thickness(0, 0, 700, 0);
            _MinimizeWindow.Height = 25;
            _MinimizeWindow.Width = 35;
            _MinimizeWindow.Name = "_MinimizeWindow";
            _MinimizeWindow.Background = Brushes.Transparent;
            _MinimizeWindow.BorderBrush = Brushes.Transparent;
            _MinimizeWindow.Click += (s, ee) => { MinimizeThisWindow(CurrentWindow); }; 
            _MinimizeWindow.VerticalAlignment = VerticalAlignment.Top;
            Label _MinimizeButtonText = new Label();
            _MinimizeButtonText.Content = "_";
            _MinimizeButtonText.FontFamily = new FontFamily("Century Gothic");
            _MinimizeButtonText.FontSize = 30;
            _MinimizeButtonText.FontWeight = FontWeights.UltraBold;
            _MinimizeButtonText.Margin = new Thickness(0, -30, 0, -30);
            _MinimizeButtonText.Foreground = Brushes.White;
            _MinimizeWindow.Content = _MinimizeButtonText;
            HeaderGrid.Children.Add(_MinimizeWindow);
            CurrentWindow.MouseLeftButtonDown += (s, ee) => { MoveWindow(CurrentWindow); };
        }
        static public void CloseThisWindow(Window CurrentWindow)
        {
            CurrentWindow.Close();
            Window MainWindow = Application.Current.MainWindow;
            try
            {   
             MainWindow.Left = CurrentWindow.Left;
             MainWindow.Top = CurrentWindow.Top;
             MainWindow.Show();
            }
            catch(NullReferenceException)
            {
            }
            catch (Exception)
            {
            }

        }
        static public void MinimizeThisWindow(Window CurrentWindow)
        {
            CurrentWindow.WindowState = System.Windows.WindowState.Minimized;
        }
        static public void MoveWindow(Window CurrentWindow)
        {
            CurrentWindow.DragMove();
        }
        static public void WindowDesign(Window CurrentWindow)
        {
            CurrentWindow.AllowsTransparency = true;
            CurrentWindow.WindowStyle = WindowStyle.None;
            CurrentWindow.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF43494F");
            CurrentWindow.WindowStartupLocation = WindowStartupLocation.Manual;

        }
        static public void OpenWindow(Window CurrentWindow, Window TargetWindow)
        {
            TargetWindow.Left = CurrentWindow.Left;
            TargetWindow.Top = CurrentWindow.Top;
            TargetWindow.Show();
            CurrentWindow.Hide();
        }

    }
}