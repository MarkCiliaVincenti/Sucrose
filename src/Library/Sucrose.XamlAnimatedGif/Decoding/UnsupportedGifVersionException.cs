﻿using System.Runtime.Serialization;

namespace Sucrose.XamlAnimatedGif.Decoding
{
    [Serializable]
    public class UnsupportedGifVersionException : GifDecoderException
    {
        internal UnsupportedGifVersionException(string message) : base(message) { }
        internal UnsupportedGifVersionException(string message, Exception inner) : base(message, inner) { }

        protected UnsupportedGifVersionException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        { }
    }
}