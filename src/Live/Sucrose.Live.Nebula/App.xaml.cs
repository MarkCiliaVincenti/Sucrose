﻿using System.Globalization;
using System.IO;
using System.Windows;
using Application = System.Windows.Application;
using SHC = Skylark.Helper.Culture;
using SMC = Sucrose.Memory.Constant;
using SMMI = Sucrose.Manager.Manage.Internal;
using SMMM = Sucrose.Manager.Manage.Manager;
using SMR = Sucrose.Memory.Readonly;
using SRHR = Sucrose.Resources.Helper.Resources;
using SSDEWT = Sucrose.Shared.Dependency.Enum.WallpaperType;
using SSEHC = Sucrose.Shared.Engine.Helper.Cycyling;
using SSEHR = Sucrose.Shared.Engine.Helper.Run;
using SSEMI = Sucrose.Shared.Engine.Manage.Internal;
using SSENVV = Sucrose.Shared.Engine.Nebula.View.Video;
using SSSHC = Sucrose.Shared.Space.Helper.Cycyling;
using SSSHI = Sucrose.Shared.Space.Helper.Instance;
using SSSHS = Sucrose.Shared.Space.Helper.Security;
using SSSHW = Sucrose.Shared.Space.Helper.Watchdog;
using SSTHI = Sucrose.Shared.Theme.Helper.Info;
using SSTHV = Sucrose.Shared.Theme.Helper.Various;
using SSWW = Sucrose.Shared.Watchdog.Watch;

namespace Sucrose.Live.Nebula
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static bool HasError { get; set; } = true;

        public App()
        {
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);

            System.Windows.Forms.Application.ThreadException += async (s, e) =>
            {
                Exception Exception = e.Exception;

                await SSWW.Watch_ThreadException(Exception);

                //Close();
                Message(Exception);
            };

            AppDomain.CurrentDomain.FirstChanceException += async (s, e) =>
            {
                Exception Exception = e.Exception;

                await SSWW.Watch_FirstChanceException(Exception);

                //Close();
                //Message(Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += async (s, e) =>
            {
                Exception Exception = (Exception)e.ExceptionObject;

                await SSWW.Watch_GlobalUnhandledException(Exception);

                //Close();
                Message(Exception);
            };

            TaskScheduler.UnobservedTaskException += async (s, e) =>
            {
                Exception Exception = e.Exception;

                await SSWW.Watch_UnobservedTaskException(Exception);

                e.SetObserved();

                //Close();
                Message(Exception);
            };

            Current.DispatcherUnhandledException += async (s, e) =>
            {
                Exception Exception = e.Exception;

                await SSWW.Watch_DispatcherUnhandledException(Exception);

                e.Handled = true;

                //Close();
                Message(Exception);
            };

            SHC.All = new CultureInfo(SMMM.Culture, true);
        }

        protected void Close()
        {
            Environment.Exit(0);
            Current.Shutdown();
            Shutdown();
        }

        protected void Message(Exception Exception)
        {
            if (HasError)
            {
                HasError = false;

                string Path = SMMI.NebulaLiveLogManager.LogFile();

                SSSHW.Start(SMR.NebulaLive, Exception, Path);

                Close();
            }
        }

        protected void Configure()
        {
            SSEMI.LibraryLocation = SMMM.LibraryLocation;
            SSEMI.LibrarySelected = SMMM.LibrarySelected;

            if (SMMI.LibrarySettingManager.CheckFile() && !string.IsNullOrEmpty(SSEMI.LibrarySelected))
            {
                SSEMI.InfoPath = Path.Combine(SSEMI.LibraryLocation, SSEMI.LibrarySelected, SMR.SucroseInfo);

                if (File.Exists(SSEMI.InfoPath) && SSTHI.CheckJson(SSTHI.ReadInfo(SSEMI.InfoPath)))
                {
                    SSEMI.Info = SSTHI.ReadJson(SSEMI.InfoPath);

                    string Source = SSEMI.Info.Source;

                    if (!SSTHV.IsUrl(Source))
                    {
                        Source = Path.Combine(SSEMI.LibraryLocation, SSEMI.LibrarySelected, Source);
                    }

                    SMMI.BackgroundogSettingManager.SetSetting(new KeyValuePair<string, bool>[]
                    {
                        new(SMC.PipeRequired, false),
                        new(SMC.AudioRequired, false),
                        new(SMC.SignalRequired, false),
                        new(SMC.PausePerformance, false)
                    });

                    if (SSTHV.IsUrl(Source) || File.Exists(Source))
                    {
                        SSSHS.Apply();

                        SSEHC.Start();

                        switch (SSEMI.Info.Type)
                        {
                            case SSDEWT.Video:
                                SSENVV Video = new(Source);
                                Video.Show();
                                break;
                            default:
                                Close();
                                break;
                        }
                    }
                    else
                    {
                        Close();
                    }
                }
                else
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            //

            Close();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SRHR.SetLanguage(SMMM.Culture);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            if (SSSHI.Basic(SMR.LiveMutex, SMR.NebulaLive) && SSEHR.Check())
            {
                if (SSSHC.Check())
                {
                    SSSHC.Change();
                }
                else
                {
                    Configure();
                }
            }
            else
            {
                Close();
            }
        }
    }
}