﻿using System.Windows;
using ClientBigBrother.ViewModel;

namespace ClientBigBrother.View
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModelMain vmMain;
        public MainWindow()
        {
            InitializeComponent();
            vmMain = new ViewModelMain();
        }

        private void SendInformationAboutUser_Click(object sender, RoutedEventArgs e)
        {
            vmMain.SendInformationAboutUser();
        }
    }
}