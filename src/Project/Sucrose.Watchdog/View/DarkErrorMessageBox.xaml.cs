﻿using System.Media;
using System.Windows;
using SMR = Sucrose.Memory.Readonly;
using SSDECT = Sucrose.Shared.Dependency.Enum.CommandsType;
using SSSHD = Sucrose.Shared.Space.Helper.Dark;
using SSSHP = Sucrose.Shared.Space.Helper.Processor;
using SSSMI = Sucrose.Shared.Space.Manage.Internal;
using SWHWI = Skylark.Wing.Helper.WindowInterop;

namespace Sucrose.Watchdog.View
{
    /// <summary>
    /// Interaction logic for DarkErrorMessageBox.xaml
    /// </summary>
    public partial class DarkErrorMessageBox : Window
    {
        private static string Path = string.Empty;
        private static string Text = string.Empty;
        private static string Source = string.Empty;

        public DarkErrorMessageBox(string ErrorMessage, string LogPath, string HelpSource = null, string HelpText = null)
        {
            InitializeComponent();

            Path = LogPath;
            Text = HelpText;
            Source = HelpSource;

            SystemSounds.Hand.Play();

            if (!string.IsNullOrEmpty(Source))
            {
                Help_Button.Content = Text;
            }

            Error_Message.Text += Environment.NewLine + ErrorMessage;

            SourceInitialized += DarkErrorMessageBox_SourceInitialized;
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            SSSHP.Run(SSSMI.Commandog, $"{SMR.StartCommand}{SSDECT.Log}{SMR.ValueSeparator}{Path}");
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Source))
            {
                SSSHP.Run(SSSMI.Commandog, $"{SMR.StartCommand}{SSDECT.Wiki}{SMR.ValueSeparator}{SMR.WikiWebsite}");
            }
            else
            {
                SSSHP.Run(SSSMI.Commandog, $"{SMR.StartCommand}{SSDECT.Wiki}{SMR.ValueSeparator}{Source}");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DarkErrorMessageBox_SourceInitialized(object sender, EventArgs e)
        {
            SSSHD.Apply(SWHWI.Handle(this));
        }

        private async void DarkErrorMessageBox_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(500);

            ShowInTaskbar = true;
        }
    }
}