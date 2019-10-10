using System;
using SDL2;
using static SDL2.SDL;

namespace ProcSharpCore
{
    public class ProcSharp
    {
        private static IntPtr window;
        private static IntPtr renderer;

        private static Type gameType;
        private static object gameObject;

        private static bool quit = false;

        private static Initializer initializer;

        public static void Initialize(Type type, object game)
        {
            gameType = type;
            gameObject = game;

            initializer = new Initializer();
            initializer.Initialize(ref window, ref renderer);

            var userSetup = gameType.GetMethod("Setup");

            // Call setup from the user
            userSetup?.Invoke(gameObject, null);

            SDL_RenderClear(renderer);
            SDL_RenderPresent(renderer);

            // Start the main loop
            MainLoop();
        }

        private static void MainLoop()
        {

            var userDraw = gameType.GetMethod("Draw");
            SDL_Event e;

            while (!quit)
            {
                while (SDL_PollEvent(out e) != 0)
                {
                    switch (e.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            quit = true;
                            break;

                        case SDL_EventType.SDL_KEYDOWN:
                            switch (e.key.keysym.sym)
                            {
                                case SDL_Keycode.SDLK_q:
                                    quit = true;
                                    break;
                            }
                            break;
                    }

                    // Call the users draw function
                    userDraw?.Invoke(gameObject, null);
                    SDL_RenderPresent(renderer);

                }
            }

            InternalExit();
        }

        private static void InternalExit()
        {
            // CLEANUP
            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            SDL_Quit();
        }



        #region Public methods
        public static void Color(float r, float g, float b, float a)
        {
            SDL_SetRenderDrawColor(renderer, Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b), Convert.ToByte(a));
        }



        public static void Exit()
        {
            quit = true;
        }

        public static int MouseX
        {
            get
            {
                int x;
                int y;
                SDL_GetMouseState(out x, out y);
                return x;
            }
        }

        public static int MouseY
        {
            get
            {
                int x;
                int y;
                SDL_GetMouseState(out x, out y);
                return y;
            }
        }

        public static void Square(float x, float y, float extent)
        {
            SDL_Rect drawRect;
            drawRect.x = (int)x;
            drawRect.y = (int)y;
            drawRect.h = (int)extent;
            drawRect.w = (int)extent;
            SDL_RenderDrawRect(renderer, ref drawRect);
        }

        public static void Background(float v1, float v2, float v3)
        {
            byte currentRed;
            byte currentGreen;
            byte currentBlue;
            byte currentAlpha;
            SDL_GetRenderDrawColor(renderer, out currentRed, out currentGreen, out currentBlue, out currentAlpha);
            SDL_SetRenderDrawColor(renderer, Convert.ToByte(v1), Convert.ToByte(v2), Convert.ToByte(v3), 255);
            SDL_RenderClear(renderer);
            SDL_SetRenderDrawColor(renderer, currentRed, currentGreen, currentBlue, currentAlpha);

        }

        #endregion
    }
}
