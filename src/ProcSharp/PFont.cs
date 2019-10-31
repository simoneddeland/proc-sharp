using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;
using static SDL2.SDL_image;
using static SDL2.SDL_ttf;

namespace ProcSharpCore
{
    public class PFont
    {


        internal static List<PFont> fonts = new List<PFont>();

        IntPtr font;

        internal static string contentFolder = "content/";

        IntPtr renderer;

        IntPtr textSurface;
        IntPtr textTexture;
        string lastText = "";

        internal PFont(string fontname, int size, IntPtr renderer)
        {
            font = TTF_OpenFont(contentFolder + fontname, size);
            this.renderer = renderer;

            if (font == IntPtr.Zero)
            {
                throw new Exception("Could not load font " + fontname + ", " + SDL_GetError());
            }

            fonts.Add(this);
        }

        internal void Text(string text, int x, int y)
        {

            if (text == "")
            {
                return;
            }

            if (lastText != text)
            {
                SDL_Color color = new SDL_Color();
                color.a = 255;
                color.r = 0;
                color.g = 0;
                color.b = 0;


                SDL_FreeSurface(textSurface);
                SDL_DestroyTexture(textTexture);

                textSurface = TTF_RenderText_Solid(font, text, color);

                if (textSurface == IntPtr.Zero)
                {
                    throw new Exception("Could not render text to surface: " + SDL_GetError());
                }

                textTexture = SDL_CreateTextureFromSurface(renderer, textSurface);

                if (textTexture == IntPtr.Zero)
                {
                    throw new Exception("Could not create a texture from the font surface: " + SDL_GetError());
                }

                lastText = text;
            }

            SDL_Rect targetRect = new SDL_Rect();
            targetRect.x = x;
            targetRect.y = y;
            SDL_QueryTexture(textTexture, out _, out _, out targetRect.w, out targetRect.h);

            SDL_RenderCopy(renderer, textTexture, IntPtr.Zero, ref targetRect);
        }

        internal void Destroy()
        {
            SDL_FreeSurface(textSurface);
            SDL_DestroyTexture(textTexture);
            TTF_CloseFont(font);
        }
    }
}
