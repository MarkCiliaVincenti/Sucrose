﻿using System.Windows;
using SSECSEYT = Sucrose.Shared.Engine.CefSharp.Event.YouTube;
using SSECSHCCM = Sucrose.Shared.Engine.CefSharp.Handler.CustomContextMenu;
using SSECSHYT = Sucrose.Shared.Engine.CefSharp.Helper.YouTube;
using SSECSMI = Sucrose.Shared.Engine.CefSharp.Manage.Internal;
using SSEEH = Sucrose.Shared.Engine.Event.Handler;
using SSEHD = Sucrose.Shared.Engine.Helper.Data;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;

namespace Sucrose.Shared.Engine.CefSharp.View
{
    /// <summary>
    /// Interaction logic for YouTube.xaml
    /// </summary>
    public sealed partial class YouTube : Window
    {
        public YouTube(string YouTube)
        {
            InitializeComponent();

            ContentRendered += (s, e) => SSEEH.ContentRendered(this);

            SSECSMI.CefEngine.MenuHandler = new SSECSHCCM();

            Content = SSECSMI.CefEngine;

            SSECSMI.YouTube = YouTube;

            SSECSMI.CefEngine.BrowserSettings = SSECSMI.CefSettings;

            SSEMI.GeneralTimer.Tick += new EventHandler(GeneralTimer_Tick);
            SSEMI.GeneralTimer.Interval = new TimeSpan(0, 0, 1);
            SSEMI.GeneralTimer.Start();

            SSECSMI.CefEngine.IsBrowserInitializedChanged += SSECSEYT.CefEngineInitializedChanged;
            SSECSMI.CefEngine.FrameLoadEnd += SSECSEYT.CefEngineFrameLoadEnd;
            SSECSMI.CefEngine.Loaded += SSECSEYT.CefEngineLoaded;

            Closing += (s, e) => SSECSMI.CefEngine.Dispose();
            Loaded += (s, e) => SSEEH.WindowLoaded(this);
        }

        private void GeneralTimer_Tick(object sender, EventArgs e)
        {
            if (SSEMI.Initialized)
            {
                SSECSHYT.First();

                SSECSHYT.SetLoop(SSEHD.GetLoop());

                SSECSHYT.SetVolume(SSEHD.GetVolume());

                SSECSHYT.SetShuffle(SSEHD.GetShuffle());
            }
        }
    }
}