﻿using Newtonsoft.Json;
using SSSCEC = Sucrose.Shared.Space.Converter.ExceptionConverter;

namespace Sucrose.Shared.Space.Helper
{
    internal static class WatchException
    {
        public static Exception Convert(string Exception)
        {
            return JsonConvert.DeserializeObject<Exception>(Exception, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                Converters =
                {
                    new SSSCEC()
                }
            });
        }

        public static string Convert(Exception Exception, Formatting Formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(Exception, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting,
                Converters =
                {
                    new SSSCEC()
                }
            });
        }
    }
}