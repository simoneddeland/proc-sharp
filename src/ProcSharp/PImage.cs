using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace ProcSharpCore
{
    public class PImage
    {
        internal IntPtr texture;
        internal int width;
        internal int height;

        internal PImage(IntPtr texture)
        {
            this.texture = texture;
            SDL_QueryTexture(texture, out _, out _, out width, out height);

        }
    }
}
