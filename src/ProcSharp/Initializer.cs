using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using static SDL2.SDL;
using static SDL2.SDL_mixer;
using static SDL2.SDL_ttf;

namespace ProcSharpCore
{
    class Initializer
    {

        internal void Initialize(ref IntPtr window, ref IntPtr renderer)
        {
            if (SDL_Init(SDL_INIT_EVERYTHING) < 0)
            {
                throw new Exception(SDL_GetError());
            }

            window = SDL_CreateWindow("ProcSharp",
                SDL_WINDOWPOS_CENTERED,
                SDL_WINDOWPOS_CENTERED,
                100,
                100,
                SDL_WindowFlags.SDL_WINDOW_RESIZABLE
            );

            renderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            SDL_SetRenderDrawColor(renderer, 100, 100, 100, 255);

            if (window == IntPtr.Zero)
            {
                throw new Exception($"Unable to create a window. SDL. Error: {SDL_GetError()}");
            }
            
            InitializeAudio();

            if (TTF_Init() == -1)
            {
                throw new Exception("Could not initialize TTF: " + SDL_GetError());
            }

        }

        private void InitializeAudio()
        {
            if (Mix_OpenAudio(MIX_DEFAULT_FREQUENCY, MIX_DEFAULT_FORMAT, MIX_DEFAULT_CHANNELS, 2048) < 0)
            {
                throw new Exception($"Unable to initialize audio: {SDL_GetError()}");
            }
        }
    }
}
