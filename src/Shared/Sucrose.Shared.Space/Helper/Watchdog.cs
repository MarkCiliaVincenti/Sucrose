﻿using System.IO;
using SMR = Sucrose.Memory.Readonly;
using SSDECT = Sucrose.Shared.Dependency.Enum.CommandsType;
using SSECCE = Skylark.Standard.Extension.Cryptology.CryptologyExtension;
using SSSHP = Sucrose.Shared.Space.Helper.Processor;
using SSSHWE = Sucrose.Shared.Space.Helper.WatchException;
using SSSMI = Sucrose.Shared.Space.Manage.Internal;

namespace Sucrose.Shared.Space.Helper
{
    internal static class Watchdog
    {
        public static string Read(string FilePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            }

            using FileStream FileStream = new(FilePath, FileMode.Open, FileAccess.Read, FileShare.None);
            using StreamReader Reader = new(FileStream);

            return Reader.ReadToEnd();
        }

        public static void Write(string FilePath, string FileContent)
        {
            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            }

            using FileStream FileStream = new(FilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            using StreamWriter Writer = new(FileStream);

            Writer.Write(FileContent);
        }

        public static void Start(string Application, Exception Exception, string Path)
        {
            if (Check())
            {
                SSSHP.Run(SSSMI.Commandog, $"{SMR.StartCommand}{SSDECT.Watchdog}{SMR.ValueSeparator}{SSSMI.Watchdog}{SMR.ValueSeparator}{Encrypt(Application, Exception, Path)}");
            }
        }

        public static void Start(string Application, Exception Exception, string Path, string Source, string Text)
        {
            if (Check())
            {
                if (string.IsNullOrEmpty(Source) || string.IsNullOrEmpty(Text))
                {
                    Start(Application, Exception, Path);
                }
                else
                {
                    SSSHP.Run(SSSMI.Commandog, $"{SMR.StartCommand}{SSDECT.Watchdog}{SMR.ValueSeparator}{SSSMI.Watchdog}{SMR.ValueSeparator}{Encrypt(Application, Exception, Path, Source, Text)}");
                }
            }
        }

        private static bool Check()
        {
            return File.Exists(SSSMI.Commandog) && File.Exists(SSSMI.Watchdog);
        }

        private static string Encrypt(string Application, Exception Exception, string Path)
        {
            return SSECCE.TextToBase($"{Application}{SMR.ValueSeparator}{SSSHWE.Convert(Exception)}{SMR.ValueSeparator}{Path}");
        }

        private static string Encrypt(string Application, Exception Exception, string Path, string Source, string Text)
        {
            return SSECCE.TextToBase($"{Application}{SMR.ValueSeparator}{SSSHWE.Convert(Exception)}{SMR.ValueSeparator}{Path}{SMR.ValueSeparator}{Source}{SMR.ValueSeparator}{Text}");
        }
    }
}