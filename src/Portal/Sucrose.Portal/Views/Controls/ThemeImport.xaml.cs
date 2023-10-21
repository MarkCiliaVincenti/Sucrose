﻿using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Controls;
using SPMI = Sucrose.Portal.Manage.Internal;
using SSDECT = Sucrose.Shared.Dependency.Enum.CompatibilityType;
using SSRER = Sucrose.Shared.Resources.Extension.Resources;

namespace Sucrose.Portal.Views.Controls
{
    /// <summary>
    /// ThemeImport.xaml etkileşim mantığı
    /// </summary>
    public partial class ThemeImport : ContentDialog, IDisposable
    {
        internal List<SSDECT> Types = new();
        internal List<string> Messages = new();

        public ThemeImport() : base(SPMI.ContentDialogService.GetContentPresenter())
        {
            InitializeComponent();
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if (Messages.Any())
            {
                MessageCard.Visibility = Visibility.Visible;

                Messages.ForEach(Message =>
                {
                    ThemeImportMessage.Children.Add(new TextBlock
                    {
                        Text = Message,
                        TextAlignment = TextAlignment.Left,
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Foreground = Types.Any(Type => Message.Contains($"'{Type}'.")) ? SSRER.GetResource<Brush>("SystemFillColorCriticalBrush") : SSRER.GetResource<Brush>("SystemFillColorSuccessBrush")
                    });
                });
            }

            if (Types.Any())
            {
                TypeCard.Visibility = Visibility.Visible;

                Types.ForEach(Type =>
                {
                    ThemeImportType.Children.Add(new TextBlock
                    {
                        TextAlignment = TextAlignment.Left,
                        TextWrapping = TextWrapping.WrapWithOverflow,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Foreground = SSRER.GetResource<Brush>("SystemFillColorCriticalBrush"),
                        Text = $"{Type}: {SSRER.GetValue("Portal", "Enum", "CompatibilityType", $"{Type}")}"
                    });
                });
            }

            await Task.Delay(10);

            Panel.MinHeight = 0;
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}